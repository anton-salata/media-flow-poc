namespace MediaFlow.Storage
{
	public class DataStore
	{
		private readonly string _dataFolderPath;
		private readonly string _didYouKnowsFileName = "DidYouKnows.txt";
		private readonly string _imagesFolderPath = "Images";

		public DataStore()
		{
			var exePath = AppContext.BaseDirectory;
			_dataFolderPath = Path.Combine(exePath, "Data");
			_imagesFolderPath = Path.Combine(_dataFolderPath, "Images");
		}

		public async Task<List<string>> GetDidYouKnowsAsync()
		{
			var filePath = Path.Combine(_dataFolderPath, _didYouKnowsFileName);

			if (!File.Exists(filePath))
			{
				throw new FileNotFoundException("DidYouKnows.txt not found in data folder.", filePath);
			}

			var lines = await File.ReadAllLinesAsync(filePath);
			return new List<string>(lines);
		}

		public async Task SaveBase64ImageToFileAsync(string base64Image)
		{
			if (!Directory.Exists(_imagesFolderPath))
			{
				Directory.CreateDirectory(_imagesFolderPath);
			}

			string fileName = Path.Combine(_imagesFolderPath, $"{Guid.NewGuid()}.png");

			byte[] imageBytes = Convert.FromBase64String(base64Image);

			await File.WriteAllBytesAsync(fileName, imageBytes);
		}
	}
}
