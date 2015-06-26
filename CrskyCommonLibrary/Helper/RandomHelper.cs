using System;
using System.Text;

namespace Crsky.Utility.Helper
{
    /// <summary>
    /// 随机数相关
    /// </summary>
    public class RandomHelper
    {
        #region 使用含时间戳的伪随机数生成器生成一个简单的数字字符串，随机随即范围在1-100000内
        /// <summary>
        /// 使用含时间戳的伪随机数生成器生成一个简单的数字字符串，随机随即范围在1-999内
        /// </summary>
        /// <returns>随机生成的简单数字字符串</returns>
        public static string GetRandomString()
        {
            System.Random random = new Random(unchecked((int)DateTime.Now.Ticks));
            StringBuilder rndString = new StringBuilder();
            rndString.Append(DateTime.Now.ToString("yyMMddHHmmssff"));
            rndString.Append(random.Next(1, 999).ToString());
            return rndString.ToString().Substring(2);

            //System.Random random = new Random(unchecked((int)DateTime.Now.Ticks));
            //int i = (int)(random.Next(100000, 999999));
            //return DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Millisecond.ToString() + i.ToString();
        }
        #endregion

        #region 使用含时间戳的伪随机数生成器按指定的数值范围生成一个随机数
        /// <summary>
        /// 使用含时间戳的伪随机数生成器按指定的数值范围生成一个随机数
        /// </summary>
        /// <param name="minValue">要获取的随机数最小值</param>
        /// <param name="maxValue">要获取的随机数最大值</param>
        /// <returns>按指定的数值范围获取一个随机数，返回的值范围包括 minValue 但不包括 maxValue</returns>
        public static int GetRandomInt(int minValue, int maxValue)
        {
            Random random = new Random(unchecked((int)DateTime.Now.Ticks));
            return (int)(random.Next(minValue, maxValue));
        }
        #endregion

        #region 使用含时间戳的伪随机数生成器按指定的最大索引数与数值范围获取n个互不相同随机数组
        /// <summary>
        /// 使用含时间戳的伪随机数生成器按指定的最大索引数与数值范围获取n个互不相同随机数组
        /// </summary>
        /// <param name="upperBound">要获取的随机数组索引上限</param>
        /// <param name="minValue">要获取的随机数最小值</param>
        /// <param name="maxValue">要获取的随机数最大值</param>
        /// <returns>按指定的最大索引数与数值范围获取一个随机数，返回的值范围包括 minValue 但不包括 maxValue</returns>
        public static int[] GetRandomInt(int upperBound, int minValue, int maxValue)
        {
            Random random = new Random(unchecked((int)DateTime.Now.Ticks));
            int[] arrNum = new int[upperBound];
            int tmp = 0;
            for (int i = 0; i <= upperBound - 1; i++)
            {
                //随机取数
                tmp = random.Next(minValue, maxValue);
                //取出值赋到数组中
                arrNum[i] = GetNum(arrNum, tmp, minValue, maxValue, random);
            }
            return arrNum;
        }

        private static int GetNum(int[] arrNum, int tmp, int minValue, int maxValue, Random random)
        {
            int n = 0;
            while (n <= arrNum.Length - 1)
            {
                //利用循环判断是否有重复
                if (arrNum[n] == tmp)
                {
                    //重新随机获取
                    tmp = random.Next(minValue, maxValue);
                    //递归:如果取出来的数字和已取得的数字有重复就重新随机获取
                    GetNum(arrNum, tmp, minValue, maxValue, random);

                }
                n++;
            }
            return tmp;
        }
        #endregion

        #region 使用含时间戳的伪随机数生成器生成一个0.0到1.0的随机小数
        /// <summary>
        /// 使用含时间戳的伪随机数生成器生成一个0.0到1.0的随机小数
        /// </summary>
        public static double GetRandomDouble()
        {
            Random random = new Random(unchecked((int)DateTime.Now.Ticks));
            return random.NextDouble();
        }
        #endregion

        #region 使用含时间戳的伪随机数生成器对一个数组进行随机排序
        /// <summary>
        /// 使用含时间戳的伪随机数生成器对一个数组进行随机排序
        /// </summary>
        /// <typeparam name="T">数组的类型</typeparam>
        /// <param name="arr">需要随机排序的数组</param>
        public static void GetRandomArray<T>(T[] arr)
        {
            Random random = new Random(unchecked((int)DateTime.Now.Ticks));

            //对数组进行随机排序的算法:随机选择两个位置，将两个位置上的值交换

            //交换的次数,这里使用数组的长度作为交换次数
            int count = arr.Length;

            //开始交换
            for (int i = 0; i < count; i++)
            {
                //生成两个随机数位置
                int randomNum1 = GetRandomInt(0, arr.Length);
                int randomNum2 = GetRandomInt(0, arr.Length);

                //定义临时变量
                T temp;

                //交换两个随机数位置的值
                temp = arr[randomNum1];
                arr[randomNum1] = arr[randomNum2];
                arr[randomNum2] = temp;
            }
        }
        #endregion

    }
}
