/*
 * 作者:丁静
 * 创建时间:2014-10-30
 * ------------------修改记录-------------------
 * 修改人      修改日期        修改目的
 * 丁静        2014-10-30      创建
 */
using System;
using RuanYun.Caching.CouchBase;
using RuanYun.Logger;

namespace QBMS.Common.CouchBase
{
    /// <summary>
    /// CouchBase工厂类
    /// </summary>
    public static class CouchBaseFactory
    {
        private static CouchBaseProtoBufManager client = new CouchBaseProtoBufManager();
        private static readonly object Lock = new object();

        /// <summary>
        /// 获取CouchbaseClient,从Web.config读取配置
        /// </summary>
        /// <returns></returns>
        public static CouchBaseProtoBufManager GetCouchBaseClient()
        {
            try
            {
                if (client != null)
                {
                    return client;
                }
                else
                {
                    client = new CouchBaseProtoBufManager();
                    return client;
                }
            }
            catch (Exception ex)
            {
                Log.Write("初始化CouchbaseClient出错", MessageType.Error, typeof(CouchBaseFactory), ex);
                return null;
            }
        }
    }
}
