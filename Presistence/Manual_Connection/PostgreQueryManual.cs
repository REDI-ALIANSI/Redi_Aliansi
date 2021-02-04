using Application.Common.Interfaces;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Presistence.Manual_Connection
{
    public class PostgreQueryManual : IPostgreConnection
    {
        public async Task NonQuery(string query, string conn)
        {
            NpgsqlConnection _connection = new NpgsqlConnection(conn);
            NpgsqlCommand cmd = new NpgsqlCommand(query, _connection);
            _connection.Open();
            await cmd.ExecuteNonQueryAsync();
            cmd.Dispose();
            _connection.Close();
        }

        public async Task<DataSet> ReturnQuery(string query, string conn)
        {
            NpgsqlConnection _connection = new NpgsqlConnection(conn);
            NpgsqlCommand cmd = new NpgsqlCommand(query, _connection);
            _connection.Open();
            object cursorVal = await cmd.ExecuteScalarAsync();
            DataSet ds = FetchAll(_connection, cursorVal);
            cmd.Dispose();
            _connection.Close();

            return ds;
        }

        private DataSet FetchAll(NpgsqlConnection _connection, object cursorVal)
        {
            try
            {
                DataSet actualData = new DataSet();

                string strSql = "fetch all from \"" + cursorVal + "\";";
                NpgsqlCommand cmd = new NpgsqlCommand(strSql, _connection);
                NpgsqlDataAdapter ada = new NpgsqlDataAdapter(cmd);
                ada.Fill(actualData);

                return actualData;

            }
            catch (Exception Exp)
            {
                throw new Exception(Exp.Message);
            }
        }
    }
}
