using BmwNewsBot.Scraper;
using MediaFlow.Clients;
using MediaFlow.Entities;
using MediaFlow.Storage;
using MediaFlow.Storage.Interfaces;
using MediaFlow.Utilities;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MediaFlow.Services
{
    public class MediaFlowService : BackgroundService
    {
        private readonly DidYouKnowsScraper _scraper;
        private readonly IRepository<DidYouKnowItem> _repository;
        //private readonly IProcessedItemStore _processedItemStore;
        private readonly ILogger<MediaFlowService> _logger;

        public MediaFlowService(
            DidYouKnowsScraper scraper,
            //IProcessedItemStore processedItemStore,
            IRepository<DidYouKnowItem> repository,
            ILogger<MediaFlowService> logger)
        {
            _scraper = scraper;
            _repository = repository;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("MediaFlowService started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await Run(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred during MediaFlowService.Run()");
                }

                _logger.LogInformation("Waiting 1 hour until next run...");
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
                _logger.LogInformation("Woke up. Starting next scraping cycle...");
            }
        }

        public async Task Run(CancellationToken cancellationToken)
        {
            //var ng = new NarrationGeneratorV2();

            //ng.GenerateNarration(
            //	"C:\\Projects\\MediaFlow\\MediaFlow\\bin\\Debug\\net8.0\\Data\\Temp\\1.txt",
            //	"C:\\Projects\\MediaFlow\\MediaFlow\\bin\\Debug\\net8.0\\Data\\Temp\\888.wav"
            //	);


            string outputMusicFile = @"C:\Projects\MediaFlow\MediaFlow\bin\Debug\net8.0\Data\Temp\music_synth.wav";
            //var ag = new MusicGenerator();
            //ag.GenerateMusic(outputMusicFile, "Retro 80s synthwave track with nostalgic analog synths, pulsing basslines", 18);

            //ag.GenerateMusic(outputMusicFile, "Emotional dreamwave track with lush ambient synths, warm analog textures", 15);

            //ag.GenerateMusic(outputMusicFile, "Lo-fi hip hop beat with chill melodies and a head-nodding groove", 15);

            string image = @"C:\Projects\MediaFlow\MediaFlow\bin\Debug\net8.0\Data\Temp\1.png";
            string srt = @"C:\Projects\MediaFlow\MediaFlow\bin\Debug\net8.0\Data\Temp\subtitles.srt";
            string audio = @"C:\Projects\MediaFlow\MediaFlow\bin\Debug\net8.0\Data\Temp\output_p313.wav";
            string output = @"C:\Projects\MediaFlow\MediaFlow\bin\Debug\net8.0\Data\Temp\final_video3338.mp4";

            var vg = new SubtitleVideoGenerator();
            vg.GenerateVideo(image, srt, audio, outputMusicFile, output);

            //WhisperWrapper.GenerateSubtitles("C:\\Projects\\MediaFlow\\MediaFlow\\bin\\Debug\\net8.0\\Data\\Temp\\output_p313.wav", "C:\\Projects\\MediaFlow\\MediaFlow\\bin\\Debug\\net8.0\\Data\\Temp\\subtitles.srt");


            //SubtitleGenerator.GenerateSubtitles(
            //										"C:\\Projects\\MediaFlow\\MediaFlow\\bin\\Debug\\net8.0\\Data\\Temp\\1.wav",
            //										"C:\\Projects\\MediaFlow\\MediaFlow\\bin\\Debug\\net8.0\\Data\\Temp\\1.txt",
            //										"C:\\Projects\\MediaFlow\\MediaFlow\\bin\\Debug\\net8.0\\Data\\Temp\\subtitles.srt"
            //									);

            //var items = await _repository.GetAllAsync();

            //var testItem = items.FirstOrDefault(i => !string.IsNullOrEmpty(i.Summary) && i.Image != null);

            //var videoGenerator = new VideoGeneratorService();

            //var videoPath = videoGenerator.GenerateVideo(testItem.Quote.Substring(13), testItem.Summary, testItem.Image);


            //var fbClient = new FusionBrainClient();
            //var itemsStore = new ProcessedItemsStore(null);
            //var dataStore = new DataStore();

            //foreach (var item in await itemsStore.LoadAll())
            //{
            //	var imageBase64 = await fbClient.GenerateImageAsync(item.Quote.Substring(13));

            //	await itemsStore.UpdateImage(item.Id, imageBase64);

            //	await dataStore.SaveBase64ImageToFileAsync(imageBase64);
            //}

            //_logger.LogInformation("Starting DidYouKnow processing...");

            //var dataStore = new DataStore();
            //var aiChatClient = new OpenRouterClient();
            //var processedItemsStore = new ProcessedItemsStore(null);

            //var didYouKnows = (await dataStore.GetDidYouKnowsAsync());

            //_logger.LogInformation($"Loaded {didYouKnows.Count} did-you-know items.");


            //foreach (var quote in didYouKnows)
            //{
            //	if (await processedItemsStore.IsQuoteProcessedAsync(quote))
            //	{
            //		_logger.LogInformation("Skipping already processed quote: {Quote}", quote);

            //		continue;
            //	}

            //	_logger.LogInformation("Processing quote: {Quote}", quote);

            //	var prompt = $"You are a fun, educational assistant. Please provide a short, engaging summary or explanation of the following fact for general audiences:\r\n\"{quote}\"\r\nThe explanation should be 2–3 sentences, easy to understand, and add interesting context or significance to the fact.";

            //	try
            //	{
            //		var response = await aiChatClient.GetAnswer(prompt);
            //		_logger.LogInformation($"Received response for quote:\n{response}");

            //		await processedItemsStore.SaveQuoteAsync(quote, response);
            //		_logger.LogInformation("Saved processed quote.");
            //	}
            //	catch (Exception ex)
            //	{
            //		_logger.LogError(ex, "Error processing quote: {Quote}", quote);
            //	}


            //	await DelayUtilities.DelayRandomAsync();
            //}

            //_logger.LogInformation("Finished processing all quotes.");



            //Console.WriteLine(resp);
            //var allFacts = await _scraper.ScrapeAsync(cancellationToken);

            //foreach (var fact in allFacts)
            //{
            //	Console.WriteLine(fact);
            //}
        }
    }
}
