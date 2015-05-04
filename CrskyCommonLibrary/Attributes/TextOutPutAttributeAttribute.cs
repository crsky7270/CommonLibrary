using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crsky.Attributes
{
   public class TextOutPutAttributeAttribute : Attribute
   {
      //文本输出顺序
      public TextOutPutAttributeAttribute(int orderIndex)
      {
         this.OrderIndex = orderIndex;
      }

      public int OrderIndex { get; private set; }
   }
}
