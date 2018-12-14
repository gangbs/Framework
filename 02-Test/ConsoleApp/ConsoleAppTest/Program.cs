using Framework;
using NCalc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppTest
{
    class Program
    {
        static void Main(string[] args)
        {

            //ExpTreeMap(10000);

            //ExpTreeMap(10000);

            //ExpTreeMap(10000);
            int a1 = 4, a2 = 5;
            string exp1 = "[a1]*[Pi]";
            NCalc.Expression e = new NCalc.Expression(exp1);
            e.Parameters["a1"] = a1;
            e.Parameters["a2"] = a2;

            e.EvaluateParameter += delegate (string name, ParameterArgs arg)
            {
                if (name == "Pi")
                    arg.Result = 2;
            };

            var r = e.Evaluate();

            //string exp2 = $"tag(\"tag1\",123)";

            //NCalc.Expression e2 = new NCalc.Expression(exp2);

            //e2.EvaluateFunction += (name, arg) =>
            //{
            //    if(name== "tag")
            //    {
            //       var p= arg.Parameters;
            //    }
            //};

            //e2.Evaluate();

            Console.ReadLine();
        }


        public static void ExpTreeMap(int num)
        {
            Stopwatch sw = new Stopwatch();
            Console.WriteLine($"开始映射：{DateTime.Now.ToString("yyyy-MM-dd")}");
            sw.Start();

            List<Student> lst = new List<Student>();
            for (int i = 0; i < num; i++)
            {
                var s = new Student { Id = i, Age = i, Name = "yyg" + i };
                lst.Add(s);
            }
            var lst2 = ObjectMap<Student, StudentSecond>.MapTo(lst);

            sw.Stop();
            Console.WriteLine($"完成映射，耗时 {sw.ElapsedMilliseconds} 毫秒");
        }

        public static void ReflectMap(int num)
        {
            Stopwatch sw = new Stopwatch();
            Console.WriteLine($"开始映射：{DateTime.Now.ToString("yyyy-MM-dd")}");
            sw.Start();

            List<Student> lst = new List<Student>();
            for (int i = 0; i < num; i++)
            {
                var s = new Student { Id = i, Age = i, Name = "yyg" + i };
                lst.Add(s);
            }
            var lst2 = lst.MapTo<Student, StudentSecond>();

            sw.Stop();
            Console.WriteLine($"完成映射，耗时 {sw.ElapsedMilliseconds} 毫秒");
        }


    }


    





    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public class StudentSecond
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
