using NCalc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.ExpCal
{
   public class ExpCalcBase
    {
        protected Dictionary<string, object> parameter { get; set; }

        protected EvaluateFunctionHandler evaluateFunctionHandler { get; set; }

        //protected  virtual object GetParameterValue(string parameterName)
        //{
        //    return th
        //}

        public virtual object Evaluate(string exp)
        {
            var e = new Expression(exp);
            if(this.parameter!=null)
            {
                e.Parameters = this.parameter;
            }
           
            if(evaluateFunctionHandler!=null)
            {
                e.EvaluateFunction += this.evaluateFunctionHandler;
            }          
            return e.Evaluate();
        }
    }
}
