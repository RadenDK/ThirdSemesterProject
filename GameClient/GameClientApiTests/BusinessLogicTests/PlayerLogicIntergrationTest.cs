using GameClientApi.DatabaseAccessors;
using GameClientApi.BusinessLogic;
using GameClientApiTests.TestHelpers;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameClientApi.Models;
using Microsoft.Data.SqlClient;
using Dapper;
using Azure.Identity;

using BC = BCrypt.Net.BCrypt;


namespace GameClientApiTests.BusinessLogicTests
{
	[Collection("Sequential")]

	public class PlayerLogicIntergrationTest : IDisposable
    {
        private IConfiguration _configuration;

        private string _connectionString;

        private TestDatabaseHelper _testDatabaseHelper;

        public PlayerLogicIntergrationTest()
        {
            _configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettingsForTesting.json", optional: false, reloadOnChange: true)
        .Build();

            _connectionString = _configuration.GetConnectionString("DefaultConnection");

            _testDatabaseHelper = new TestDatabaseHelper(_connectionString);

            _testDatabaseHelper.TearDownAndBuildTestDatabase();
        }

        public void Dispose()
        {
            _testDatabaseHelper.TearDownAndBuildTestDatabase();
        }


        [Fact]
        public void CreatePlayer_TC5_MethodHashesAndStoresPasswordInDatabse()
        {
            // Arrange
            IPlayerDatabaseAccessor playerTestDatabaseAccessor = new PlayerDatabaseAccessor(_configuration);
            PlayerLogic SUT = new PlayerLogic(_configuration, playerTestDatabaseAccessor);

            AccountRegistrationModel mockPlayer = new AccountRegistrationModel
            {
                Username = "username1",
                Password = "password1",
                Email = "email1@example.com",
                InGameName = "InGameName1",
                BirthDay = DateTime.Now 
            };

            // Act
            bool testResult = SUT.CreatePlayer(mockPlayer);

            // Assert
            Assert.True(testResult, "Expected CreatePlayer to return true, but it returned false.");
            string selectQuery = "SELECT PasswordHash FROM Player WHERE Username = @Username";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var result = connection.Query<string>(selectQuery, new { Username = mockPlayer.Username }).SingleOrDefault();
                Assert.True(result != null, "Expected to retrieve a password hash from the database, but the result was null.");
                Assert.True(result != mockPlayer.Password, "Expected the stored password hash to be different from the plain text password, but they were the same.");
                Assert.True(BC.Verify(mockPlayer.Password, result), "The stored password hash does not match the hash of the plain text password.");
            }
        }

        
    }
}
