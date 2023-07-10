using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InheriranceProj
{
    //multi level inheritance
    public class Program2
    {
        public void P1()
        {
            Console.WriteLine("Grand Parent method");
        }
    }
    public class Child1: Program2
    {
        public void P2()
        {
            Console.WriteLine("Parent method");
        }
    }
    public class Child2: Child1
    {
        public void P3()
        {
            Console.WriteLine("Child method");
            Console.ReadLine();
        }
    }
}
