using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public class RandomNumCode : VerifyCodeStrategy
    {
        readonly Random r;

        public RandomNumCode()
        {
            this.r = new Random();
        }


        public override string GenCode(int num)
        {
            int maxVal = (int)Math.Pow(10, num);
            int minVal = (int)Math.Pow(10, num) / 10;
            return r.Next(minVal, maxVal).ToString();
        }
    }
}
