using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Elastic.Transport;
using ElasticsearchInAction.Api.Repositories;
using ElasticsearchInAction.Repositories.Elasticsearch.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;
using ElasticsearchInAction.Repositories.Elasticsearch.Converters;
using ElasticsearchInAction.Repositories.Elasticsearch.Models.Query;

namespace ElasticsearchInAction.Repositories.Elasticsearch;

public class ElasticRepository : IElasticRepository
{

    private readonly ILogger<IElasticRepository> _logger;
    private readonly ElasticsearchClient _client;

    private readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
        Converters =
                {
                    new QueryTypeJsonConverter(),
                    new RangeQueryJsonConverter<double?>()
                }
    };

    public ElasticRepository(IOptions<ElasticOptions> options, ILogger<IElasticRepository> logger)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(logger);
        
        _logger = logger;

        _client = ElasticsearchClientFactory.Create(
            options.Value.Host,
            options.Value.User,
            options.Value.Password,
            options.Value.IndexName);
    }

    public async Task<long> GetCount(CancellationToken cancellationToken = default)
    {
        var response = await _client.CountAsync(
            descriptor => descriptor.Indices(_client.ElasticsearchClientSettings.DefaultIndex),
            cancellationToken);
        
        // If I need to get answer from many indices
        // var response = await _client.CountAsync(
        //     descriptor => descriptor.Indices(Indices.Index("index1").And("index2")),
        //     cancellationToken);
        if (response.IsSuccess())
        {
            return response.Count;
        }
        
        // It must never be executed, because IndexAsync throw exception if something goes wrong
        throw new InvalidOperationException("Something was wrong.");
    }

    public async Task<BookResponse?> Get(string id,CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);

        var response = await _client.GetAsync<Book>(new Id(id), cancellationToken);

        if (response.IsSuccess())
        {
            if (response.Found)
            {
                _logger.LogInformation("Document with Id {DocumentID} found", response.Id);
                return new BookResponse
                {
                    Id = response.Id,
                    Score = 1,
                    Book = response.Source!
                };
            }

            return null;
        }
        
        // It must never be executed, because IndexAsync throw exception if something goes wrong
        throw new InvalidOperationException("Something was wrong.");
    }
    
    public async Task<BookResponse[]> Get(string[] ids, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(ids);
        
        if (ids.Length == 0)
        {
            return Array.Empty<BookResponse>();
        }
        
        var idsQuery = new IdsQuery
        {
            Values = ids
        };

        var response = await _client.SearchAsync<Book>(
            config => config.Query(idsQuery),
            cancellationToken);
        
        // Fluent DSL variant
        // var response = await _client.SearchAsync<Book>(
        //     config => 
        //         config.Query(descriptor =>
        //             descriptor.Ids(q => q.Values(ids))),
        //     cancellationToken);

        if (response.IsSuccess())
        {
            _logger.LogInformation(
                "Get all documents. Received {DocumentsCount} documents",
                response.Documents.Count);
            return response.Hits.Select(x =>
                    new BookResponse
                    {
                        Id = x.Id,
                        Score = x.Score!.Value,
                        Book = x.Source!
                    })
                .ToArray();
        }
        
        // It must never be executed, because IndexAsync throw exception if something goes wrong
        throw new InvalidOperationException("Something was wrong.");
    }
    
    public async Task<BookResponse[]> GetAll(CancellationToken cancellationToken = default)
    {
        var response = await _client.SearchAsync<Book>(cancellationToken);
        
        // The same result
        // var response = await _client.SearchAsync<Book>(
        //     config => 
        //         config.Query(descriptor => descriptor.MatchAll()),
        //     cancellationToken);

        if (response.IsSuccess())
        {
            _logger.LogInformation(
                "Get all documents. Received {DocumentsCount} documents",
                response.Documents.Count);
            return response.Hits.Select(x =>
                    new BookResponse
                    {
                        Id = x.Id,
                        Score = x.Score!.Value,
                        Book = x.Source!
                    })
                .ToArray();
        }
        
        // It must never be executed, because IndexAsync throw exception if something goes wrong
        throw new InvalidOperationException("Something was wrong.");
    }

    public async Task<BookResponse[]> Search(
        double? minimumRating,
        double? maximumRating,
        CancellationToken cancellationToken = default)
    {
        if (minimumRating == null && maximumRating == null)
        {
            throw new ArgumentException("One of params mustn't be null");
        }

        var rangeParameters = new RangeQueryParameter<double?>();
        
        if (minimumRating != null)
        {
            rangeParameters.Gt = minimumRating.Value;
        }
        if (maximumRating != null)
        {
            rangeParameters.Lt = maximumRating.Value;
        }

        var query = new QueryContainer
        {
            Query = new RangeQuery<double?>
            {
                FieldName = nameof(Book.AmazonRating),
                RangeParams = rangeParameters
            }
        };

        // TODO: See NEST
        var serializedQuery = JsonSerializer.Serialize(query, _serializerOptions);

        var result = await _client.Transport.RequestAsync<SearchResponse<Book>>(
            Elastic.Transport.HttpMethod.GET,
            $"{_client.ElasticsearchClientSettings.DefaultIndex}/_search",
            PostData.String(serializedQuery),
            cancellationToken: cancellationToken);

        if (result.ApiCallDetails.HasSuccessfulStatusCode)
        {
            return result.Hits
                .Select(x =>
                    new BookResponse
                    {
                        Id = x.Id,
                        Score = x.Score!.Value,
                        Book = x.Source!
                    })
                .ToArray();
        }
        
        throw new InvalidOperationException("Something was wrong.");
    }

    public async Task<BookResponse[]> Search(
        string author,
        double? minimumRating,
        string[]? tags,
        DateOnly? latestReleaseDate,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(author);

        var query = new BoolQuery
        {
            Must = new List<Query>
            {
                new MatchQuery("author")
                {
                    Query = author
                }
            }
        };
        
        if (minimumRating != null)
        {
            // TODO: Waiting for range_query implementation in library for Elasticsearch 8
            // https://github.com/elastic/elasticsearch-net/issues/7050
        }

        if (tags != null && tags.Length != 0)
        {
            var tagsQuery = new List<Query>();
            
            foreach (var tag in tags)
            {
                tagsQuery.Add(
                    new MatchQuery("tags")
                    {
                        Query = tag
                    });
            }

            query.Should = tagsQuery;
        }

        if (latestReleaseDate != null)
        {
            // TODO: Waiting for range_query implementation in library for Elasticsearch 8
            // https://github.com/elastic/elasticsearch-net/issues/7050
        }

        var result = await _client.SearchAsync<Book>(
            req => req.Query(query),
            cancellationToken);

        if (result.IsSuccess())
        {
            return result.Hits
                .Select(x =>
                    new BookResponse
                    {
                        Id = x.Id,
                        Score = x.Score!.Value,
                        Book = x.Source!
                    })
                .ToArray();
        }
        
        // It must never be executed, because IndexAsync throw exception if something goes wrong
        throw new InvalidOperationException("Something was wrong.");
    }

    
    public async Task<BookResponse[]> SearchInField(string field, string query, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(field);
        ArgumentException.ThrowIfNullOrEmpty(query);

        var response = await _client.SearchAsync<Book>(
            req =>
                req.Query(q => q.Match(
                    match =>
                    {
                        match.Field(field);
                        match.Query(query);
                        match.Operator(Operator.And);
                    })), 
            cancellationToken);
        
        if (response is not null && response.IsSuccess())
        {
            _logger.LogInformation(
                "Get all documents of {AuthorName}. Received {DocumentsCount} documents",
                query,
                response.Documents.Count);
            return response.Hits.Select(x =>
                    new BookResponse
                    {
                        Id = x.Id,
                        Score = x.Score!.Value,
                        Book = x.Source!
                    })
                .ToArray();
        }
        
        // It must never be executed, because IndexAsync throw exception if something goes wrong
        throw new InvalidOperationException("Something was wrong.");
    }

    public async Task<BookResponse[]> SearchInFieldByPrefix(string field, string queryPrefix, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(field);
        ArgumentException.ThrowIfNullOrEmpty(queryPrefix);

        SearchResponse<Book> response;

        if (queryPrefix.Split(' ').Length == 1)
        {
            // doesn't work with more than 1 word
            // works only with lowercase strings
            response = await _client.SearchAsync<Book>(
                req =>
                    req.Query(q => q.Prefix(
                        prefix =>
                        {
                            prefix.Field(field);
                            prefix.Value(queryPrefix.ToLower());
                        })), 
                cancellationToken);
        }
        else
        {
            response = await _client.SearchAsync<Book>(
                req =>
                    req.Query(q => q.MatchPhrasePrefix(
                        prefix =>
                        {
                            prefix.Field(field);
                            prefix.Query(queryPrefix);
                        })), 
                cancellationToken);
        }
        
        if (response.IsSuccess())
        {
            _logger.LogInformation(
                "Get all documents of {AuthorPrefix}. Received {DocumentsCount} documents",
                queryPrefix,
                response.Documents.Count);
            return response.Hits.Select(x =>
                    new BookResponse
                    {
                        Id = x.Id,
                        Score = x.Score!.Value,
                        Book = x.Source!
                    })
                .ToArray();
        }

        // It must never be executed, because IndexAsync throw exception if something goes wrong
        throw new InvalidOperationException("Something was wrong.");
    }

    public async Task<BookResponse[]> SearchInFields(string[] fieldsList, string query, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(query);

        var response = await _client.SearchAsync<Book>(
            req =>
                req.Query(q => q.MultiMatch(
                    match =>
                    {
                        match.Fields(fieldsList);
                        match.Query(query);
                        match.Operator(Operator.And);
                    })), 
            cancellationToken);
        
        if (response is not null && response.IsSuccess())
        {
            _logger.LogInformation(
                "Get all documents of {AuthorName}. Received {DocumentsCount} documents",
                query,
                response.Documents.Count);
            return response.Hits.Select(x =>
                    new BookResponse
                    {
                        Id = x.Id,
                        Score = x.Score!.Value,
                        Book = x.Source!
                    })
                .ToArray();
        }
        
        // It must never be executed, because IndexAsync throw exception if something goes wrong
        throw new InvalidOperationException("Something was wrong.");
    }

    public async Task<string> Save(Book book, CancellationToken cancellationToken = default)
    {
        var response = await _client.IndexAsync(
            book, 
            cancellationToken);

        if (response.IsSuccess())
        {
            _logger.LogInformation("Book was saved successful. ID: {ResponseId}", response.Id);
            return response.Id;
        }
        
        // It must never be executed, because IndexAsync throw exception if something goes wrong
        throw new InvalidOperationException("Something was wrong.");
    }

    public async Task<string[]> BulkSave(Book[] books, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(books);
        if (books.Length == 0)
        {
            return Array.Empty<string>();
        }
        
        var bulkAsync = await _client.BulkAsync(
            req =>
            {
                req.CreateMany(books);
            },
            cancellationToken);
        
        if (bulkAsync.IsSuccess())
        {
            return bulkAsync.Items.Select(x => x.Id).ToArray()!;
        }

        // It must never be executed, because IndexAsync throw exception if something goes wrong
        throw new InvalidOperationException("Something was wrong.");
        
        // var bulk = _client.BulkAll(
        //     books,
        //     conf =>
        //     {
        //         conf.Size(5);
        //         conf.MaxDegreeOfParallelism(10);
        //     },
        //     cancellationToken);
        
        // var ids = new List<string>();
        
        // bulk.Wait(TimeSpan.FromMinutes(30), response =>
        // {
        //     ids.AddRange(response.Items.Select(x => x.Id)!);
        // });

        // return ids.ToArray();
    }
}