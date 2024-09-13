using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Core.Bulk;
using SearchEngine.Services.Constracts;

namespace SearchEngine.Services.Business;

public class ElasticsearchService : IElasticsearchService
{
    private readonly ElasticsearchClient _client;

    private const string _allFields = "_all";

    private const int BulkSize = 1000; // Maximum number of documents per bulk request


    public ElasticsearchService(ElasticsearchClient client)
    {
        _client = client;
    }

    public async Task IndexDocumentAsync<T>(T document, string indexName) where T : class
    {
        var response = await _client.IndexAsync(document, idx => idx.Index(indexName));
        if (!response.IsValidResponse)
        {
            throw new Exception("Failed to index document");
        }
    }

    public async Task IndexDocumentsAsync<T>(IEnumerable<T> documents, string indexName) where T : class
    {
        var documentList = documents.ToList(); // Convert to list to allow indexing
        var totalDocuments = documentList.Count;

        for (int i = 0; i < totalDocuments; i += BulkSize)
        {
            var chunk = documentList.Skip(i).Take(BulkSize).ToList(); // Get the chunk of documents

            var bulkRequest = new BulkRequest(indexName)
            {
                Operations = new()
            };

            foreach (var document in chunk)
            {
                bulkRequest.Operations.Add(new BulkIndexOperation<T>(document));
            }

            var response = await _client.BulkAsync(bulkRequest);

            if (!response.IsValidResponse)
            {
                throw new Exception($"Failed to index documents in chunk starting at index {i}");
            }

            Console.WriteLine($"Indexed {chunk.Count} documents successfully (chunk starting at index {i}).");
        }

    }

    public async Task<SearchResponse<T>> SearchDocumentsAsync<T>(string indexName, string query) where T : class
    {
        var response = await _client.SearchAsync<T>(s => s
            .Index(indexName)
            .Query(q => q
                .QueryString(qs => qs
                    .Query(query)
                )
            )
        );

        return response;
    }
}
