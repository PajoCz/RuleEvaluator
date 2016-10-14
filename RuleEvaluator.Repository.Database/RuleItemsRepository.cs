using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace RuleEvaluator.Repository.Database
{
    public class RuleItemsRepository
    {
        private readonly string _ConnectionString;
        private readonly string _SplNameForColumns;
        private readonly string _SplNameForData;
        private readonly string _SplParamNameForKey;

        public RuleItemsRepository(string p_ConnectionString, string p_SplNameForColumns, string p_SplNameForData, string p_SplParamNameForKey = "Key")
        {
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

                List<ColumnIndexInOut> cols = SelectColumnsSettings(conn, p_Key);
                var list = FillRuleItems(conn, p_Key, cols);
                return new RuleItems(list);
            }
        }

        private List<ColumnIndexInOut> SelectColumnsSettings(SqlConnection p_Connection, string p_SplParamInput)
        {
            List<ColumnIndexInOut> result = new List<ColumnIndexInOut>();
            var cmd = SqlCommand(p_Connection, _SplNameForColumns, _SplParamNameForKey, p_SplParamInput);
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var colIndex = reader.GetInt32(0);
                    var isOut = Convert.ToBoolean(reader.GetInt32(1));
                    result.Add(new ColumnIndexInOut(colIndex, isOut ? CellInputOutputType.Output : CellInputOutputType.Input));
                }
            }
            return result;
        }

        private List<RuleItem> FillRuleItems(SqlConnection p_Connection, string p_SplParamInput, List<ColumnIndexInOut> p_Columns)
        {
            List<RuleItem> result = new List<RuleItem>();
            var cmd = SqlCommand(p_Connection, _SplNameForData, _SplParamNameForKey, p_SplParamInput);
            object[] cellValues = new object[p_Columns.Count];
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var i = 0;
                    foreach (var col in p_Columns)
                    {
                        cellValues[i] = new Cell(reader.GetString(col.Index), col.InputOutput);
                        i++;
                    }
                    result.Add(new RuleItem(cellValues));
                }
            }
            return result;
        }

        private static SqlCommand SqlCommand(SqlConnection p_ConnectionString, string p_SplName, string p_SplParamName, string p_SplParamValue)
        {
            var cmd = p_ConnectionString.CreateCommand();
            cmd.CommandText = p_SplName;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue(p_SplParamName, p_SplParamValue);
            return cmd;
        }

        private class ColumnIndexInOut
        {
            public ColumnIndexInOut(int p_Index, CellInputOutputType p_InputOutput)
            {
                Index = p_Index;
                InputOutput = p_InputOutput;
            }

            public int Index { get; }
            public CellInputOutputType InputOutput { get; }
        }
    }
}