using System.Collections.Generic;
using Castle.Windsor;

namespace RuleEvaluator
{
    public class RuleItems
    {
        private readonly IWindsorContainer _Container;
        private readonly List<RuleItem> Data;

        public RuleItems(IWindsorContainer p_Container)
        {
            _Container = p_Container;
            Data = new List<RuleItem>();
        }

        public void AddRuleItem(params object[] p_Cells)
        {
            Data.Add(new RuleItem(_Container, p_Cells));
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