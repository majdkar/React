using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using LazyCache;
using System.Linq.Dynamic.Core;
using System.Diagnostics;

namespace SchoolV01.Application.Extensions
{
    public static class MemoryCacheExtensions
    {
        #region Microsoft.Extensions.Caching.Memory_6_OR_OLDER

        private static readonly Lazy<Func<MemoryCache, object>> GetEntries6 =
            new(() => (Func<MemoryCache, object>)Delegate.CreateDelegate(
                typeof(Func<MemoryCache, object>),
                typeof(MemoryCache).GetProperty("EntriesCollection", BindingFlags.NonPublic | BindingFlags.Instance).GetGetMethod(true),
                throwOnBindFailure: true));

        #endregion

        #region Microsoft.Extensions.Caching.Memory_7_OR_NEWER

        private static readonly Lazy<Func<MemoryCache, object>> GetCoherentState =
            new(() =>
                CreateGetter<MemoryCache, object>(typeof(MemoryCache)
                    .GetField("_coherentState", BindingFlags.NonPublic | BindingFlags.Instance)));
        #endregion

        #region Microsoft.Extensions.Caching.Memory_7_TO_8.0.8
        private static readonly Lazy<Func<object, IDictionary>> GetEntries7 =
            new(() =>
                CreateGetter<object, IDictionary>(typeof(MemoryCache)
                    .GetNestedType("CoherentState", BindingFlags.NonPublic)
                    .GetField("_entries", BindingFlags.NonPublic | BindingFlags.Instance)));

        #endregion

        #region Microsoft.Extensions.Caching.Memory_8.0.10_OR_NEWER

        private static readonly Lazy<Func<object, IDictionary>> GetStringEntries8010 =
            new(() => CreateGetter<object, IDictionary>(typeof(MemoryCache)
                .GetNestedType("CoherentState", BindingFlags.NonPublic)
                .GetField("_stringEntries", BindingFlags.NonPublic | BindingFlags.Instance)));

        private static readonly Lazy<Func<object, IDictionary>> GetNonStringEntries8010 =
            new(() => CreateGetter<object, IDictionary>(typeof(MemoryCache)
                .GetNestedType("CoherentState", BindingFlags.NonPublic)
                .GetField("_nonStringEntries", BindingFlags.NonPublic | BindingFlags.Instance)));

        #endregion
        private static Func<TParam, TReturn> CreateGetter<TParam, TReturn>(FieldInfo field)
        {
            if (field == null)
                return null;

            try
            {
                var methodName = $"{field.ReflectedType.FullName}.get_{field.Name}";

                var method = new DynamicMethod(methodName, typeof(TReturn), [typeof(TParam)], typeof(TParam), true);
                var ilGen = method.GetILGenerator();
                ilGen.Emit(OpCodes.Ldarg_0);
                ilGen.Emit(OpCodes.Ldfld, field);
                ilGen.Emit(OpCodes.Ret);

                return (Func<TParam, TReturn>)method.CreateDelegate(typeof(Func<TParam, TReturn>));
            }
            catch (Exception)
            {
                return null;
            }
        }


        private static readonly Func<MemoryCache, IEnumerable> GetEntries =
            FileVersionInfo.GetVersionInfo(Assembly.GetAssembly(typeof(MemoryCache)).Location) switch
            {
                { ProductMajorPart: < 7 } =>
                    static cache => ((IDictionary)GetEntries6.Value(cache)).Keys,
                { ProductMajorPart: < 8 } or { ProductMajorPart: 8, ProductMinorPart: 0, ProductBuildPart: < 10 } =>
                    static cache => GetEntries7.Value(GetCoherentState.Value(cache)).Keys,
                _ =>
                    static cache => ((ICollection<string>)GetStringEntries8010.Value(GetCoherentState.Value(cache)).Keys)
                        .Concat((ICollection<object>)GetNonStringEntries8010.Value(GetCoherentState.Value(cache)).Keys)
            };

        public static IEnumerable GetKeys(this IAppCache cache)
        {
            if (cache == null)
                return null;

            var cacheProvider = cache.CacheProvider;
            if (cacheProvider == null)
                return null;

            var field = cacheProvider.GetType().GetField("cache", BindingFlags.Instance | BindingFlags.NonPublic);
            if (field == null)
                return null;

            if (field.GetValue(cacheProvider) is not MemoryCache memoryCache)
                return null;

            return GetEntries(memoryCache);
        }

        public static string[] GetKeys(this IAppCache cache, string name)
        {
            var cacheProvider = cache.CacheProvider;
            var field = cacheProvider.GetType().GetField("cache", BindingFlags.Instance | BindingFlags.NonPublic);

            if (field == null)
                return [];

            if (field.GetValue(cacheProvider) is not MemoryCache memoryCache)
                return [];

            return GetEntries(memoryCache)?
                .OfType<string>()
                .Where(x => x.Contains(name))
                .ToArray() ?? [];
        }

        public static IEnumerable<T> GetKeys<T>(this IAppCache cache) =>
            cache.GetKeys()?.OfType<T>() ?? [];
    }
}
