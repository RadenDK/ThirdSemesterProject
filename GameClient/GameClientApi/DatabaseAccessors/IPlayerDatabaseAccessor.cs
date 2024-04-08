namespace GameClientApi.DatabaseAccessors
{
	public interface IPlayerDatabaseAccessor
	{
		bool VerifyLogin(string userName, string password);

		string GetPassword(string password);
	}
}
