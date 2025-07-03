using HtmlAgilityPack;
using System.Globalization;
using System.Net.Http;
using System.Net.Security;
using TelegramAutoPoster.Scrapers;

namespace BmwNewsBot.Scraper
{
	public class DidYouKnowsScraper : BaseScraper
	{
		public DidYouKnowsScraper(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
		{
		}

		public async Task<IEnumerable<string>> ScrapeAsync(CancellationToken cancellationToken)
		{
			var baseUrl = "https://www.did-you-knows.com/";

			var allFacts = new List<string>();
			string? nextPageUrl = baseUrl;

			while (!string.IsNullOrEmpty(nextPageUrl))
			{
				var html = await _httpClient.GetStringAsync(nextPageUrl);
				var doc = new HtmlDocument();
				doc.LoadHtml(html);

				// Extract facts from current page
				var facts = doc.DocumentNode
					.SelectNodes("//ul[@class='dykList']/li")
					?.Select(li =>
					{
						var dyk = li.SelectSingleNode(".//span[@class='dyk']")?.InnerText.Trim();
						var dykText = li.SelectSingleNode(".//span[@class='dykText']")?.InnerText.Trim();
						return $"{dyk} {dykText}";
					})
					.Where(f => f != null)
					.ToList();

				if (facts != null)
					allFacts.AddRange(facts);

				// Check for the next button
				var nextLink = doc.DocumentNode.SelectSingleNode("//div[@class='pagePagintionLinks']//a[contains(@class, 'next')]");
				if (nextLink != null)
				{
					var href = nextLink.GetAttributeValue("href", null);
					nextPageUrl = new Uri(new Uri(baseUrl), href).ToString();
				}
				else
				{
					nextPageUrl = null; // no more pages
				}
			}

			return allFacts;
		}
	}
}
