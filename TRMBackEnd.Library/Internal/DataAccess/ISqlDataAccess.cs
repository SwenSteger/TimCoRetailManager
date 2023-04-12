using System.Collections.Generic;

namespace TRMBackEnd.Library.Internal.DataAccess
{
	public interface ISqlDataAccess
	{
		string GetConnectionString(string name);
		List<T> LoadData<T, T2>(string storedProcedure, T2 parameters, string connectionStringName);
		void SaveData<T>(string storedProcedure, T parameters, string connectionStringName);
		void StartTransaction(string connectionStringName);
		List<T> LoadDataInTransaction<T, T2>(string storedProcedure, T2 parameters);
		void SaveDataInTransaction<T>(string storedProcedure, T parameters);
		void CommitTransaction();
		void RollbackTransaction();
		void Dispose();
		void CloseConnection();
	}
}