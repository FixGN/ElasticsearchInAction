using ElasticsearchInAction.Api.Models;
using ElasticsearchInAction.Api.Repositories;
using ElasticsearchInAction.Api.Responses;
using Dto = ElasticsearchInAction.Repositories.Elasticsearch.Models;
using Microsoft.AspNetCore.Mvc;

namespace ElasticsearchInAction.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class BooksController : ControllerBase
{
    [HttpGet("count")]
    public async Task<dynamic> GetCountOfDocuments(
        [FromServices] IElasticRepository repository,
        CancellationToken cancellationToken)
    {
        var response = await repository.GetCount(cancellationToken);

        return new
        {
            Count = response
        };
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(
        [FromServices] IElasticRepository repository,
        string id)
    {
        var response = await repository.Get(id);

        if (response == null)
        {
            return NotFound();
        }

        return new OkObjectResult(new Book
        {
            Id = response.Id,
            Score = response.Score,
            Data = BookData.FromDto(response.Book)
        });
    }
    
    [HttpGet]
    public async Task<GetAllResponse> GetAll(
        [FromServices] IElasticRepository repository,
        [FromQuery] string[]? ids,
        CancellationToken cancellationToken)
    {
        if (ids is not null && ids is not { Length: 0 })
        {
            return await GetAllByIdsInternal(repository, ids, cancellationToken);
        }

        return await GetAllInternal(repository, cancellationToken);
    }

    /// <summary>
    /// Add one book to repository
    /// </summary>
    [HttpPost]
    public async Task<CreateResponse> Create(
        [FromServices] IElasticRepository repository, 
        [FromBody] BookData bookData,
        CancellationToken cancellationToken)
    {
        var dto = new Dto.Book
        {
            Title = bookData.Title,
            Author = bookData.Author,
            Edition = bookData.Edition,
            Synopsys = bookData.Synopsys,
            ReleaseDate = DateOnly.FromDateTime(bookData.ReleaseDate),
            AmazonRating = bookData.AmazonRating,
            Tags = bookData.Tags,
            BestSeller = bookData.BestSeller,
            Prices = new Dictionary<string, double>(
                bookData.Prices.Select(x => 
                    new KeyValuePair<string, double>(x.Key.ToString("G"), x.Value)))
        };
        
        var documentId = await repository.Save(dto, cancellationToken);

        return new CreateResponse
        {
            IsSuccess = true,
            DocumentId = documentId
        };
    }

    [HttpGet("search")]
    public async Task<GetAllResponse> SearchAcrossAllText(
        [FromServices] IElasticRepository repository,
        [FromQuery] string query,
        CancellationToken cancellationToken)
    {
        var response = await repository.SearchInFields(
            new[] { "title^2", "author^3", "synopsis" },
            query,
            cancellationToken);

        return GetAllResponse.SuccessResponse(
            response
                .Select(x => 
                    new Book
                    {
                        Id = x.Id,
                        Score = x.Score,
                        Data = BookData.FromDto(x.Book)
                    })
                .ToArray());
    }
    
    [HttpGet("search/author")]
    public async Task<GetAllResponse> SearchByAuthor(
        [FromServices] IElasticRepository repository,
        [FromQuery] string query,
        [FromQuery] bool? byPrefix,
        CancellationToken cancellationToken)
    {
        Dto.BookResponse[] response;
        
        if (byPrefix is true)
        {
            response = await repository.SearchInFieldByPrefix("author", query, cancellationToken);
        }
        else
        {
            response = await repository.SearchInField("author", query, cancellationToken);
        }


        return GetAllResponse.SuccessResponse(
            response
                .Select(x => 
                    new Book
                    {
                        Id = x.Id,
                        Score = x.Score,
                        Data = BookData.FromDto(x.Book)
                    })
                .ToArray());
    }

    [HttpGet("search/author/best")]
    public async Task<GetAllResponse> SearchByAuthor(
        [FromServices] IElasticRepository repository,
        [FromQuery] string? author,
        [FromQuery] double? rating,
        [FromQuery] string[] tags,
        [FromQuery] DateTime oldestDate,
        CancellationToken cancellationToken)
    {
        if (author == null)
        {
            return new GetAllResponse
            {
                IsSuccess = true
            };
        }

        var response = await repository.Search(
            author,
            rating,
            tags,
            DateOnly.FromDateTime(oldestDate),
            cancellationToken);

        return GetAllResponse.SuccessResponse(
            response
                .Select(x => 
                    new Book
                    {
                        Id = x.Id,
                        Score = x.Score,
                        Data = BookData.FromDto(x.Book)
                    })
                .ToArray());
    }

    [HttpGet("search/amazonrating")]
    public async Task<IActionResult> SearchByAmazonRating(
        [FromServices] IElasticRepository repository,
        [FromQuery] double? minRating,
        [FromQuery] double? maxRating,
        CancellationToken cancellationToken)
    {
        if (minRating == null && maxRating == null)
        {
            return BadRequest(GetAllResponse.ErrorResponse("minRating or maxRating must be set!"));
        }

        var response = await repository.Search(
            minRating,
            maxRating,
            cancellationToken);

        return Ok(GetAllResponse.SuccessResponse(
            response
                .Select(x => 
                    new Book
                    {
                        Id = x.Id,
                        Score = x.Score,
                        Data = BookData.FromDto(x.Book)
                    })
                .ToArray()));
    }
    
    private async Task<GetAllResponse> GetAllInternal(IElasticRepository repository, CancellationToken cancellationToken)
    {
        var response = await repository.GetAll(cancellationToken);

        return GetAllResponse.SuccessResponse(
            response
                .Select(x => 
                    new Book
                    {
                        Id = x.Id,
                        Score = x.Score,
                        Data = BookData.FromDto(x.Book)
                    })
                .ToArray());
    }

    private async Task<GetAllResponse> GetAllByIdsInternal(
        [FromServices] IElasticRepository repository,
        [FromQuery] string[] ids,
        CancellationToken cancellationToken)
    {
        var response = await repository.Get(ids, cancellationToken);
        
        return GetAllResponse.SuccessResponse(
            response
                .Select(x => 
                    new Book
                    {
                        Id = x.Id,
                        Score = x.Score,
                        Data = BookData.FromDto(x.Book)
                    })
                .ToArray());
    }
}
