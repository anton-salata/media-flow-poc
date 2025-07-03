
namespace MediaFlow.Clients
{
	public interface IAiImageClient
	{
		Task<string> GenerateImageAsync(string prompt);
	}
}