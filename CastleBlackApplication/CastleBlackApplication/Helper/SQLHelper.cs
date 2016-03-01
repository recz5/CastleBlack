using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CastleBlackApplication.Helper
{
    public class SQLHelper
    {
        public static void UpsertDataTable(string sqlConn, DataTable dT, string sprocDoUpsert)
        {
            SqlConnection connection = new SqlConnection(sqlConn);

            SqlCommand command = new SqlCommand(sprocDoUpsert, connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("@tvpTable", dT));            

            connection.Open();

            connection.ChangeDatabase("CastleBlack");

            command.ExecuteNonQuery();

            connection.Close();
        }
    }
}
