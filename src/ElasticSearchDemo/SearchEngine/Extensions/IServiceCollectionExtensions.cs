using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SearchEngine.Configurations;
using SearchEngine.Services.Business;
using SearchEngine.Services.Constracts;
using System.Net;

namespace SearchEngine.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddElasticsearch(this IServiceCollection services, IConfiguration configuration)
    {
        var config = configuration.GetSection("ElasticsearchConfiguration").Get<ElasticsearchConfiguration>()
            ?? throw new ArgumentNullException(nameof(ElasticsearchConfiguration));

        var settings = new ElasticsearchClientSettings(new Uri(config.Url))
            .Authentication(new BasicAuthentication(config.Username, config.Password));

        services.AddSingleton(new ElasticsearchClient(settings));
        ServicePointManager.ServerCertificateValidationCallback +=
            (sender, certificate, chain, errors) =>
            {
                return true;
            };

        services.AddSingleton<IElasticsearchService, ElasticsearchService>();

        return services;
    }
}
