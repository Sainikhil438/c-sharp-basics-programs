using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InheriranceProj
{

    //single level inheritance
    public class Program1
    {
        public void Salary()
        {
            Console.WriteLine("Salary from parent");
        }

    }
    public class Child: Program1
    {
        public void Bonus()
        {
            Console.WriteLine("Bonus from child");
        }
    }
}
