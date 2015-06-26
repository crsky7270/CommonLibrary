using System.Collections.Generic;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Builders;

namespace Crsky.DBUtility
{
    /// <summary>
    /// MongoDb 增删改查操作封装类
    /// </summary>
    public sealed class MongoDbHelper
    {
        public static readonly string ConnectionStringDefault = System.Configuration.ConfigurationManager.AppSettings["ConnectionString_mongoDB_Primary"];
        public static readonly string DatabaseDefault = System.Configuration.ConfigurationManager.AppSettings["DefaultDatabase"];

        #region 新增

        public static WriteConcernResult InsertOne<T>(string collectionName, T entity)
        {
            return InsertOne<T>(ConnectionStringDefault, DatabaseDefault, collectionName, entity);
        }

        public static WriteConcernResult InsertOne<T>(string connectionString, string databaseName, string collectionName, T entity)
        {
            WriteConcernResult result;

            if (null == entity)
            {
                return null;
            }
            var client = new MongoClient(connectionString);
            var server = client.GetServer();
            //获取数据库或者创建数据库（不存在的话）。
            MongoDatabase database = server.GetDatabase(databaseName);

            using (server.RequestStart(database))//开始连接数据库。
            {
                MongoCollection<BsonDocument> myCollection = database.GetCollection<BsonDocument>(collectionName);
                result = myCollection.Insert(entity);
            }
            return result;
        }

        public static IEnumerable<WriteConcernResult> InsertAll<T>(string collectionName, IEnumerable<T> entitys)
        {
            return InsertAll<T>(ConnectionStringDefault, DatabaseDefault, collectionName, entitys);
        }

        public static IEnumerable<WriteConcernResult> InsertAll<T>(string connectionString, string databaseName, string collectionName, IEnumerable<T> entitys)
        {
            IEnumerable<WriteConcernResult> result;

            if (null == entitys)
            {
                return null;
            }

            var client = new MongoClient(connectionString);
            var server = client.GetServer();

            //获取数据库或者创建数据库（不存在的话）。
            MongoDatabase database = server.GetDatabase(databaseName);

            using (server.RequestStart(database))//开始连接数据库。
            {
                MongoCollection<BsonDocument> myCollection = database.GetCollection<BsonDocument>(collectionName);
                result = myCollection.InsertBatch(entitys);
            }

            return result;
        }

        #endregion

        #region 修改

        public static WriteConcernResult UpdateOne<T>(string collectionName, T entity)
        {
            return UpdateOne<T>(ConnectionStringDefault, DatabaseDefault, collectionName, entity);
        }

        public static WriteConcernResult UpdateOne<T>(string connectionString, string databaseName, string collectionName, T entity)
        {
            var client = new MongoClient(connectionString);
            var server = client.GetServer();

            //获取数据库或者创建数据库（不存在的话）。
            MongoDatabase database = server.GetDatabase(databaseName);

            WriteConcernResult result;

            using (server.RequestStart(database))//开始连接数据库。
            {
                MongoCollection<BsonDocument> myCollection = database.GetCollection<BsonDocument>(collectionName);

                result = myCollection.Save(entity);
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="query">条件查询。 调用示例：Query.Matches("Title", "女神") 或者 Query.EQ("Title", "女神") 或者Query.And(Query.Matches("Title", "女神"),Query.EQ("Author", "yanc")) 等等</param>
        /// <param name="update">更新设置。调用示例：Update.Set("Title", "yanc") 或者 Update.Set("Title", "yanc").Set("Author", "yanc2") 等等</param>
        /// <returns></returns>
        public static WriteConcernResult UpdateAll<T>(string collectionName, IMongoQuery query, IMongoUpdate update)
        {
            return UpdateAll<T>(ConnectionStringDefault, DatabaseDefault, collectionName, query, update);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connectionString"></param>
        /// <param name="databaseName"></param>
        /// <param name="collectionName"></param>
        /// <param name="query">条件查询。 调用示例：Query.Matches("Title", "女神") 或者 Query.EQ("Title", "女神") 或者Query.And(Query.Matches("Title", "女神"),Query.EQ("Author", "yanc")) 等等</param>
        /// <param name="update">更新设置。调用示例：Update.Set("Title", "yanc") 或者 Update.Set("Title", "yanc").Set("Author", "yanc2") 等等</param>
        /// <returns></returns>
        public static WriteConcernResult UpdateAll<T>(string connectionString, string databaseName, string collectionName, IMongoQuery query, IMongoUpdate update)
        {
            WriteConcernResult result;

            if (null == query || null == update)
            {
                return null;
            }

            var client = new MongoClient(connectionString);
            var server = client.GetServer();

            //获取数据库或者创建数据库（不存在的话）。
            MongoDatabase database = server.GetDatabase(databaseName);

            using (server.RequestStart(database))//开始连接数据库。
            {
                MongoCollection<BsonDocument> myCollection = database.GetCollection<BsonDocument>(collectionName);

                result = myCollection.Update(query, update, UpdateFlags.Multi);
            }
            return result;
        }
        #endregion

        #region 删除

        public static WriteConcernResult Delete(string collectionName, string id)
        {
            return Delete(ConnectionStringDefault, DatabaseDefault, collectionName, id);
        }

        public static WriteConcernResult Delete(string connectionString, string databaseName, string collectionName, string _id)
        {
            WriteConcernResult result;
            ObjectId id;
            if (!ObjectId.TryParse(_id, out id))
            {
                return null;
            }

            var client = new MongoClient(connectionString);
            var server = client.GetServer();

            //获取数据库或者创建数据库（不存在的话）。
            MongoDatabase database = server.GetDatabase(databaseName);

            using (server.RequestStart(database))//开始连接数据库。
            {
                MongoCollection<BsonDocument> myCollection = database.GetCollection<BsonDocument>(collectionName);

                result = myCollection.Remove(Query.EQ("_id", id));
            }
            return result;
        }

        public static WriteConcernResult DeleteAll(string collectionName)
        {
            return DeleteAll(ConnectionStringDefault, DatabaseDefault, collectionName, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="query">条件查询。 调用示例：Query.Matches("Title", "女神") 或者 Query.EQ("Title", "女神") 或者Query.And(Query.Matches("Title", "女神"),Query.EQ("Author", "yanc")) 等等</param>
        /// <returns></returns>
        public static WriteConcernResult DeleteAll(string collectionName, IMongoQuery query)
        {
            return DeleteAll(ConnectionStringDefault, DatabaseDefault, collectionName, query);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="databaseName"></param>
        /// <param name="collectionName"></param>
        /// <param name="query">条件查询。 调用示例：Query.Matches("Title", "女神") 或者 Query.EQ("Title", "女神") 或者Query.And(Query.Matches("Title", "女神"),Query.EQ("Author", "yanc")) 等等</param>
        /// <returns></returns>
        public static WriteConcernResult DeleteAll(string connectionString, string databaseName, string collectionName, IMongoQuery query)
        {
            var client = new MongoClient(connectionString);
            var server = client.GetServer();

            //获取数据库或者创建数据库（不存在的话）。
            MongoDatabase database = server.GetDatabase(databaseName);

            WriteConcernResult result;

            using (server.RequestStart(database))//开始连接数据库。
            {
                MongoCollection<BsonDocument> myCollection = database.GetCollection<BsonDocument>(collectionName);

                result = null == query ? myCollection.RemoveAll() : myCollection.Remove(query);
            }
            return result;

        }

        #endregion

        #region 获取单条信息

        public static T GetOne<T>(string collectionName, string _id)
        {
            return GetOne<T>(ConnectionStringDefault, DatabaseDefault, collectionName, _id);
        }

        public static T GetOne<T>(string connectionString, string databaseName, string collectionName, string _id)
        {
            T result;
            ObjectId id;
            if (!ObjectId.TryParse(_id, out id))
            {
                return default(T);
            }

            var client = new MongoClient(connectionString);
            var server = client.GetServer();

            //获取数据库或者创建数据库（不存在的话）。
            MongoDatabase database = server.GetDatabase(databaseName);

            using (server.RequestStart(database))//开始连接数据库。
            {
                MongoCollection<BsonDocument> myCollection = database.GetCollection<BsonDocument>(collectionName);

                result = myCollection.FindOneAs<T>(Query.EQ("_id", id));
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="query">条件查询。 调用示例：Query.Matches("Title", "女神") 或者 Query.EQ("Title", "女神") 或者Query.And(Query.Matches("Title", "女神"),Query.EQ("Author", "yanc")) 等等</param>
        /// <returns></returns>
        public static T GetOne<T>(string collectionName, IMongoQuery query)
        {
            return GetOne<T>(ConnectionStringDefault, DatabaseDefault, collectionName, query);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connectionString"></param>
        /// <param name="databaseName"></param>
        /// <param name="collectionName"></param>
        /// <param name="query">条件查询。 调用示例：Query.Matches("Title", "女神") 或者 Query.EQ("Title", "女神") 或者Query.And(Query.Matches("Title", "女神"),Query.EQ("Author", "yanc")) 等等</param>
        /// <returns></returns>
        public static T GetOne<T>(string connectionString, string databaseName, string collectionName, IMongoQuery query)
        {
            var client = new MongoClient(connectionString);
            var server = client.GetServer();

            //获取数据库或者创建数据库（不存在的话）。
            MongoDatabase database = server.GetDatabase(databaseName);

            T result;

            using (server.RequestStart(database))//开始连接数据库。
            {
                MongoCollection<BsonDocument> myCollection = database.GetCollection<BsonDocument>(collectionName);

                result = null == query ? myCollection.FindOneAs<T>() : myCollection.FindOneAs<T>(query);
            }
            return result;
        }

        #endregion

        #region 获取多个

        public static List<T> GetAll<T>(string collectionName)
        {
            return GetAll<T>(ConnectionStringDefault, DatabaseDefault, collectionName);
        }

        /// <summary>
        /// 如果不清楚具体的数量，一般不要用这个函数。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connectionString"></param>
        /// <param name="databaseName"></param>
        /// <param name="collectionName"></param>
        /// <returns></returns>
        public static List<T> GetAll<T>(string connectionString, string databaseName, string collectionName)
        {
            var client = new MongoClient(connectionString);
            var server = client.GetServer();

            //获取数据库或者创建数据库（不存在的话）。
            MongoDatabase database = server.GetDatabase(databaseName);

            var result = new List<T>();

            using (server.RequestStart(database))//开始连接数据库。
            {
                MongoCollection<BsonDocument> myCollection = database.GetCollection<BsonDocument>(collectionName);

                foreach (T entity in myCollection.FindAllAs<T>())
                {
                    result.Add(entity);
                }
            }
            return result;
        }

        public static List<T> GetAll<T>(string collectionName, int count)
        {
            return GetAll<T>(collectionName, count, null, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="count"></param>
        /// <param name="query">条件查询。 调用示例：Query.Matches("Title", "女神") 或者 Query.EQ("Title", "女神") 或者Query.And(Query.Matches("Title", "女神"),Query.EQ("Author", "yanc")) 等等</param>
        /// <returns></returns>
        public static List<T> GetAll<T>(string collectionName, int count, IMongoQuery query)
        {
            return GetAll<T>(collectionName, count, query, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="count"></param>
        /// <param name="sortBy">排序用的。调用示例：SortBy.Descending("Title") 或者 SortBy.Descending("Title").Ascending("Author")等等</param>
        /// <returns></returns>
        public static List<T> GetAll<T>(string collectionName, int count, IMongoSortBy sortBy)
        {
            return GetAll<T>(collectionName, count, null, sortBy);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="count"></param>
        /// <param name="query">条件查询。 调用示例：Query.Matches("Title", "女神") 或者 Query.EQ("Title", "女神") 或者Query.And(Query.Matches("Title", "女神"),Query.EQ("Author", "yanc")) 等等</param>
        /// <param name="sortBy">排序用的。调用示例：SortBy.Descending("Title") 或者 SortBy.Descending("Title").Ascending("Author")等等</param>
        /// <param name="fields">只返回所需要的字段的数据。调用示例："Title" 或者 new string[] { "Title", "Author" }等等</param>
        /// <returns></returns>
        public static List<T> GetAll<T>(string collectionName, int count, IMongoQuery query, IMongoSortBy sortBy, params string[] fields)
        {
            var pagerInfo = new PagerInfo { Page = 1, PageSize = count };
            return GetAll<T>(ConnectionStringDefault, DatabaseDefault, collectionName, query, pagerInfo, sortBy, fields);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="query">条件查询。 调用示例：Query.Matches("Title", "女神") 或者 Query.EQ("Title", "女神") 或者Query.And(Query.Matches("Title", "女神"),Query.EQ("Author", "yanc")) 等等</param>
        /// <param name="pagerInfo"></param>
        /// <returns></returns>
        public static List<T> GetAll<T>(string collectionName, IMongoQuery query, PagerInfo pagerInfo)
        {
            return GetAll<T>(ConnectionStringDefault, DatabaseDefault, collectionName, query, pagerInfo, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="query">条件查询。 调用示例：Query.Matches("Title", "女神") 或者 Query.EQ("Title", "女神") 或者Query.And(Query.Matches("Title", "女神"),Query.EQ("Author", "yanc")) 等等</param>
        /// <param name="pagerInfo"></param>
        /// <param name="sortBy">排序用的。调用示例：SortBy.Descending("Title") 或者 SortBy.Descending("Title").Ascending("Author")等等</param>
        /// <returns></returns>
        public static List<T> GetAll<T>(string collectionName, IMongoQuery query, PagerInfo pagerInfo, IMongoSortBy sortBy)
        {
            return GetAll<T>(ConnectionStringDefault, DatabaseDefault, collectionName, query, pagerInfo, sortBy);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="query">条件查询。 调用示例：Query.Matches("Title", "女神") 或者 Query.EQ("Title", "女神") 或者Query.And(Query.Matches("Title", "女神"),Query.EQ("Author", "yanc")) 等等</param>
        /// <param name="pagerInfo"></param>
        /// <param name="fields">只返回所需要的字段的数据。调用示例："Title" 或者 new string[] { "Title", "Author" }等等</param>
        /// <returns></returns>
        public static List<T> GetAll<T>(string collectionName, IMongoQuery query, PagerInfo pagerInfo, params string[] fields)
        {
            return GetAll<T>(ConnectionStringDefault, DatabaseDefault, collectionName, query, pagerInfo, null, fields);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="query">条件查询。 调用示例：Query.Matches("Title", "女神") 或者 Query.EQ("Title", "女神") 或者Query.And(Query.Matches("Title", "女神"),Query.EQ("Author", "yanc")) 等等</param>
        /// <param name="pagerInfo"></param>
        /// <param name="sortBy">排序用的。调用示例：SortBy.Descending("Title") 或者 SortBy.Descending("Title").Ascending("Author")等等</param>
        /// <param name="fields">只返回所需要的字段的数据。调用示例："Title" 或者 new string[] { "Title", "Author" }等等</param>
        /// <returns></returns>
        public static List<T> GetAll<T>(string collectionName, IMongoQuery query, PagerInfo pagerInfo, IMongoSortBy sortBy, params string[] fields)
        {
            return GetAll<T>(ConnectionStringDefault, DatabaseDefault, collectionName, query, pagerInfo, sortBy, fields);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connectionString"></param>
        /// <param name="databaseName"></param>
        /// <param name="collectionName"></param>
        /// <param name="query">条件查询。 调用示例：Query.Matches("Title", "女神") 或者 Query.EQ("Title", "女神") 或者Query.And(Query.Matches("Title", "女神"),Query.EQ("Author", "yanc")) 等等</param>
        /// <param name="pagerInfo"></param>
        /// <param name="sortBy">排序用的。调用示例：SortBy.Descending("Title") 或者 SortBy.Descending("Title").Ascending("Author")等等</param>
        /// <param name="fields">只返回所需要的字段的数据。调用示例："Title" 或者 new string[] { "Title", "Author" }等等</param>
        /// <returns></returns>
        public static List<T> GetAll<T>(string connectionString, string databaseName, string collectionName, IMongoQuery query, PagerInfo pagerInfo, IMongoSortBy sortBy, params string[] fields)
        {
            var client = new MongoClient(connectionString);
            var server = client.GetServer();

            //获取数据库或者创建数据库（不存在的话）。
            MongoDatabase database = server.GetDatabase(databaseName);

            var result = new List<T>();

            using (server.RequestStart(database))//开始连接数据库。
            {
                MongoCollection<BsonDocument> myCollection = database.GetCollection<BsonDocument>(collectionName);

                MongoCursor<T> myCursor;

                if (null == query)
                {
                    myCursor = myCollection.FindAllAs<T>();
                }
                else
                {
                    myCursor = myCollection.FindAs<T>(query);
                }

                if (null != sortBy)
                {
                    myCursor.SetSortOrder(sortBy);
                }

                if (null != fields)
                {
                    myCursor.SetFields(fields);
                }

                foreach (T entity in myCursor.SetSkip((pagerInfo.Page - 1) * pagerInfo.PageSize).SetLimit(pagerInfo.PageSize))//.SetSkip(100).SetLimit(10)是指读取第一百条后的10条数据。
                {
                    result.Add(entity);
                }
            }
            return result;
        }

        #endregion

        #region 索引
        public static void CreateIndex(string collectionName, params string[] keyNames)
        {
            CreateIndex(ConnectionStringDefault, DatabaseDefault, collectionName, keyNames);
        }

        public static void CreateIndex(string connectionString, string databaseName, string collectionName, params string[] keyNames)
        {
            if (null == keyNames)
            {
                return;
            }
            var client = new MongoClient(connectionString);
            var server = client.GetServer();

            //获取数据库或者创建数据库（不存在的话）。
            MongoDatabase database = server.GetDatabase(databaseName);

            using (server.RequestStart(database))//开始连接数据库。
            {
                MongoCollection<BsonDocument> myCollection = database.GetCollection<BsonDocument>(collectionName);
                if (!myCollection.IndexExists(keyNames))
                {
                    myCollection.EnsureIndex(keyNames);
                }
            }
        }
        #endregion
    }
}
