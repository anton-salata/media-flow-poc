namespace MediaFlow.Entities
{
	public class DidYouKnowItem
	{
		public int Id { get; set; }
		public string Quote { get; set; }
		public string Summary { get; set; }
		public byte[]? Image { get; set; }
	}
}
