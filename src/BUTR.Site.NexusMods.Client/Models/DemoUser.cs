﻿using BUTR.Site.NexusMods.Client.Services;
using BUTR.Site.NexusMods.Shared.Helpers;
using BUTR.Site.NexusMods.Shared.Models;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BUTR.Site.NexusMods.Client.Models
{
    public static class DemoUser
    {
        private static readonly ProfileModel _profile = new(31179975, "Pickysaurus", "demo@demo.com", "https://forums.nexusmods.com/uploads/profile/photo-31179975.png", true, true, ApplicationRoles.User);
        private static readonly List<ModModel> _mods = new()
        {
            new("Demo Mod 1", "demo", 1),
            new("Demo Mod 2", "demo", 2),
            new("Demo Mod 3", "demo", 3),
            new("Demo Mod 4", "demo", 4),
        };
        private static List<CrashReportModel>? _crashReports;

        public static Task<ProfileModel> GetProfile() => Task.FromResult(_profile);
        public static IAsyncEnumerable<ModModel> GetMods() => _mods.ToAsyncEnumerable();
        public static async IAsyncEnumerable<CrashReportModel> GetCrashReports(IHttpClientFactory factory, BrotliDecompressorService brotliDecompressor)
        {
            static async Task<(string Id, string Content)> DownloadReport(HttpClient client, BrotliDecompressorService brotliDecompressor, string id)
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"{id}.html.br");
                var response = await client.SendAsync(request);
                var compressed = await response.Content.ReadAsStreamAsync();
                var decompressed = await brotliDecompressor.DecompileAsync(compressed);
                var data = new StreamReader(decompressed);
                return (id, await data.ReadToEndAsync());
            }

            if (_crashReports is null)
            {
                var crm = new List<CrashReportModel>();
                const string baseUrl = "https://crash.butr.dev/report/";
                var client = factory.CreateClient("InternalReports");
                var reports = new[] { "FC58E239", "7AA28856", "4EFF0B0A", "3DF57593" };
                var contents = await Task.WhenAll(reports.Select(r => DownloadReport(client, brotliDecompressor, r)));
                foreach (var (id, content) in contents)
                {
                    var cr = CrashReportParser.Parse(id, content);
                    var report = new CrashReportModel(cr.Id, cr.Exception, DateTime.UtcNow, $"{baseUrl}{cr.Id2}.html");
                    crm.Add(report);
                    yield return report;

                }
                _crashReports = crm;
            }
            else
            {
                foreach (var crashReport in _crashReports)
                {
                    yield return crashReport;
                }
            }
        }
    }
}