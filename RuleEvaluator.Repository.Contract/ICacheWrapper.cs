using System;
using System.Linq.Expressions;

namespace RuleEvaluator.Repository.Contract
{
    /// <summary>
    /// Cache abstraction for rule item repositories.
    /// </summary>
    public interface ICacheWrapper
    {
        /// <summary>
        /// Gets an item from cache by key, or retrieves it using the callback if not cached.
        /// </summary>
        object? GetItem(string cacheKey, Func<object> retrievalCallback, TimeSpan relativeExpiration);

        /// <summary>
        /// Clears all cached items.
        /// </summary>
        void ClearAll();

        #region Legacy API - maintained for backward compatibility

        /// <summary>
        /// Legacy API. Use GetItem(string, Func&lt;object&gt;, TimeSpan) instead.
        /// </summary>
        [Obsolete("Use GetItem(string, Func<object>, TimeSpan) instead. This method exists for backward compatibility.")]
        object? GetItem(Expression<Func<object>> p_CacheKeyExpression, Func<object> p_NotFoundInCacheItemRetrievalCallback, TimeSpan p_RelativeExpiration);

        /// <summary>
        /// Legacy API. Use GetItem(string, Func&lt;object&gt;, TimeSpan) instead.
        /// </summary>
        [Obsolete("Use GetItem(string, Func<object>, TimeSpan) instead. This method exists for backward compatibility.")]
        object? GetItem(Expression<Func<object>> p_Action, TimeSpan p_RelativeExpiration);

        #endregion
    }
}
