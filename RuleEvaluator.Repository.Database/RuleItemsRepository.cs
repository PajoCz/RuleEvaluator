using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace RuleEvaluator.Repository.Database
{
    public class RuleItemsRepository
    {
        private readonly ICellFactory _CellFactory;
        private readonly ICacheWrapper _CacheWrapper;
        private readonly string _ConnectionString;
        private readonly string _SplNameForColumns;
        private readonly string _SplNameForData;

        public RuleItemsRepository(ICellFactory p_CellFactory, ICacheWrapper p_CacheWrapper, string p_ConnectionString, string p_SplNameForColumns, string p_SplNameForData)
        {
            _CellFactory = p_CellFactory;
            _CacheWrapper = p_CacheWrapper;
            _ConnectionString = p_ConnectionString;
            _SplNameForColumns = p_SplNameForColumns;
            _SplNameForData = p_SplNameForData;
        }

        public RuleItems Load(string p_Key)
        {
            return (RuleItems)_CacheWrapper.GetItem(() => LoadImpl(p_Key), TimeSpan.FromMinutes(10));
        }

        public RuleItems LoadImpl(string p_Key)
        {
            using (var conn = new SqlConnection(_ConnectionString))
            {
                conn.Open();

                List<ColumnSettings> columnSettings = conn.Query<ColumnSettings>(_SplNameForColumns, new { Key = p_Key }, commandType: CommandType.StoredProcedure).ToList();
                List<IDictionary<string, object>> data = (conn.Query(_SplNameForData, new { Key = p_Key }, commandType: CommandType.StoredProcedure) as IEnumerable<IDictionary<string, object>>).ToList();

                var result = new RuleItems(_CellFactory);
                for (var iRow = 0; iRow < data.Count; iRow++)
                {
                    object[] cellValues = new object[columnSettings.Count];
                    List<object> rowData = data[iRow].Values.ToList();
                    for (var iColumn = 0; iColumn < columnSettings.Count; iColumn++)
                    {
                        cellValues[iColumn] = _CellFactory.CreateCell(rowData[columnSettings[iColumn].Index], columnSettings[iColumn].InputOutput);
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