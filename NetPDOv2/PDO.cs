using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NetPDOv2 {
    public class PDO {
        private string connectionString = @"server=localhost;username=root;password=;database=db";
        private string statement;
        private MySqlConnection connection;
        MySqlCommand command;
        public void _connect() {
            MySqlConnection ms;
            try {
                ms = new MySqlConnection(connectionString);
            } catch (Exception) {
                ms = null;
            }
            connection = ms;
        }
        public MySqlCommand _generateCommand(MySqlConnection con) {
            MySqlCommand command;
            try {
                command = con.CreateCommand();
            } catch (Exception) {
                command = null;
            }
            return command;
        }
        public void prepare(string sql) {
            string[] strings = sql.Split('?');
            string nstr = "";
            if (strings.Length == 1) {
                nstr = sql;
            } else {
                for (int i = 0; i < strings.Length; i++) {
                    if (!string.IsNullOrEmpty(strings[i])) {
                        nstr += strings[i] + " @bind" + i.ToString();
                    }
                }
            }
            statement = nstr;
            try {
                command = _generateCommand(this.connection);
                command.CommandText = statement;
                command.CommandType = System.Data.CommandType.Text;
                command.Connection = this.connection;
            } catch (Exception) {
                this.command = null;
            }
        }
        public void bindValue(string key, string value) {
            command.Parameters.AddWithValue(key, value);
        }
        public void bindValues(string[] values) {
            for (int i = 0; i < values.Length; i++) {
                command.Parameters.AddWithValue("@bind" + i, values[i]);
            }
        }
        public bool execute(bool isFetch = false) {
            try {
                connection.Open();
                command.ExecuteNonQuery();
                if (!isFetch)
                    connection.Close();
                return true;
            } catch (Exception e) {
                throw e;
            }
        }
        public JArray fetchAllAsObj() {
            DataTable data = new DataTable();
            data.Load(command.ExecuteReader());
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in data.Rows) {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in data.Columns) {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
            connection.Close();
            return (JArray)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(rows, Formatting.Indented));
        }
        public string fetchAllAsStr() {
            DataTable data = new DataTable();
            data.Load(command.ExecuteReader());
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in data.Rows) {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in data.Columns) {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
            connection.Close();
            return JsonConvert.SerializeObject(rows, Formatting.Indented);
        }
    }
}
