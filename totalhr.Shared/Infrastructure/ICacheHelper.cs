using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace totalhr.Shared.Infrastructure
{
    public interface ICacheHelper
    {
        void Add<T>(T o, string key);

        void Clear(string key);

        bool Exists(string key);

        bool Get<T>(string key, out T value);
    }
}
