using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace TRMBackEnd.Library.Internal.DataAccess
{
    internal class SqlDataAccess : IDisposable
    {
	    private readonly IConfiguration _config;
	    private IDbConnection _sqlConnection;
	    private IDbTransaction _transaction;
	    private bool _isClosed = false;

	    public SqlDataAccess(IConfiguration config)
	    {
		    _config = config;

	    }
		public string GetConnectionString(string name)
		{
			return _config.GetConnectionString(name);
		    //return ConfigurationManager.ConnectionStrings[name].ConnectionString;
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

		public void StartTransaction(string connectionStringName)
		{
			string connectionString = GetConnectionString(connectionStringName);
			_sqlConnection = new SqlConnection(connectionString);
			_sqlConnection.Open();
			_transaction = _sqlConnection.BeginTransaction();
			_isClosed = false;
		}

		public List<T> LoadDataInTransaction<T, T2>(string storedProcedure, T2 parameters)
		{
			List<T> rows = _sqlConnection.Query<T>(storedProcedure, parameters, 
								commandType: CommandType.StoredProcedure, transaction: _transaction).ToList();
			return rows;
		}

		public void SaveDataInTransaction<T>(string storedProcedure, T parameters)
		{
			_sqlConnection.Execute(storedProcedure, parameters, 
				commandType: CommandType.StoredProcedure, transaction: _transaction);
		}

		public void CommitTransaction()
		{
			_transaction?.Commit();
			CloseConnection();
		}

		public void RollbackTransaction()
		{
			_transaction?.Rollback();
			CloseConnection();
		}

		public void Dispose()
		{
			if (_isClosed == false)
			{
				try
				{
					CommitTransaction();
				}
				catch
				{
					// TODO - Log this issue
				}
			}

			_transaction = null;
			_sqlConnection = null;
		}

		public void CloseConnection()
		{
			_sqlConnection?.Close();
			_isClosed = true;
		}
	}
}
