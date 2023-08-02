namespace TestMaker.Helpers.Interfaces
{
	public interface ISafeJsonSerializer
	{
		public string Serialize<T>(T obj);

		public T? Deserialize<T>(string json);
	}
}
