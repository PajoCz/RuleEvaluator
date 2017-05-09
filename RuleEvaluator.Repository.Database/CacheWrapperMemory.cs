using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Caching;
using System.Text;

namespace RuleEvaluator.Repository.Database
{
    public class CacheWrapperMemory : ICacheWrapper
    {
        public object GetItem(Expression<Func<object>> p_Action, TimeSpan p_RelativeExpiration)
        {
            return GetItem(p_Action, p_Action.Compile(), p_RelativeExpiration);
        }

        public object GetItem(Expression<Func<object>> p_CacheKeyExpression, Func<object> p_NotFoundInCacheItemRetrievalCallback, TimeSpan p_RelativeExpiration)
        {
            var body = (MethodCallExpression)p_CacheKeyExpression.Body;
            var callerType = body.Object?.Type;
            var parameters = body.Arguments.Cast<MemberExpression>().Select(expression => ((FieldInfo)expression.Member).GetValue(((ConstantExpression)expression.Expression).Value));

            // construct the cache key for a cache item lookup/insertion by joining the cache key parts
            var cacheKey = new StringBuilder()
                .Append(callerType?.FullName)
                .Append(".")
                .Append(body.Method.Name)
                .Append(parameters.Any() ? "_" + string.Join("_", parameters) : string.Empty)
                .ToString();

            // try to get the desired item from the cache based on the key we just constructed
            var item = MemoryCache.Default.Get(cacheKey);

            // otherwise call the callback for getting the item 
            // and put the result of the callback into the mem cache for a specified duration
            if (item == null)
            {
                item = p_NotFoundInCacheItemRetrievalCallback();
                MemoryCache.Default.Add(cacheKey, item, DateTime.Now + p_RelativeExpiration);
            }

            return item;
        }

        public void ClearAll()
        {
            //After call Dispose is internal state IsDisposed=1 and nothing is caching
            //MemoryCache.Default.Dispose();

            //bad performance
            //http://blog.aggregatedintelligence.com/2011/01/listing-contents-of-memorycache.html
            //http://stackoverflow.com/questions/4183270/how-to-clear-the-net-4-memorycache
            MemoryCache memoryCache = MemoryCache.Default;
            IDictionaryEnumerator cacheEnumerator = (IDictionaryEnumerator)((IEnumerable)memoryCache).GetEnumerator();
            while (cacheEnumerator.MoveNext())
            {
                memoryCache.Remove(cacheEnumerator.Key.ToString());
            }

        }
    }
}
