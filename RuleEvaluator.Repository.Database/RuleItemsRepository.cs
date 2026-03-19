using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
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
        private readonly ICellFactory _cellFactory;
        private readonly ICacheWrapper _cacheWrapper;
        private readonly string _connectionString;
        private readonly string _splNameForColumns;
        private readonly string _splNameForData;
        private readonly TimeSpan _cacheRelativeExpirationDefault;
        private readonly DatabaseType _databaseType;
        public IRuleItemsCall? RuleItemsCall { get; set; }

        public RuleItemsRepository(ICellFactory p_CellFactory, ICacheWrapper p_CacheWrapper, string p_ConnectionString, string p_SplNameForColumns, string p_SplNameForData, TimeSpan p_CacheRelativeExpirationDefault, DatabaseType p_DatabaseType = DatabaseType.MSSQL)
        {
            _cellFactory = p_CellFactory ?? throw new ArgumentNullException(nameof(p_CellFactory));
            _cacheWrapper = p_CacheWrapper ?? throw new ArgumentNullException(nameof(p_CacheWrapper));
            _connectionString = p_ConnectionString ?? throw new ArgumentNullException(nameof(p_ConnectionString));
            _splNameForColumns = p_SplNameForColumns ?? throw new ArgumentNullException(nameof(p_SplNameForColumns));
            _splNameForData = p_SplNameForData ?? throw new ArgumentNullException(nameof(p_SplNameForData));
            _cacheRelativeExpirationDefault = p_CacheRelativeExpirationDefault;
            _databaseType = p_DatabaseType;
        }

        public RuleItems Load(string p_Key)
        {
            return Load(p_Key, _cacheRelativeExpirationDefault);
        }

        public RuleItems Load(string p_Key, TimeSpan p_CacheRelativeExpiration)
        {
            var cacheKey = $"RuleItems_{_databaseType}_{p_Key}";
            return (RuleItems)_cacheWrapper.GetItem(cacheKey, () => LoadImpl(p_Key), p_CacheRelativeExpiration)!;
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
                throw new RuleNotFoundException(p_Key, p_Parameters);
            return found;
        }

        private RuleItems LoadImpl(string p_Key)
        {
            try
            {
                IDbConnection conn = _databaseType == DatabaseType.PostgreSQL
                    ? (IDbConnection)new NpgsqlConnection(_connectionString)
                    : new SqlConnection(_connectionString);

                using (conn)
                {
                    conn.Open();

                    List<ColumnSettings> columnSettings;
                    List<IDictionary<string, object>> data;

                    if (_databaseType == DatabaseType.PostgreSQL)
                    {
                        columnSettings = conn.Query<ColumnSettings>($"SELECT * FROM {_splNameForColumns}(@key)", new { key = p_Key }).ToList();
                        data = (conn.Query($"SELECT * FROM {_splNameForData}(@key)", new { key = p_Key }) as IEnumerable<IDictionary<string, object>>)!.ToList();
                    }
                    else
                    {
                        columnSettings = conn.Query<ColumnSettings>(_splNameForColumns, new { Key = p_Key }, commandType: CommandType.StoredProcedure).ToList();
                        data = (conn.Query(_splNameForData, new { Key = p_Key }, commandType: CommandType.StoredProcedure) as IEnumerable<IDictionary<string, object>>)!.ToList();
                    }

                    var result = new RuleItems(_cellFactory, RuleItemsCall, p_Key);
                    foreach (var d in data)
                    {
                        object[] cellValues = new object[columnSettings.Count];
                        List<object> rowData = d.Values.ToList();
                        for (var iColumn = 0; iColumn < columnSettings.Count; iColumn++)
                        {
                            cellValues[iColumn] = _cellFactory.CreateCell(rowData[columnSettings[iColumn].Index] ?? string.Empty, columnSettings[iColumn].InputOutput);
                        }
                        result.AddRuleItem(cellValues);
                    }
                    return result;
                }
            }
            catch (Exception ex) when (ex is not RuleEvaluatorException)
            {
                throw new RuleLoadException($"Failed to load ruleset '{p_Key}' from database", ex);
            }
        }

        private class ColumnSettings
        {
            public int Index { get; set; }
            public CellInputOutputType InputOutput { get; set; }
        }
    }
}