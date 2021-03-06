﻿using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Crsky.Caching.CouchBase;
using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Crsky.Utility;
using Crsky.Attributes;
using System.Collections.Generic;
using Crsky.Utility.Helper;
using Crsky.IoC;
using Crsky.Caching;
namespace UnitTestProject1
{
   /// <summary>
   /// Test Class OK
   /// </summary>
   [TestClass]
   public class UnitTest1
   {
      [TestMethod]
      public void TestAppendTxtFileBySeperator()
      {
         List<TestClass> testCls = new List<TestClass>();
         testCls.Add(new TestClass() { PropA = "A", PropB = 2, PropC = 3 });
         var appendTxt = ConvertHelper.GetAppendTxtFileBySeperator(testCls);
         File.AppendAllLines(@"c:\test\12.txt", appendTxt, new UTF8Encoding(false));
      }

      [TestMethod]
      public void TestCreateTxtFile()
      {
         FileHelper.CreateTxtFile(@"C:\test2\1.txt");
      }

      [TestMethod]
      public void TestIOC()
      {
         var locator = new StructureMapServiceLocator();
         locator.UseAsDefault();
         locator.Map(() => ServiceLocator.Current);
         locator.Map<ITry, Try>();
         locator.Map<ITryBase, TryBase>();
         locator.Load();

         var myTry = ServiceLocator.Current.GetInstance<ITry>();
         var str = "Hello world!";
         var result = myTry.DoSome(str);
         myTry.GetSome("12");
      }

      [TestMethod]
      public void TestNoIOC()
      {
         TryBase tryBase = new TryBase();
         Try myTry = new Try(tryBase);
         var str = myTry.DoSome("hello");
      }



      [TestMethod]
      public void TestCouchbase()
      {
         var testClass = new TestClass() { PropA = "1", PropB = 1, PropC = 1.2 };
         var testClass1 = new TestClass() { PropA = "2", PropB = 2, PropC = 2.2 };
         var obj = new List<TestClass>();
         obj.Add(testClass);
         obj.Add(testClass1);

         CouchbaseManager.ResetCouchClientBySectionName("couchbase");
         CouchbaseManager.Add<List<TestClass>>("key11", obj);

         var dd = CouchbaseManager.Get<List<TestClass>>("key11");

         //CouchbaseManager.SectionName = "bucketV2";
         CouchbaseManager.ResetCouchClientBySectionName("bucketV2");
         CouchbaseManager.Add<List<TestClass>>("key12", obj);
      }
   }

   public interface ITry
   {
      string DoSome(string str);

      string GetSome(string str);
   }

   public class Try : ITry
   {
      public readonly ITryBase TryBase;
      public Try(ITryBase tryBase)
      {
         TryBase = tryBase;
      }

      public string DoSome(string str)
      {
         return TryBase.DoSome(str) + "\r\n" + string.Format("Try Say:{0}", str);
         //Console.WriteLine("Do Some!");
         //throw new NotImplementedException();
      }

      public string GetSome(string str)
      {
         Console.WriteLine("get Some!");
         return "KO";
         //throw new NotImplementedException();
      }
   }

   public interface ITryBase
   {
      string DoSome(string str);
   }

   public class TryBase : ITryBase
   {
      public string DoSome(string str)
      {
         return "TryBase Say:" + str;
      }
   }

   [Serializable]
   public class TestClass
   {
      [TextOutPutAttribute(2)]
      public string PropA { get; set; }

      [TextOutPutAttribute(1)]
      public float PropB { get; set; }

      [TextOutPutAttribute(3)]
      public double PropC { get; set; }

   }
}
