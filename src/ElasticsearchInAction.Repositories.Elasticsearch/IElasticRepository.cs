using ElasticsearchInAction.Repositories.Elasticsearch.Models;

namespace ElasticsearchInAction.Api.Repositories;

public interface IElasticRepository
{
    Task<long> GetCount(CancellationToken cancellationToken = default);
    Task<BookResponse?> Get(string id, CancellationToken cancellationToken = default);
    Task<BookResponse[]> Get(string[] ids, CancellationToken cancellationToken = default);
    Task<BookResponse[]> GetAll(CancellationToken cancellationToken = default);
    Task<BookResponse[]> Search(string[] fieldsList, string query, CancellationToken cancellationToken = default);

    Task<BookResponse[]> Search(
        string author,
        double? minimumRating,
        string[]? tags,
        DateOnly? latestReleaseDate,
        CancellationToken cancellationToken = default);
    Task<BookResponse[]> SearchInField(string field, string query, CancellationToken cancellationToken = default);
    Task<BookResponse[]> SearchInFieldByPrefix(string field, string queryPrefix, CancellationToken cancellationToken = default);
    Task<string> Save(Book book, CancellationToken cancellationToken = default);
    Task<string[]> BulkSave(Book[] books, CancellationToken cancellationToken = default);
}