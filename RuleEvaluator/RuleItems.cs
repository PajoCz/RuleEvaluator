using System.Collections.Generic;

namespace RuleEvaluator
{
    public class RuleItems
    {
        private readonly ICellFactory _CellFactory;
        private readonly List<RuleItem> Data;

        public RuleItems(ICellFactory p_CellFactory)
        {
            _CellFactory = p_CellFactory;
            Data = new List<RuleItem>();
        }

        public void AddRuleItem(params object[] p_Cells)
        {
            Data.Add(new RuleItem(_CellFactory, p_Cells));
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