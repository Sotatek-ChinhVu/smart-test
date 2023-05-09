using System.Data;
using System.Data.SqlClient;

namespace Reporting.CommonMasters.Services;

public class RenkeiMsSqlDataAccess : IDisposable
{
    public RenkeiMsSqlDataAccess()
    {
        Connection = new SqlConnection(DbConnectionSetting.RenkeiConnectionString);
        Connection.Open();
    }

    public RenkeiMsSqlDataAccess(string hostName, string dbName, string userName, string password)
    {
        string renkeiConnectionString = "Data Source=" + hostName +
            ";Initial Catalog=" + dbName +
            ";Integrated Security=False" +
            ";UID=" + userName +
            ";pwd=" + password +
            ";MultipleActiveResultSets=True;";
        Connection = new SqlConnection(renkeiConnectionString);
        Connection.Open();
    }

    protected void PrepareCommandAndParameters(SqlCommand command, List<SqlParameter> listParams)
    {
        if (listParams != null)
        {
            command.Parameters.AddRange(listParams.ToArray());
        }
    }

    protected SqlConnection Connection { get; set; }

    protected SqlTransaction Transaction { get; set; }

    public SqlTransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
    {
        Transaction = Connection.BeginTransaction(isolationLevel);
        return Transaction;
    }

    /// <summary>
    /// Execute select query
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public DataTable ExecuteReader(string sql, List<SqlParameter> parameters = null)
    {
        using (var sqlCommand = new SqlCommand(sql, Connection))
        {
            PrepareCommandAndParameters(sqlCommand, parameters);
            if (Transaction != null)
            {
                sqlCommand.Transaction = Transaction;
            }

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            return dataTable;
        }
    }

    /// <summary>
    /// Execute select 1 field or sum, count,...
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public object ExecuteScalar(string sql, List<SqlParameter> parameters = null)
    {
        using (var sqlCommand = new SqlCommand(sql, Connection))
        {
            PrepareCommandAndParameters(sqlCommand, parameters);
            if (Transaction != null)
            {
                sqlCommand.Transaction = Transaction;
            }
            return sqlCommand.ExecuteScalar();
        }
    }

    /// <summary>
    /// Execute insert, update or delete sql
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public int ExecuteNonQuery(string sql, List<SqlParameter> parameters = null)
    {
        using (var sqlCommand = new SqlCommand(sql, Connection))
        {
            PrepareCommandAndParameters(sqlCommand, parameters);
            if (Transaction != null)
            {
                sqlCommand.Transaction = Transaction;
            }
            return sqlCommand.ExecuteNonQuery();
        }
    }

    public void Dispose()
    {
        Transaction?.Dispose();

        Connection?.Close();
        Connection?.Dispose();
    }
}
