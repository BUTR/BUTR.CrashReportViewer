﻿using HtmlAgilityPack;

using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BUTR.Site.NexusMods.Server.Services
{
    public sealed class NexusModsClient
    {
        private readonly HttpClient _httpClient;

        public NexusModsClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<HtmlDocument?> GetArticleAsync(string gameDomain, int articleId)
        {
            using var response = await _httpClient.GetAsync($"{gameDomain}/articles/{articleId}");

            var doc = new HtmlDocument();
            doc.Load(await response.Content.ReadAsStreamAsync());

            return doc;
        }
    }
}