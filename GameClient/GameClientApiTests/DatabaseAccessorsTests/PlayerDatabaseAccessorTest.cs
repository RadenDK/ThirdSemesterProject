using GameClientApiTests.TestHelpers;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClientApiTests.DatabaseAccessorsTests
{
	public class PlayerDatabaseAccessorTest
	{
		private IConfiguration _configuration;
		private string _connectionString;

		private TestDatabaseHelper _testDatabaseHelper;

		public PlayerDatabaseAccessorTest(IConfiguration configuration)
		{
			_configuration = configuration;
			_connectionString = _configuration.GetConnectionString("TestDatabase");
			_testDatabaseHelper = new TestDatabaseHelper(configuration);
		}


	}
}
