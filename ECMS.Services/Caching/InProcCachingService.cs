using ECMS.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;
namespace ECMS.Services
{
    public class InProcCachingService : ICachingService
    {
        private ObjectCache _cache = MemoryCache.Default;
        public T Get<T>(string key_)
        {
            return (T)_cache.Get(key_);
        }

        public void Set<T>(string key_, T value_)
        {
            _cache.Set(key_, value_, null);
        }

    }
}
