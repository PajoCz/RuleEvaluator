using System.Collections.Generic;

namespace RuleEvaluator
{
    public class RuleItems
    {
        public readonly List<RuleItem> Data;

        public RuleItems(List<RuleItem> p_Data)
        {
            Data = p_Data;
        }

        public List<RuleItem> FindAll(params object[] p_FindParams)
        {
            return Data.FindAll(i => i.ValidateInput(p_FindParams));
        }

        public RuleItem Find(params object[] p_FindParams)
        {
            return Data.Find(i => i.ValidateInput(p_FindParams));
        }
    }
}