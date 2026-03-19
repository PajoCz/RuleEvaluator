using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Caching;
using System.Text;
using RuleEvaluator.Repository.Contract;

namespace RuleEvaluator.Repository.Database
{
    /// <summary>
    /// In-memory cache wrapper using MemoryCache.
    /// </summary>
    public class CacheWrapperMemory : ICacheWrapper
    {
        public object? GetItem(string cacheKey, Func<object> retrievalCallback, TimeSpan relativeExpiration)
        {
            var item = MemoryCache.Default.Get(cacheKey);
            if (item == null)
            {
                item = retrievalCallback();
                MemoryCache.Default.Add(cacheKey, item, DateTime.Now + relativeExpiration);
            }
            return item;
        }

        [Obsolete("Use GetItem(string, Func<object>, TimeSpan) instead.")]
        public object? GetItem(Expression<Func<object>> p_Action, TimeSpan p_RelativeExpiration)
        {
            return GetItem(p_Action, p_Action.Compile(), p_RelativeExpiration);
        }

        [Obsolete("Use GetItem(string, Func<object>, TimeSpan) instead.")]
        public object? GetItem(Expression<Func<object>> p_CacheKeyExpression, Func<object> p_NotFoundInCacheItemRetrievalCallback, TimeSpan p_RelativeExpiration)
        {
            var body = (MethodCallExpression)p_CacheKeyExpression.Body;
            var callerType = body.Object?.Type;
            var parameters = body.Arguments.Cast<MemberExpression>().Select(expression => ((FieldInfo)expression.Member).GetValue(((ConstantExpression)expression.Expression!).Value));

            var cacheKey = new StringBuilder()
                .Append(callerType?.FullName)
                .Append(".")
                .Append(body.Method.Name)
                .Append(parameters.Any() ? "_" + string.Join("_", parameters) : string.Empty)
                .ToString();

            return GetItem(cacheKey, p_NotFoundInCacheItemRetrievalCallback, p_RelativeExpiration);
        }

        public void ClearAll()
        {
            MemoryCache memoryCache = MemoryCache.Default;
            IDictionaryEnumerator cacheEnumerator = (IDictionaryEnumerator)((IEnumerable)memoryCache).GetEnumerator();
            while (cacheEnumerator.MoveNext())
            {
                memoryCache.Remove(cacheEnumerator.Key.ToString()!);
            }
        }
    }
}
