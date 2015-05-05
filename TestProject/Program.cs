using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crsky.Utility;
using Crsky.Utility.Helper;

namespace TestProject
{
   class Program
   {
      static void Main(string[] args)
      {
         //FileHelper.CreateTxtFile(@"c:\test\12.txt");

         //var len = CommonHelper.GetLength("中");
         //var tmp = CommonHelper.Substring("12", 0);

         //var b1 = "a".ToLower() == "b".ToLower();
         //var b = CommonHelper.GetIndexInArray("ab", new string[] {"AB", "C"}, true);

         //var newlist = new List<string>();


         //var arr = CommonHelper.Split("a,b,c,d", ",",2);

         //var htm = CommonHelper.FormatHtmlTag("<h1> </h1>");
         //var html = CommonHelper.UnFormatHtmlTag(htm);
         //var text = ConvertHelper.RepairZero("aaa", 5);

         List<TestClass> testCls = new List<TestClass>();
         testCls.Add(new TestClass() { PropA = "A", PropB = 2, PropC = 3 });
         ConvertHelper.GetAppendTxtFileBySeperator(testCls);
      }
   }

   public class TestClass
   {
      [TxtOutPut(2)]
      public string PropA { get; set; }

      [TxtOutPut(1)]
      public float PropB { get; set; }

      [TxtOutPut(3)]
      public double PropC { get; set; }

   }


   public class TxtOutPutAttribute : Attribute
   {
      //文本输出顺序
      int orderIndex = 0;
      public TxtOutPutAttribute(int orderIndex)
      {
         this.orderIndex = orderIndex;
      }
   }
}
