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
using RuleEvaluator.Repository.Contract;

namespace RuleEvaluator.Repository.Database
{
    public class RuleItemsRepository : IRuleItemsRepository
    {
        private readonly ICellFactory _CellFactory;
        private readonly ICacheWrapper _CacheWrapper;
        private readonly string _ConnectionString;
        private readonly string _SplNameForColumns;
        private readonly string _SplNameForData;
        private readonly TimeSpan _CacheRelativeExpirationDefault;
        public IRuleItemsCall RuleItemsCall { get; set; }

        public RuleItemsRepository(ICellFactory p_CellFactory, ICacheWrapper p_CacheWrapper, string p_ConnectionString, string p_SplNameForColumns, string p_SplNameForData, TimeSpan p_CacheRelativeExpirationDefault)
        {
            _CellFactory = p_CellFactory;
            _CacheWrapper = p_CacheWrapper;
            _ConnectionString = p_ConnectionString;
            _SplNameForColumns = p_SplNameForColumns;
            _SplNameForData = p_SplNameForData;
            _CacheRelativeExpirationDefault = p_CacheRelativeExpirationDefault;
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
            using (var conn = new SqlConnection(_ConnectionString))
            {
                conn.Open();

                List<ColumnSettings> columnSettings = conn.Query<ColumnSettings>(_SplNameForColumns, new { Key = p_Key }, commandType: CommandType.StoredProcedure).ToList();
                List<IDictionary<string, object>> data = (conn.Query(_SplNameForData, new { Key = p_Key }, commandType: CommandType.StoredProcedure) as IEnumerable<IDictionary<string, object>>).ToList();

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