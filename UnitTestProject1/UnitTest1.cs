using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Crsky.Utility;
using Crsky.Attributes;
using System.Collections.Generic;
using Crsky.Utility.Helper;

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
         var appendTxt = FileHelper.GetAppendTxtFileBySeperator(testCls);
         File.AppendAllLines(@"c:\test\12.txt", appendTxt, new UTF8Encoding(false));
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
