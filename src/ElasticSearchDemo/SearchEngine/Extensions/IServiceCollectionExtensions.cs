using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using ESD.SearchEngine.Configurations;
using ESD.SearchEngine.Services.Business;
using ESD.SearchEngine.Services.Constracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace ESD.SearchEngine.Extensions;

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
