using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Tool
{
    public static class GitHubReleaseChecker
    {
        private const string GitHubApiUrl = "https://api.github.com/repos/docfo4r/ToolUpdater/releases/latest";

        public static async Task<(Version latestVersion, string downloadUrl)> GetLatestReleaseAsync()
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.UserAgent.ParseAdd("MyAppUpdater/1.0"); // GitHub requires User-Agent

            var response = await client.GetAsync(GitHubApiUrl);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);

            var versionStr = doc.RootElement.GetProperty("tag_name").GetString(); // like "v1.0.1"
            var asset = doc.RootElement.GetProperty("assets")[0];
            var downloadUrl = asset.GetProperty("browser_download_url").GetString();

            return (new Version(versionStr.TrimStart('v')), downloadUrl);
        }
    }
}