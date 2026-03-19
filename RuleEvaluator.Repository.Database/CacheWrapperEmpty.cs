using System;
using System.Linq.Expressions;
using RuleEvaluator.Repository.Contract;

namespace RuleEvaluator.Repository.Database
{
    /// <summary>
    /// No-op cache wrapper that always retrieves fresh data.
    /// </summary>
    public class CacheWrapperEmpty : ICacheWrapper
    {
        public object? GetItem(string cacheKey, Func<object> retrievalCallback, TimeSpan relativeExpiration)
        {
            return retrievalCallback();
        }

        public void ClearAll()
        {
        }

        [Obsolete("Use GetItem(string, Func<object>, TimeSpan) instead.")]
        public object? GetItem(Expression<Func<object>> p_Action, TimeSpan p_RelativeExpiration)
        {
            return GetItem(p_Action, p_Action.Compile(), p_RelativeExpiration);
        }

        [Obsolete("Use GetItem(string, Func<object>, TimeSpan) instead.")]
        public object? GetItem(Expression<Func<object>> p_CacheKeyExpression, Func<object> p_NotFoundInCacheItemRetrievalCallback, TimeSpan p_RelativeExpiration)
        {
            return p_NotFoundInCacheItemRetrievalCallback();
        }
    }
}
