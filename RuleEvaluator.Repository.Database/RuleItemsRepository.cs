using System;
using System.Collections.Generic;
using System.Data;
#if NET472
using System.Data.SqlClient;
#else
using Microsoft.Data.SqlClient;
#endif
using System.Linq;
using Dapper;
using Npgsql;
using RuleEvaluator.Repository.Contract;

namespace RuleEvaluator.Repository.Database
{
    public enum DatabaseType
    {
        MSSQL,
        PostgreSQL
    }

    public class RuleItemsRepository : IRuleItemsRepository
    {
        private readonly ICellFactory _CellFactory;
        private readonly ICacheWrapper _CacheWrapper;
        private readonly string _ConnectionString;
        private readonly string _SplNameForColumns;
        private readonly string _SplNameForData;
        private readonly TimeSpan _CacheRelativeExpirationDefault;
        private readonly DatabaseType _DatabaseType;
        public IRuleItemsCall RuleItemsCall { get; set; }

        public RuleItemsRepository(ICellFactory p_CellFactory, ICacheWrapper p_CacheWrapper, string p_ConnectionString, string p_SplNameForColumns, string p_SplNameForData, TimeSpan p_CacheRelativeExpirationDefault, DatabaseType p_DatabaseType = DatabaseType.MSSQL)
        {
            _CellFactory = p_CellFactory;
            _CacheWrapper = p_CacheWrapper;
            _ConnectionString = p_ConnectionString;
            _SplNameForColumns = p_SplNameForColumns;
            _SplNameForData = p_SplNameForData;
            _CacheRelativeExpirationDefault = p_CacheRelativeExpirationDefault;
            _DatabaseType = p_DatabaseType;
        }

        public RuleItems Load(string p_Key)
        {
            return Load(p_Key, _CacheRelativeExpirationDefault);
        }

        public RuleItems Load(string p_Key, TimeSpan p_CacheRelativeExpiration)
        {
            return (RuleItems)_CacheWrapper.GetItem(() => LoadImpl(p_Key), p_CacheRelativeExpiration);
        }

        public string GetRule(string p_Key, params object[] p_Parameters)
        {
            var ruleItems = Load(p_Key);
            var found = ruleItems.Find(p_Parameters);
            if (found == null)
                return string.Empty;

            var result = found.CellsOnlyOutput[0].FilterValue;
            var resultAsString = result?.ToString() ?? string.Empty;
            return resultAsString;
        }

        public string GetRuleOrException(string p_Key, params object[] p_Parameters)
        {
            var found = FindOrException(p_Key, p_Parameters);
            var result = found.CellsOnlyOutput[0].FilterValue;
            var resultAsString = result?.ToString() ?? string.Empty;
            return resultAsString;
        }

        public RuleItem FindOrException(string p_Key, params object[] p_Parameters)
        {
            var ruleItems = Load(p_Key);
            var found = ruleItems.Find(p_Parameters);
            if (found == null)
                throw new Exception($"Not found in RuleEvaluator with key='{p_Key}' and parameters='{string.Join(",", p_Parameters.ToList())}'");
            return found;
        }

        private RuleItems LoadImpl(string p_Key)
        {
            IDbConnection conn = _DatabaseType == DatabaseType.PostgreSQL 
                ? (IDbConnection)new NpgsqlConnection(_ConnectionString) 
                : new SqlConnection(_ConnectionString);

            using (conn)
            {
                conn.Open();

                List<ColumnSettings> columnSettings;
                List<IDictionary<string, object>> data;
                
                if (_DatabaseType == DatabaseType.PostgreSQL)
                {
                    // PostgreSQL SELECT
                    columnSettings = conn.Query<ColumnSettings>($"SELECT * FROM {_SplNameForColumns}(@key)", new { key = p_Key }).ToList();
                    data = (conn.Query($"SELECT * FROM {_SplNameForData}(@key)", new { key = p_Key }) as IEnumerable<IDictionary<string, object>>).ToList();
                }
                else
                {
                    // MSSQL CommandType.StoredProcedure
                    columnSettings = conn.Query<ColumnSettings>(_SplNameForColumns, new { Key = p_Key }, commandType: CommandType.StoredProcedure).ToList();
                    data = (conn.Query(_SplNameForData, new { Key = p_Key }, commandType: CommandType.StoredProcedure) as IEnumerable<IDictionary<string, object>>).ToList();
                }

                var result = new RuleItems(_CellFactory, RuleItemsCall, p_Key);
                foreach (var d in data)
                {
                    object[] cellValues = new object[columnSettings.Count];
                    List<object> rowData = d.Values.ToList();
                    for (var iColumn = 0; iColumn < columnSettings.Count; iColumn++)
                    {
                        cellValues[iColumn] = _CellFactory.CreateCell(rowData[columnSettings[iColumn].Index] ?? string.Empty, columnSettings[iColumn].InputOutput);
                    }
                    result.AddRuleItem(cellValues);
                }
                return result;
            }
        }

        private class ColumnSettings
        {
            /// <summary>
            /// Index in data row, where is located FilterValue of cell
            /// </summary>
            public int Index { get; set; }
            public CellInputOutputType InputOutput { get; set; }
        }
    }
}