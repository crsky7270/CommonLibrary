using System;
using System.Collections;
using Couchbase;
using Couchbase.Extensions;
using Enyim.Caching.Memcached;
using Newtonsoft.Json;

namespace Crsky.Caching.CouchBase
{
   /// <summary>
   /// Initialize Couchbase Client Instance
   /// </summary>
   public static class CouchbaseManager
   {
      private static readonly long DefaultExpireTime = 60;// 默认超时时间(分钟计)
      private static CouchbaseClient _instance;
      static CouchbaseManager()
      {
         _instance = new CouchbaseClient();
      }

      /// <summary>
      /// reset couchbase client by new section name in app config file
      /// </summary>
      /// <param name="sectionName"></param>
      public static void ResetCouchClientBySectionName(string sectionName)
      {
         _instance = string.IsNullOrEmpty(sectionName) ? new CouchbaseClient() : new CouchbaseClient(sectionName);
      }

      private static CouchbaseClient Instance { get { return _instance; } }

      /// <summary>
      /// 添加缓存(未指定过期时间，使用默认过期时间)
      /// </summary>
      /// <param name="key">键名</param>
      /// <param name="value">键值</param>
      public static bool Add<T>(string key, T value)
      {
         return Add(key, value, DefaultExpireTime);
      }

      /// <summary>
      /// 添加缓存(指定缓存绝对过期时间)
      /// </summary>
      /// <param name="key">键名</param>
      /// <param name="value">键值</param>
      /// <param name="numOfMinutes">缓存绝对过期时间值(分钟计)</param>
      public static bool Add<T>(string key, T value, long numOfMinutes)
      {
         string serializeStr = JsonConvert.SerializeObject(value);
         return Instance.Store(StoreMode.Set, key, serializeStr, DateTime.Now.AddMinutes(numOfMinutes));
      }

      /// <summary>
      /// 添加缓存(指定缓存相对过期时间间隔)
      /// </summary>
      /// <param name="key">键名</param>
      /// <param name="value">键值</param>
      /// <param name="timeSpan">缓存相对过期时间间隔(分钟计)</param>
      public static bool Add<T>(string key, T value, TimeSpan timeSpan)
      {
         string serializeStr = JsonConvert.SerializeObject(value);
         return Instance.Store(StoreMode.Set, key, serializeStr, timeSpan);
      }

      /// <summary>
      /// 更新指定缓存
      /// </summary>
      /// <param name="key">键名</param>
      /// <param name="value">键值</param>
      /// <returns></returns>
      public static bool Update(string key, object value)
      {
         return Instance.Store(StoreMode.Replace, key, value);
      }

      /// <summary>
      /// 是否存在指定键名的Cache,并获取指定键名的Cache项
      /// </summary>
      /// <param name="key">键名</param>
      /// <param name="obj">键值</param>
      /// <returns>true为存在指定键名的Cache项</returns>
      public static bool GetCacheTryParse(string key, out object obj)
      {
         if (string.IsNullOrEmpty(key))
         {
            obj = null;
            return false;
         }

         obj = Get(key);

         return (obj != null);
      }

      /// <summary>
      /// 获取Cache值对象
      /// </summary>
      /// <param name="key">要获取的Cache项键名</param>
      /// <returns>Cache对象</returns>
      public static object Get(string key)
      {
         return Instance.Get(key);
      }

      /// <summary>
      /// 获取Cache值对象
      /// </summary>
      /// <param name="key">要获取的Cache项键名</param>
      /// <returns>Cache对象</returns>
      public static T Get<T>(string key) where T : class
      {
         var obj = Get(key);
         return obj != null ? JsonConvert.DeserializeObject<T>(obj.ToString()) : default(T);
      }

      /// <summary>
      /// 移除指定键的缓存
      /// </summary>
      /// <param name="key">键名</param>
      /// <returns></returns>
      public static bool Remove(string key)
      {
         return Instance.Remove(key);
      }
   }
}
