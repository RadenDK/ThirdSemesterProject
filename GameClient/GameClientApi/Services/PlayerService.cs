﻿using GameClientApi.DatabaseAccessors;
using GameClientApi.Models;
using BC = BCrypt.Net.BCrypt;
namespace GameClientApi.Services
{
    public class PlayerService
    {
        IPlayerDatabaseAccessor _playerAccessor;

        public PlayerService(IConfiguration configuration, IPlayerDatabaseAccessor playerDatabaseAccessor)
        {
            _playerAccessor = playerDatabaseAccessor;
        }

        public bool VerifyLogin(string userName, string password)
        {
            string storedHashedPassword = _playerAccessor.GetPassword(userName);
            if (storedHashedPassword == null || userName == null)
            {
                throw new ArgumentNullException("storedHashedPassword or userName is null");
            }
            return BC.Verify(password, storedHashedPassword);
        }

        public Player GetPlayer(string userName)
        {
            Player playerData = _playerAccessor.GetPlayer(userName);
            if (playerData == null)
            {
                throw new Exception("Player not found");
            }
            return playerData;
        }


        public bool CreatePlayer(AccountRegistrationModel newPlayerAccount)
        {
            if (_playerAccessor.UsernameExists(newPlayerAccount.Username))
            {
                throw new ArgumentException("Username already exists");
            }
            if (_playerAccessor.InGameNameExists(newPlayerAccount.InGameName))
            {
                throw new ArgumentException("InGameName already exists");
            }

            AccountRegistrationModel newPlayerAccountWithHashedPassword = new AccountRegistrationModel
            {
                Username = newPlayerAccount.Username,
                Password = BC.HashPassword(newPlayerAccount.Password),
                Email = newPlayerAccount.Email,
                InGameName = newPlayerAccount.InGameName,
                BirthDay = newPlayerAccount.BirthDay
            };

            return _playerAccessor.CreatePlayer(newPlayerAccountWithHashedPassword);
        }
    }
}
