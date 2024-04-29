using Microsoft.Data.SqlClient;
using System.Data;

namespace GameClientApi.DatabaseAccessors
{
    public class TransactionHandler : ITransactionHandler
    {

        private readonly string _connectionString;

        public TransactionHandler(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
       
        
        
        
        
        public SqlTransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction(isolationLevel);
            return transaction;
        }

        public void CommitTransaction(SqlTransaction sqlTransaction)
        {
            try
            {
                sqlTransaction.Commit();
            }
            finally
            {
                try
                {
                    sqlTransaction.Connection.Close();
                }
                catch (NullReferenceException ex)
                {

                }
            }
        }

        public void RollbackTransaction(SqlTransaction sqlTransaction)
        {
            try
            {
                sqlTransaction.Rollback();
            }
            finally
            {
                if (sqlTransaction.Connection != null)
                {
                    sqlTransaction.Connection.Close();
                }
            }
        }

    }
}
