using Framework;
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

            ReflectMap(10000);

            ReflectMap(10000);

            ReflectMap(10000);

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
            var lst2 = from item in lst
                       select new StudentSecond().InEntityFrom(item);

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
