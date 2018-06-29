using System;

namespace RuleEvaluator.Repository.Contract
{
    public interface IRuleItemsRepository
    {
        RuleItems Load(string p_Key);
        RuleItems Load(string p_Key, TimeSpan p_CacheRelativeExpiration);
        string GetRule(string p_Key, params object[] p_Parameters);
        string GetRuleOrException(string p_Key, params object[] p_Parameters);
        RuleItem FindOrException(string p_Key, params object[] p_Parameters);
    }
}