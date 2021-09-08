namespace SamweiSpiderApp2021.AppCode
{
    #region _Spaces
    using Microsoft.Data.Sqlite;
    using System.Collections;
    using System.Data;
    using System.Reflection;
    using System.Text;
    #endregion

    class SQLiteHelper
    {
        #region _Variables
        private const string PrimaryColumnName = "Id";
        #endregion

        #region Init
        private static string _ConnPath = "";
        public SQLiteHelper()
        {
            _ConnPath = $"Data Source=SWSAppV1.db;Cache=Shared;";
        }
        #endregion

        #region CreateTable & Save
        public void Save<T>(List<T> Data)
        {
            if (Data == null || Data.Count < 1)
                return;

            //using var conn = new SqliteConnection(_ConnPath);

            #region Define
            var DbTypeArray = new Hashtable
            {
                { "System.Single", "FLOAT" },
                { "System.Int32", "INTEGER" },
                { "System.Int64", "BIGINT" },
                { "System.String", "TEXT" },
                { "System.Decimal", "NUMERIC" }
            };

            var ModelObject = Data[0].GetType();
            var TableName = ModelObject.Name;
            var DbColumns = new List<SQLiteColumnInfo> { };            
            #endregion

            #region Columns
            // 组织列
            foreach (var item in ModelObject.GetProperties())
            {
                var _TypeName = AppFun.GetString(DbTypeArray[item.PropertyType.FullName], true);

                DbColumns.Add(new SQLiteColumnInfo()
                {
                    Name = item.Name,
                    Type = item.PropertyType,
                    TypeName = _TypeName,
                    IsPrimary = item.PropertyType == typeof(int) && item.Name == PrimaryColumnName,
                    IsInsert = _TypeName.Length > 0
                });
            }
            #endregion

            //Step 1: Table
            CreateTable(TableName, DbColumns);

            var InsertColumnItem = new StringBuilder();
            var NeedInsertColumns = DbColumns.Where(q => q.IsInsert && !q.IsPrimary).ToList();
            foreach (var col in NeedInsertColumns)
                    InsertColumnItem.AppendFormat("{0},", col.Name);
            var InsertColumnSQL = string.Format(" INSERT INTO [{0}] ({1}) VALUES ", TableName, InsertColumnItem.ToString().TrimEnd(','));

            //Rows
            var InsertRowSQL = new StringBuilder();
            foreach (var row in Data)
            {
                var _InsertRowSQL =  new StringBuilder();
                foreach (var col in NeedInsertColumns)
                {
                    var _Value = row.GetType().GetProperty(col.Name).GetValue(row);
                    _InsertRowSQL.Append(col.Type== typeof(string)? $"'{_Value}',": $"{_Value},");
                }
                InsertRowSQL.AppendFormat("({0}),", _InsertRowSQL.ToString().TrimEnd(','));
            }

            //Execute
            var SaveQuerySQL = $"{InsertColumnSQL}{InsertRowSQL.ToString().TrimEnd(',')};";
            Console.WriteLine(SaveQuerySQL);
            ExecuteNonQuery(SaveQuerySQL, new Dictionary<string, string> { });
        }
        #endregion

        #region Create-Table
        /// <summary>
        /// 判断表存在
        /// </summary>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public bool TableExists(string TableName)
        {
            using var connection = new SqliteConnection(_ConnPath);
            var command = new SqliteCommand();
            var cmdText = string.Format("SELECT 1 FROM sqlite_master WHERE name='{0}'", TableName);
            PrepareCommand(command, connection, cmdText, new Dictionary<string, string> { });
            using var dr = command.ExecuteReader();
            bool IsExists = dr.HasRows;

            return IsExists;
        }

        /// <summary>
        /// 创建表
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="Columns"></param>
        public void CreateTable(string TableName, List<SQLiteColumnInfo> Columns)
        {
            if (TableExists(TableName))
                return;

            var CreateColumnSQL = new StringBuilder();
            var PrimaryKeyCount = 0;
            foreach (var item in Columns)
            {
                if (item.IsInsert)
                {
                    CreateColumnSQL.AppendFormat("  [{0}] {1},", item.Name, item.TypeName + (item.IsPrimary ? " NOT NULL UNIQUE" : ""));
                    if (item.IsPrimary)
                        PrimaryKeyCount++;
                }
            }
            var CreateTableSQL = string.Format("CREATE TABLE [{0}] ({1}{2});",
                TableName,
                CreateColumnSQL.ToString().TrimEnd(','),
                PrimaryKeyCount > 0 ? $",PRIMARY KEY([{PrimaryColumnName}] AUTOINCREMENT)" : "");

            Console.WriteLine(CreateTableSQL);
            ExecuteNonQuery(CreateTableSQL, new Dictionary<string, string> { });
        }
        #endregion

        #region Prepare-Command
        /// <summary>
        /// 准备操作命令参数
        /// </summary>
        /// <param name="cmd">SQLiteCommand</param>
        /// <param name="conn">SQLiteConnection</param>
        /// <param name="cmdText">Sql命令文本</param>
        /// <param name="data">参数数组</param>
        private void PrepareCommand(SqliteCommand cmd, SqliteConnection conn, string cmdText, Dictionary<String, String> data)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Parameters.Clear();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 30;
            if (data != null && data.Count >= 1)
            {
                foreach (KeyValuePair<string, string> val in data)
                    cmd.Parameters.AddWithValue(val.Key, val.Value);
            }
        }
        #endregion

        #region ExecuteNonQuery (RowCount)
        /// <summary>
        /// 执行数据库操作
        /// </summary>
        /// <param name="cmdText">Sql命令文本</param>
        /// <param name="data">传入的参数</param>
        /// <returns>返回受影响的行数</returns>
        public int ExecuteNonQuery(string cmdText, Dictionary<string, string> data)
        {
            using var connection = new SqliteConnection(_ConnPath);
            var command = new SqliteCommand();
            PrepareCommand(command, connection, cmdText, data);
            return command.ExecuteNonQuery();
        }
        #endregion

    }

    #region Model
    /// <summary>
    /// 列信息
    /// </summary>
    public class SQLiteColumnInfo
    {
        public string? Name { get; set; }
        public Type? Type { get; set; }
        public string? TypeName { get; set; }
        public bool IsPrimary { get; set; }
        public bool IsInsert { get; set; }
    }
    #endregion
}
