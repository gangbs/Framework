using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class QueryCompareAttribute : Attribute
    {
        public readonly CompareEnum compare;
        public int Order { get; set; }
        public string FieldName { get; set; }

        public QueryCompareAttribute(CompareEnum compare)
        {
            this.compare = compare;
        }
    }
}
