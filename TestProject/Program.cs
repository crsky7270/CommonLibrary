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
         var len = CommonHelper.GetLength("中");
         var tmp = CommonHelper.Substring("12", 0);

         var b1 = "a".ToLower() == "b".ToLower();
         var b = CommonHelper.GetIndexInArray("ab", new string[] {"AB", "C"}, true);

         var arr = CommonHelper.Split("a,b,c,d", ",",2);

         var htm = CommonHelper.FormatHtmlTag("<h1> </h1>");
         var html = CommonHelper.UnFormatHtmlTag(htm);
         var text = ConvertHelper.RepairZero("aaa", 5);
      }
   }
}
