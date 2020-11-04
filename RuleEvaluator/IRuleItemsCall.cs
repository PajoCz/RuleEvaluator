using System.Collections.Generic;

namespace RuleEvaluator
{
    public interface IRuleItemsCall
    {
        void FindCalled(RuleItemsCallFindCalled p_CalledInfo);
    }

    public class RuleItemsCallFindCalled
    {
        public RuleItemsCallFindCalledMethod Method { get; set; }
        public string Name { get; set; }
        public List<object> Inputs { get; set; }
        public List<List<object>> Outputs { get; set; }
    }

    public enum RuleItemsCallFindCalledMethod
    {
        Find,
        FindAll,
        GetAll
    }
}