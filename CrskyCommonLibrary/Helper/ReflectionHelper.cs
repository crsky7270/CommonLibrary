using System;
using System.Collections.Generic;
using System.Reflection;
using System.Collections;
using System.ComponentModel;
using Crsky.Utility.Helper;

namespace RuanYun.Utility.Helper
{
    /// <summary>
    /// 反射操作公共类
    /// </summary>    
    public class ReflectionHelper
    {
        #region 加载程序集
        /// <summary>
        /// 加载程序集
        /// </summary>
        /// <param name="assemblyName">程序集名称,不要加上程序集的后缀，如.dll</param> 
        public static Assembly LoadAssembly(string assemblyName)
        {
            try
            {
                return Assembly.Load(assemblyName);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region 获取程序集中的类型
        /// <summary>
        /// 获取本地程序集中的类型
        /// </summary>
        /// <param name="typeName">类型名称，范例格式："命名空间.类名",类型名称必须在本地程序集中</param>        
        public static Type GetType(string typeName)
        {
            try
            {
                return Type.GetType(typeName);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 获取指定程序集中的类型
        /// </summary>
        /// <param name="assembly">指定的程序集</param>
        /// <param name="typeName">类型名称，范例格式："命名空间.类名",类型名称必须在assembly程序集中</param>
        /// <returns></returns>
        public static Type GetType(Assembly assembly, string typeName)
        {
            try
            {
                return assembly.GetType(typeName);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region 动态创建对象实例

        #region 重载方法一
        /// <summary>
        /// 动态创建对象实例
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="parameters">传递给构造函数的参数</param>        
        public static object CreateInstance(Type type, params object[] parameters)
        {
            try
            {
                //类型为空则返回
                if (ValidationHelper.IsNullOrEmpty(type))
                {
                    return null;
                }

                return Activator.CreateInstance(type, parameters);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region 重载方法二
        /// <summary>
        /// 动态创建对象实例
        /// </summary>
        /// <param name="className">类名，格式:"命名空间.类名"</param>
        /// <param name="parameters">传递给构造函数的参数</param>        
        public static object CreateInstance(string className, params object[] parameters)
        {
            //获取类型
            Type type = GetType(className);

            return CreateInstance(type, parameters);
        }
        #endregion

        #region 重载方法三
        /// <summary>
        /// 创建类的实例
        /// </summary>
        /// <typeparam name="T">转换的目标类型</typeparam>
        /// <param name="type">类型</param>
        /// <param name="parameters">传递给构造函数的参数</param>        
        public static T CreateInstance<T>(Type type, params object[] parameters)
        {
            return ConvertHelper.ConvertTo<T>(CreateInstance(type, parameters));
        }
        #endregion

        #region 重载方法四
        /// <summary>
        /// 创建类的实例
        /// </summary>
        /// <typeparam name="T">转换的目标类型</typeparam>
        /// <param name="className">类名，格式:"命名空间.类名"</param>
        /// <param name="parameters">传递给构造函数的参数</param>        
        public static T CreateInstance<T>(string className, params object[] parameters)
        {
            return ConvertHelper.ConvertTo<T>(CreateInstance(className, parameters));
        }
        #endregion

        #region 重载方法五
        /// <summary>
        /// 动态创建对象实例
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <param name="className">类名，格式:"命名空间.类名"</param>
        /// <param name="parameters">传递给构造函数的参数</param>        
        public static object CreateInstance(Assembly assembly, string className, params object[] parameters)
        {
            //获取类型
            Type type = assembly.GetType(className);

            return CreateInstance(type, parameters);
        }
        #endregion

        #region 重载方法六
        /// <summary>
        /// 动态创建对象实例
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <param name="className">类名，格式:"命名空间.类名"</param>
        /// <param name="parameters">传递给构造函数的参数</param>        
        public static T CreateInstance<T>(Assembly assembly, string className, params object[] parameters)
        {
            //获取类型
            Type type = assembly.GetType(className);

            return CreateInstance<T>(type, parameters);
        }
        #endregion

        #endregion

        #region 创建远程代理对象
        /// <summary>
        /// 创建远程代理对象
        /// </summary>
        /// <param name="type">远程对象的类型</param>
        /// <param name="url">远程对象的URL地址</param>        
        public static object CreateProxy(Type type, string url)
        {
            return Activator.GetObject(type, url);
        }

        /// <summary>
        /// 创建远程代理对象
        /// </summary>
        /// <typeparam name="T">远程对象类</typeparam>
        /// <param name="url">远程对象的URL地址</param>
        public static T CreateProxy<T>(string url)
        {
            //获取远程对象的类型
            Type type = typeof(T);

            return ConvertHelper.ConvertTo<T>(CreateProxy(type, url));
        }
        #endregion

        #region 检测在元素上是否使用了指定特性

        #region 检测成员
        /// <summary>
        /// 检测在元素上是否使用了指定特性
        /// </summary>
        /// <typeparam name="T">特性</typeparam>
        /// <param name="element">成员元素</param>
        public static bool IsDefinedAttribute<T>(MemberInfo element)
        {
            return Attribute.IsDefined(element, typeof(T));
        }
        #endregion

        #endregion

        #region 获取构造函数的自定义特性
        /// <summary>
        /// 获取构造函数的自定义特性
        /// </summary>
        /// <typeparam name="T">目标类型,比如要获取类Class1的构造函数自定义属性Attribute1,
        /// 则传入Class1</typeparam>
        /// <typeparam name="K">获取的自定义特性,比如Attribute1</typeparam>
        /// <param name="types">构造函数的数据类型列表,比如构造函数有两个参数:
        /// string param1,int param2 . 则传入typeof(string),typeof(int)</param>
        public static K GetConstructorAttribute<T, K>(params Type[] types)
        {
            try
            {
                //获取目标类型的反射入口点
                Type type = typeof(T);

                //获取构造函数参数
                Type[] tempTypes;
                //如果types为空，则赋值为Type.EmptyTypes
                if (types.Length == 0)
                {
                    tempTypes = Type.EmptyTypes;
                }
                else
                {
                    tempTypes = types;
                }

                //获取构造函数信息
                ConstructorInfo info = type.GetConstructor(tempTypes);

                //获取自定义特性
                return ConvertHelper.ConvertTo<K>(Attribute.GetCustomAttribute(info, typeof(K)));
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region 获取属性的自定义特性
        /// <summary>
        /// 获取属性的自定义特性
        /// </summary>
        /// <typeparam name="T">目标类型,比如要获取类Class1的构造函数自定义属性Attribute1,
        /// 则传入Class1</typeparam>
        /// <typeparam name="K">获取的自定义特性,比如Attribute1</typeparam>
        /// <param name="propertyName">字符串形式的属性名</param> 
        public static K GetPropertyAttribute<T, K>(string propertyName)
        {
            try
            {
                //获取目标类型的反射入口点
                Type type = typeof(T);

                //获取构造函数信息
                PropertyInfo info = type.GetProperty(propertyName, BindingFlags.Public);

                //获取自定义特性
                return ConvertHelper.ConvertTo<K>(Attribute.GetCustomAttribute(info, typeof(K)));
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region 获取类的命名空间
        /// <summary>
        /// 获取类的命名空间
        /// </summary>
        /// <typeparam name="T">类名或接口名</typeparam>        
        public static string GetNamespace<T>()
        {
            return typeof(T).Namespace;
        }
        #endregion

        #region 设置成员的值

        #region 设置属性值
        /// <summary>
        /// 将值装载到目标对象的指定属性中
        /// </summary>
        /// <param name="target">要装载数据的目标对象</param>
        /// <param name="propertyName">目标对象的属性名</param>
        /// <param name="value">要装载的值</param>
        public static void SetPropertyValue(object target, string propertyName, object value)
        {
            PropertyInfo propertyInfo = target.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            SetValue(target, propertyInfo, value);
        }
        #endregion

        #region 设置成员的值
        /// <summary>
        /// 设置成员的值
        /// </summary>
        /// <param name="target">要装载数据的目标对象</param>
        /// <param name="memberInfo">目标对象的成员</param>
        /// <param name="value">要装载的值</param>
        private static void SetValue(object target, MemberInfo memberInfo, object value)
        {
            if (value != null)
            {
                //获取成员类型
                Type pType;
                if (memberInfo.MemberType == MemberTypes.Property)
                    pType = ((PropertyInfo)memberInfo).PropertyType;
                else
                    pType = ((FieldInfo)memberInfo).FieldType;

                //获取值的类型
                Type vType = GetPropertyType(value.GetType());

                //强制将值转换为属性类型
                value = CoerceValue(pType, vType, value);
            }
            if (memberInfo.MemberType == MemberTypes.Property)
                ((PropertyInfo)memberInfo).SetValue(target, value, null);
            else
                ((FieldInfo)memberInfo).SetValue(target, value);
        }
        #endregion

        #region 强制将值转换为指定类型
        /// <summary>
        /// 强制将值转换为指定类型
        /// </summary>
        /// <param name="propertyType">目标类型</param>
        /// <param name="valueType">值的类型</param>
        /// <param name="value">值</param>
        private static object CoerceValue(Type propertyType, Type valueType, object value)
        {
            //如果值的类型与目标类型相同则直接返回,否则进行转换
            if (propertyType.Equals(valueType))
            {
                return value;
            }
            else
            {
                if (propertyType.IsGenericType)
                {
                    if (propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        if (value == null)
                            return null;
                        else if (valueType.Equals(typeof(string)) && (string)value == string.Empty)
                            return null;
                    }
                    propertyType = GetPropertyType(propertyType);
                }

                if (propertyType.IsEnum && valueType.Equals(typeof(string)))
                    return Enum.Parse(propertyType, value.ToString());

                if (propertyType.IsPrimitive && valueType.Equals(typeof(string)) && string.IsNullOrEmpty((string)value))
                    value = 0;

                try
                {
                    return Convert.ChangeType(value, GetPropertyType(propertyType));
                }
                catch (Exception ex)
                {
                    TypeConverter cnv = TypeDescriptor.GetConverter(GetPropertyType(propertyType));
                    if (cnv != null && cnv.CanConvertFrom(value.GetType()))
                        return cnv.ConvertFrom(value);
                    else
                        throw ex;
                }
            }
        }
        #endregion

        #region 获取类型,如果类型为Nullable<>，则返回Nullable<>的基础类型
        /// <summary>
        /// 获取类型,如果类型为Nullable(of T)，则返回Nullable(of T)的基础类型
        /// </summary>
        /// <param name="propertyType">需要转换的类型</param>
        private static Type GetPropertyType(Type propertyType)
        {
            Type type = propertyType;
            if (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Nullable<>)))
                return Nullable.GetUnderlyingType(type);
            return type;
        }
        #endregion

        #endregion

        #region 数据映射

        #region 从对象进行映射

        #region 将源对象的数据装载到目标对象
        /// <summary>
        /// 将源对象的数据装载到目标对象
        /// </summary>
        /// <param name="source">源对象</param>
        /// <param name="target">目标对象</param>
        public static void Map(object source, object target)
        {
            Map(source, target, false);
        }

        /// <summary>
        /// 将源对象的数据装载到目标对象
        /// </summary>
        /// <param name="source">源对象</param>
        /// <param name="target">目标对象</param>
        /// <param name="ignoreList">被忽略装载的键名列表。如果属性为只读，则不应该装载数据，应把该属性名传入。</param>
        public static void Map(object source, object target, params string[] ignoreList)
        {
            Map(source, target, false, ignoreList);
        }

        /// <summary>
        /// 将源对象的数据装载到目标对象
        /// </summary>
        /// <param name="source">源对象</param>
        /// <param name="target">目标对象</param>
        /// <param name="suppressExceptions">遇到错误,是否抛出异常,默认不抛出。如果要抛出异常，则传入false。</param>
        /// <param name="ignoreList">被忽略装载的键名列表。如果属性为只读，则不应该装载数据，应把该属性名传入。</param>
        public static void Map(object source, object target, bool suppressExceptions, params string[] ignoreList)
        {
            //忽略属性列表
            List<string> ignore = new List<string>(ignoreList);

            //获取源对象所有可浏览属性
            PropertyInfo[] sourceProperties = GetSourceProperties(source.GetType());

            foreach (PropertyInfo sourceProperty in sourceProperties)
            {
                string propertyName = sourceProperty.Name;
                if (!ignore.Contains(propertyName))
                {
                    try
                    {
                        SetPropertyValue(target, propertyName, sourceProperty.GetValue(source, null));
                    }
                    catch (Exception ex)
                    {
                        if (!suppressExceptions)
                        {
                            throw new ArgumentException(String.Format("{0} ({1})", "属性值装载错误", propertyName), ex);
                        }
                    }
                }
            }
        }
        #endregion

        #region 获取类型所有可浏览属性
        /// <summary>
        /// 获取类型所有可浏览属性
        /// </summary>
        /// <param name="sourceType">要获取属性列表的类型</param>
        private static PropertyInfo[] GetSourceProperties(Type sourceType)
        {
            List<PropertyInfo> result = new List<PropertyInfo>();
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(sourceType);
            foreach (PropertyDescriptor item in props)
            {
                if (item.IsBrowsable)
                    result.Add(sourceType.GetProperty(item.Name));
            }
            return result.ToArray();
        }
        #endregion

        #endregion

        #region 从IDictionary进行映射
        /// <summary>
        /// 将字典集合中的值装载到目标对象的指定属性中，属性名为字典中的键
        /// </summary>
        /// <param name="source">字典集合</param>
        /// <param name="target">目标对象</param>
        public static void Map(IDictionary source, object target)
        {
            Map(source, target, false);
        }

        /// <summary>
        /// 将字典集合中的值装载到目标对象的指定属性中，属性名为字典中的键
        /// </summary>
        /// <param name="source">字典集合</param>
        /// <param name="target">目标对象</param>
        /// <param name="ignoreList">被忽略装载的键名列表。如果属性为只读，则不应该装载数据，应把该属性名传入。</param>
        public static void Map(IDictionary source, object target, params string[] ignoreList)
        {
            Map(source, target, false, ignoreList);
        }

        /// <summary>
        /// 将字典集合中的值装载到目标对象的指定属性中，属性名为字典中的键
        /// </summary>
        /// <param name="source">字典集合</param>
        /// <param name="target">目标对象</param>
        /// <param name="suppressExceptions">遇到错误,是否抛出异常,默认不抛出。如果要抛出异常，则传入false。</param>
        /// <param name="ignoreList">被忽略装载的键名列表。如果属性为只读，则不应该装载数据，应把该属性名传入。</param>
        public static void Map(IDictionary source, object target, bool suppressExceptions, params string[] ignoreList)
        {
            List<string> ignore = new List<string>(ignoreList);

            foreach (string propertyName in source.Keys)
            {
                if (!ignore.Contains(propertyName))
                {
                    try
                    {
                        SetPropertyValue(target, propertyName, source[propertyName]);
                    }
                    catch (Exception ex)
                    {
                        if (!suppressExceptions)
                        {
                            throw new ArgumentException(String.Format("{0} ({1})", "属性值装载错误", propertyName), ex);
                        }
                    }
                }
            }
        }
        #endregion

        #endregion

        #region 执行方法

        #region 重载方法1
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="type">类型，在该类型上执行方法。该重载方法只能执行
        /// 构造函数不带参数的方法</param>
        /// <param name="methodName">调用的方法名</param>
        /// <param name="parameters">为调用方法传递的参数</param>
        public static T InvokeMethod<T>(Type type, string methodName, params object[] parameters)
        {
            //创建实例
            object instance = CreateInstance(type);

            return InvokeMethod<T>(instance, methodName, BindingFlags.InvokeMethod, parameters);
        }
        #endregion

        #region 重载方法2
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="instance">实例，在该实例上执行方法。</param>
        /// <param name="methodName">调用的方法名</param>
        /// <param name="parameters">调用的参数</param>
        public static T InvokeMethod<T>(object instance, string methodName, params object[] parameters)
        {
            return InvokeMethod<T>(instance, methodName, BindingFlags.InvokeMethod, parameters);
        }
        #endregion

        #region 重载方法3
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="instance">实例，在该实例上执行方法。</param>
        /// <param name="methodName">调用的方法名</param>
        /// <param name="flag">搜索标志</param>
        /// <param name="parameters">调用的参数</param>
        public static T InvokeMethod<T>(object instance, string methodName, BindingFlags flag, params object[] parameters)
        {
            //创建返回值
            object result;

            //获取类型
            Type type = instance.GetType();

            if (parameters.Length == 0)
            {
                result = type.InvokeMember(methodName, flag, null, instance, new object[] { });
            }
            else
            {
                result = type.InvokeMember(methodName, flag, null, instance, parameters);
            }

            //返回值
            return ConvertHelper.ConvertTo<T>(result);
        }
        #endregion

        #endregion
    }
}
