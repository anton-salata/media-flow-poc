using MediaFlow.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;

namespace MediaFlow.Storage
{
	public class ProcessedItemsStore
	{
		private readonly string _dbPath;
		private readonly ILogger<ProcessedItemsStore> _logger;
		private readonly SemaphoreSlim _semaphore = new(1, 1);

		public ProcessedItemsStore(ILogger<ProcessedItemsStore> logger)
		{
			_logger = logger;

			_dbPath = Path.Combine(AppContext.BaseDirectory, "items.db");

			InitializeAsync().Wait();
		}

		public async Task InitializeAsync()
		{
			await _semaphore.WaitAsync();
			try
			{
				Directory.CreateDirectory(Path.GetDirectoryName(_dbPath)!);

				await using var connection = new SqliteConnection($"Data Source={_dbPath};");
				await connection.OpenAsync();

				var command = connection.CreateCommand();
				command.CommandText = @"
                CREATE TABLE IF NOT EXISTS DidYouKnow (
						Id INTEGER PRIMARY KEY AUTOINCREMENT,
						Quote TEXT NOT NULL,						
						Summary TEXT,
						Image BLOB
				);";
				await command.ExecuteNonQueryAsync();
			}
			finally
			{
				_semaphore.Release();
			}
		}

		public async Task<bool> IsQuoteProcessedAsync(string quote)
		{
			await _semaphore.WaitAsync();
			try
			{
				await using var connection = new SqliteConnection($"Data Source={_dbPath};");
				await connection.OpenAsync();

				var command = connection.CreateCommand();
				command.CommandText = "SELECT COUNT(*) FROM DidYouKnow WHERE Quote = $quote";
				command.Parameters.AddWithValue("$quote", quote);

				var count = (long?)await command.ExecuteScalarAsync();
				return count > 0;
			}
			finally
			{
				_semaphore.Release();
			}
		}

		public async Task SaveQuoteAsync(string quote, string summary)
		{
			await _semaphore.WaitAsync();
			try
			{
				await using var connection = new SqliteConnection($"Data Source={_dbPath};");
				await connection.OpenAsync();

				var command = connection.CreateCommand();
				command.CommandText = "INSERT INTO DidYouKnow (Quote, Summary) VALUES ($quote, $summary)";
				command.Parameters.AddWithValue("$quote", quote);
				command.Parameters.AddWithValue("$summary", summary);

				await command.ExecuteNonQueryAsync();
			}
			finally
			{
				_semaphore.Release();
			}
		}

		public async Task UpdateImage(int id, string base64Image)
		{
			byte[] imageBytes = Convert.FromBase64String(base64Image);

			await using var connection = new SqliteConnection($"Data Source={_dbPath};");
			await connection.OpenAsync();

			var command = connection.CreateCommand();

			command.CommandText = "UPDATE DidYouKnow SET Image = @image WHERE Id = @id";
			command.Parameters.AddWithValue("@image", imageBytes);
			command.Parameters.AddWithValue("@id", id);

			int rowsAffected = command.ExecuteNonQuery();

			Console.WriteLine(rowsAffected > 0
				? $"Image updated for row ID {id}."
				: $"No row found with ID {id}.");
		}

		public async Task<List<DidYouKnowItem>> LoadAll()
		{
			var items = new List<DidYouKnowItem>();

			using (var connection = new SqliteConnection($"Data Source={_dbPath};"))
			{
				connection.OpenAsync();
				var command = connection.CreateCommand();
				command.CommandText = "SELECT Id, Quote, Summary, Image FROM DidYouKnow";

				using (var reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						items.Add(new DidYouKnowItem
						{
							Id = reader.GetInt32(0),
							Quote = reader.GetString(1),
							Summary = reader.GetString(2),
							Image = reader.IsDBNull(3) ? null : (byte[])reader["Image"]
						});
					}
				}
			}

			return items;
		}
	}
}
