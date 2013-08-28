using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECMS.Core.Interfaces
{
    public interface ICachingService
    {
        T Get<T>(string key_);
        void Set<T>(string key_, T value_);
    }
}
