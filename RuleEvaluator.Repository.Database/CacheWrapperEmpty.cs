using System;
using System.Linq.Expressions;

namespace RuleEvaluator.Repository.Database
{
    public class CacheWrapperEmpty : ICacheWrapper
    {
        public object GetItem(Expression<Func<object>> p_Action, TimeSpan p_RelativeExpiration)
        {
            return GetItem(p_Action, p_Action.Compile(), p_RelativeExpiration);
        }

        public object GetItem(Expression<Func<object>> p_CacheKeyExpression, Func<object> p_NotFoundInCacheItemRetrievalCallback, TimeSpan p_RelativeExpiration)
        {
            return p_NotFoundInCacheItemRetrievalCallback();
        }

        public void ClearAll()
        {            
        }
    }
}
