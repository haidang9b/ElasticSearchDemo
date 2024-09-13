using Elastic.Clients.Elasticsearch;

namespace SearchEngine.Services.Constracts;

public interface IElasticsearchService
{
    Task IndexDocumentAsync<T>(T document, string indexName) where T : class;

    Task IndexDocumentsAsync<T>(IEnumerable<T> documents, string indexName) where T : class;

    Task<SearchResponse<T>> SearchDocumentsAsync<T>(string indexName, string query) where T : class;
}
