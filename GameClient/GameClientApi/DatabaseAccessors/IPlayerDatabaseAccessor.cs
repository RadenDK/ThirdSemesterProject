﻿using GameClientApi.Models;

namespace GameClientApi.DatabaseAccessors
{
	public interface IPlayerDatabaseAccessor
	{
		bool VerifyLogin(string userName, string password);

		string GetPassword(string userName);

		bool CreatePlayer(AccountRegistrationModel newPlayer);

		bool UsernameExists(string username);

		bool InGameNameExists(string ingamename);


	}
}
