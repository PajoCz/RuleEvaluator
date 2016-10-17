using System;
using System.Data.SqlClient;

namespace RuleEvaluator.Repository.Database
{
    public static class SqlCommandExtensions
    {
        public static void ExecuteReaderWithAction(this SqlCommand p_Command, Action<SqlDataReader> p_Action)
        {
            using (var reader = p_Command.ExecuteReader())
            {
                while (reader.Read())
                {
                    p_Action(reader);
                }
            }
        }
    }
}
