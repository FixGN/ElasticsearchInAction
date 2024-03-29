﻿using Elastic.Clients.Elasticsearch;
using Elastic.Transport;

namespace ElasticsearchInAction.Repositories.Elasticsearch;

public static class ElasticsearchClientFactory
{
    public static ElasticsearchClient Create(Uri host, string user, string password, string indexName)
    {
        var elasticSettings = new ElasticsearchClientSettings(host);
        elasticSettings.Authentication(new BasicAuthentication(user, password));
        // Due to ssl cert was generated by ourself
        elasticSettings.ServerCertificateValidationCallback(CertificateValidations.AllowAll);
        elasticSettings.DefaultIndex(indexName);
        return new ElasticsearchClient(elasticSettings);
    }
}