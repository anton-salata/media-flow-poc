namespace TelegramAutoPoster.Scrapers
{
	public abstract class BaseScraper
	{
		protected readonly HttpClient _httpClient;

		public BaseScraper(IHttpClientFactory httpClientFactory)
		{
            //_httpClient = httpClientFactory.CreateClient("WithProxy"); //.CreateClient(); //.CreateClient("WithProxy");
            _httpClient = httpClientFactory.CreateClient(); //.CreateClient(); //.CreateClient("WithProxy");
        }
	}
}
