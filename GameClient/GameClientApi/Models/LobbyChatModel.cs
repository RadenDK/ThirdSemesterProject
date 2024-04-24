namespace GameClientApi.Models
{
	public class LobbyChatModel
	{
		public int LobbyChatId { get; set; }
		public string ChatType { get; set; }

		public IEnumerable<ChatMessageModel> ChatMessages { get; set;}
	}
}
