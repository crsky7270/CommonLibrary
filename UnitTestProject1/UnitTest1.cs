using System;
using System.IO;
using System.Text;
using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Crsky.Utility;
using Crsky.Attributes;
using System.Collections.Generic;
using Crsky.Utility.Helper;
using Crsky.IoC;

namespace UnitTestProject1
{
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
         locator.Load();

         var myTry = ServiceLocator.Current.GetInstance<ITry>();
         myTry.Do();
         myTry.GetSome("12");

      }
   }

   public interface ITry
   {
      void Do();

      string GetSome(string str);
   }

   public class Try : ITry
   {
      public Try()
      {
         
      }

      public void Do()
      {
         Console.WriteLine("Do Some!");
         //throw new NotImplementedException();
      }

      public string GetSome(string str)
      {
         Console.WriteLine("get Some!");
         return "KO";
         //throw new NotImplementedException();
      }
   }

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
