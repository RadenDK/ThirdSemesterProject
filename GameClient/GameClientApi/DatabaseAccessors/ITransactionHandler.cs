using Microsoft.Data.SqlClient;
using System.Data;

namespace GameClientApi.DatabaseAccessors
{
    public interface ITransactionHandler
    {

        public SqlTransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        public void CommitTransaction(SqlTransaction sqlTransaction);

        public void RollbackTransaction(SqlTransaction sqlTransaction);


    }
}
