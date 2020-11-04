using System.Collections.Generic;
using System.Linq;

namespace RuleEvaluator
{
    public class RuleItems
    {
        public readonly string Name;
        private readonly ICellFactory _CellFactory;
        private readonly IRuleItemsCall _RuleItemsCall;
        private readonly List<RuleItem> Data;

        public RuleItems(ICellFactory p_CellFactory)
        {
            _CellFactory = p_CellFactory;
            Data = new List<RuleItem>();
        }

        public RuleItems(ICellFactory p_CellFactory, IRuleItemsCall p_RuleItemsCall, string p_Name)
        {
            _CellFactory = p_CellFactory;
            _RuleItemsCall = p_RuleItemsCall;
            Name = p_Name;
            Data = new List<RuleItem>();
        }

        public void AddRuleItem(params object[] p_Cells)
        {
            Data.Add(new RuleItem(_CellFactory, p_Cells));
        }

        public List<RuleItem> FindAll(params object[] p_FindParams)
        {
            var result = Data.FindAll(i => i.ValidateInput(Name, p_FindParams));
            _RuleItemsCall?.FindCalled(new RuleItemsCallFindCalled()
            {
                Method = RuleItemsCallFindCalledMethod.FindAll,
                Name = Name,
                Inputs = p_FindParams?.ToList(),
                Outputs = result.Select(r => r.CellsOnlyOutput.Select(c => c.FilterValue).ToList()).ToList()
            });
            return result;
        }

        public RuleItem Find(params object[] p_FindParams)
        {
            var result = Data.Find(i => i.ValidateInput(Name, p_FindParams));
            _RuleItemsCall?.FindCalled(new RuleItemsCallFindCalled()
            {
                Method = RuleItemsCallFindCalledMethod.Find,
                Name = Name,
                Inputs = p_FindParams?.ToList(),
                Outputs = result != null ? new List<List<object>>() {result.CellsOnlyOutput.Select(c => c.FilterValue).ToList()} : null
            });
            return result;
        }

        public List<RuleItem> GetAll()
        {
            _RuleItemsCall?.FindCalled(new RuleItemsCallFindCalled()
            {
                Method = RuleItemsCallFindCalledMethod.GetAll,
                Name = Name,
                Inputs = null,
                Outputs = Data?.Select(r => r.CellsOnlyOutput.Select(c => c.FilterValue).ToList()).ToList()
            });
            return Data;
        }
    }
}