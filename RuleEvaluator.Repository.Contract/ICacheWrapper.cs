using System;
using System.Linq.Expressions;

namespace RuleEvaluator.Repository.Contract
{
    public interface ICacheWrapper
    {
        object GetItem(Expression<Func<object>> p_CacheKeyExpression, Func<object> p_NotFoundInCacheItemRetrievalCallback, TimeSpan p_RelativeExpiration);
        object GetItem(Expression<Func<object>> p_Action, TimeSpan p_RelativeExpiration);
        void ClearAll();
    }
}
