using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace CURSETools.Mod
{
    public static class Parser
    {
        public static Mod ParseModPage(string html)
        {
            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.OptionFixNestedTags = true;
            htmlDoc.LoadHtml(html);

            if (htmlDoc.ParseErrors != null && htmlDoc.ParseErrors.Any())
            {
                Console.WriteLine("Parse error: "+htmlDoc.ParseErrors);
                return null;
            }

            if (htmlDoc.DocumentNode == null) return null;
            var mod = new Mod();
            var bodyNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='main-info']");
            var authorsSection = bodyNode.SelectSingleNode("//ul[@class='authors group']").ChildNodes.Where(m=>m.Name=="li");
            mod.project_manager = authorsSection.First().ChildNodes.Where(m => m.Name == "a").Select(m => m.InnerText).ToArray();
            mod.contributor =     authorsSection.Last().ChildNodes.Where(m => m.Name == "a").Select(m => m.InnerText).ToArray();
            var detailsSection = bodyNode.SelectSingleNode("//ul[@class='details-list']");
            if (detailsSection == null) return null;
            var gameNode = detailsSection.SelectSingleNode("//li[@class='game']");
            var gameLink = gameNode.ChildNodes.FirstOrDefault(m=>m.Name=="a");
            mod.game_id = gameLink.Attributes["href"].Value.Split('/').Last();
            mod.game_name = gameLink.InnerText;
            var averageDownloads = detailsSection.SelectSingleNode("//li[@class='average-downloads']");
            mod.average_downloads = int.Parse(averageDownloads.InnerText.Split(' ')[0].Replace(",", ""));
            var detailsList = detailsSection.ChildNodes.Where(m=>m.Name=="li");
            var supports = detailsList
                    .FirstOrDefault(m =>
                    {
                        var attrib = m.Attributes["class"];
                        return attrib != null && attrib.Value.Split(' ').Contains("version");
                    });
            mod.supports = double.Parse(supports.InnerText.Split(' ')[1]);
            var downloads = detailsSection.SelectSingleNode("//li[@class='average-downloads']");
            mod.downloads = int.Parse(downloads.InnerText.Split(' ')[0].Replace(",", ""));
            var updated = detailsSection.SelectNodes("//li[@class='updated']");
            foreach (var upd in updated)
            {
                if (upd.InnerText.StartsWith("Updated"))
                {
                    mod.updated = Utils.FromUnixTime(long.Parse(upd.ChildNodes[1].Attributes["data-epoch"].Value));
                }
                else
                {
                    mod.created = Utils.FromUnixTime(long.Parse(upd.ChildNodes[1].Attributes["data-epoch"].Value));
                }
            }
            var favorited = detailsSection.SelectSingleNode("//li[@class='favorited']");
            mod.favorited = int.Parse(favorited.InnerText.Split(' ')[0]);
            var projecturl = detailsList.FirstOrDefault(m=>m.Attributes["class"].Value == "curseforge");
            mod.project_url = projecturl.FirstChild.Attributes["href"].Value;
            var release = detailsSection.SelectSingleNode("//li[@class='release']");
            mod.release_type = release.InnerText.Substring(release.InnerText.IndexOf(": ")+2);
            var license = detailsSection.SelectSingleNode("//li[@class='license']");
            mod.license = license.InnerText.Substring(license.InnerText.IndexOf(": ") + 2);
            var file = detailsSection.SelectSingleNode("//li[@class='newest-file']");
            mod.newest_file = file.InnerText.Substring(file.InnerText.IndexOf(": ") + 2);
            return mod;
        }

        public static Mod FromUrl(string url)
        {
            using (var myWebClient = new WebClient())
            {
                return ParseModPage(myWebClient.DownloadString(url));
            }
        }
    }
}
