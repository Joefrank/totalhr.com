using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.data.EF;
using totalhr.Shared.Infrastructure;

namespace totalhr.tests.libs
{
    public class TestGlossaryCacheHelper : ICacheHelper
    {
        Hashtable hash = new Hashtable();

        public TestGlossaryCacheHelper()
        {
        }

        public void Add<T>(T o, string key)
        {
            hash.Add(key, o);
        }

        public void Update<T>(T o, string key)
        {
            if (!Exists(key))
            {
                Add<T>(o, key);
            }
            else
            {
                hash[key] = o;
            }
        }
        /// <summary>
        /// Remove item from cache
        /// </summary>
        /// <param name="key">Name of cached item</param>
        public void Clear(string key)
        {
            hash.Remove(key);
        }

        /// <summary>
        /// Check for item in cache
        /// </summary>
        /// <param name="key">Name of cached item</param>
        /// <returns></returns>
        public bool Exists(string key)
        {
            return hash[key] != null;
        }

        /// <summary>
        /// Retrieve cached item
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="key">Name of cached item</param>
        /// <param name="value">Cached value. Default(T) if 
        /// item doesn't exist.</param>
        /// <returns>Cached item as type</returns>
        public bool Get<T>(string key, out T value)
        {
            try
            {
                if (!Exists(key))
                {
                    value = default(T);
                    return false;
                }

                value = (T)hash[key];
            }
            catch
            {
                value = default(T);
                return false;
            }

            return true;
        }

        public T Get<T>(string key)
        {
            try
            {
                if (!Exists(key))
                {
                    return default(T);
                }

                return (T)hash[key];
            }
            catch
            {
                return default(T);
            }
        }
    }
}
