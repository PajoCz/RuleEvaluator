using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Castle.Windsor;

namespace RuleEvaluator.Repository.Database
{
    public class RuleItemsRepository
    {
        private readonly IWindsorContainer _Container;
        private readonly string _ConnectionString;
        private readonly string _SplNameForColumns;
        private readonly string _SplNameForData;
        private readonly string _SplParamNameForKey;

        public RuleItemsRepository(IWindsorContainer p_Container, string p_ConnectionString, string p_SplNameForColumns, string p_SplNameForData, string p_SplParamNameForKey = "Key")
        {
            _Container = p_Container;
            _ConnectionString = p_ConnectionString;
            _SplNameForColumns = p_SplNameForColumns;
            _SplNameForData = p_SplNameForData;
            _SplParamNameForKey = p_SplParamNameForKey;
        }

        public RuleItems Load(string p_Key)
        {
            using (var conn = new SqlConnection(_ConnectionString))
            {
                conn.Open();

                List<ColumnSettings> cols = CreateColumnSettings(conn, p_Key);
                var result = new RuleItems(_Container);
                FillRuleItems(conn, p_Key, cols, result);
                return result;
            }
        }

        private List<ColumnSettings> CreateColumnSettings(SqlConnection p_Connection, string p_SplParamInput)
        {
            List<ColumnSettings> result = new List<ColumnSettings>();
            var cmd = CreateSqlCommand(p_Connection, _SplNameForColumns, _SplParamNameForKey, p_SplParamInput);
            cmd.ExecuteReaderWithAction(reader =>
            {
                var colIndex = reader.GetInt32(0);
                var isOut = Convert.ToBoolean(reader.GetInt32(1));
                result.Add(new ColumnSettings(colIndex, isOut ? CellInputOutputType.Output : CellInputOutputType.Input));
            });
            return result;
        }

        private void FillRuleItems(SqlConnection p_Connection, string p_SplParamInput, List<ColumnSettings> p_Columns, RuleItems p_RuleItems)
        {
            var cmd = CreateSqlCommand(p_Connection, _SplNameForData, _SplParamNameForKey, p_SplParamInput);
            cmd.ExecuteReaderWithAction(reader =>
            {
                var i = 0;
                object[] cellValues = new object[p_Columns.Count];
                foreach (var col in p_Columns)
                {
                    cellValues[i++] = new CellFactory(_Container).CreateCell(reader.GetString(col.Index), col.InputOutput);
                }
                p_RuleItems.AddRuleItem(cellValues);
            });
        }

        private static SqlCommand CreateSqlCommand(SqlConnection p_ConnectionString, string p_SplName, string p_SplParamName, string p_SplParamValue)
        {
            var cmd = p_ConnectionString.CreateCommand();
            cmd.CommandText = p_SplName;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue(p_SplParamName, p_SplParamValue);
            return cmd;
        }

        private class ColumnSettings
        {
            public ColumnSettings(int p_Index, CellInputOutputType p_InputOutput)
            {
                Index = p_Index;
                InputOutput = p_InputOutput;
            }

            public int Index { get; }
            public CellInputOutputType InputOutput { get; }
        }
    }
}