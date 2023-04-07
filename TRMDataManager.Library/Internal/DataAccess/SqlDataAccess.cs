using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace TRMDataManager.Library.Internal.DataAccess
{
    internal class SqlDataAccess : IDisposable
    {
	    private IDbConnection _sqlConnection;
	    private IDbTransaction _transaction;

		public string GetConnectionString(string name)
	    {
		    return ConfigurationManager.ConnectionStrings[name].ConnectionString;
	    }

	    public List<T> LoadData<T, T2>(string storedProcedure, T2 parameters, string connectionStringName)
	    {
		    string connectionString = GetConnectionString(connectionStringName);
		    using (IDbConnection connection = new SqlConnection(connectionString))
		    {
			    List<T> rows = connection
				    .Query<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure).ToList();

			    return rows;
		    }
	    }

	    public void SaveData<T>(string storedProcedure, T parameters, string connectionStringName)
	    {
		    string connectionString = GetConnectionString(connectionStringName);
		    using (IDbConnection connection = new SqlConnection(connectionString))
		    {
			    connection
				    .Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
		    }
	    }

		// Open Connection, Begin Transaction
		public void StartTransaction(string connectionStringName)
		{
			string connectionString = GetConnectionString(connectionStringName);
			_sqlConnection = new SqlConnection(connectionString);
			_sqlConnection.Open();
			_transaction = _sqlConnection.BeginTransaction();
		}

		// Close Connection, Stop Transaction
		public void CommitTransaction()
		{
			_transaction?.Commit();
			_sqlConnection?.Close();
		}
		public void	RollbackTransaction() 
		{
			_transaction?.Rollback();
			_sqlConnection?.Close();
		}

		// Load using the transaction
		public List<T> LoadDataInTransaction<T, T2>(string storedProcedure, T2 parameters)
		{
			List<T> rows = _sqlConnection.Query<T>(storedProcedure, parameters, 
								commandType: CommandType.StoredProcedure, transaction: _transaction).ToList();
			return rows;
		}

		// Save using the transaction
		public void SaveDataInTransaction<T>(string storedProcedure, T parameters)
		{
			_sqlConnection.Execute(storedProcedure, parameters, 
				commandType: CommandType.StoredProcedure, transaction: _transaction);
		}
		// Dispose

		public void Dispose()
		{
			CommitTransaction();
		}
    }
}
