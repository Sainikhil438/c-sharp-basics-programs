using InheriranceProj;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InheriranceProj
{
    interface IShape
    {
        double GetArea();
    }
    interface IColor
    {
        string GetColor();
    }
    public class Program3: IShape: IColor
    {
        private double l;
    private double b;
    private string c;
      public Program3(double l, double b, string c)
    {
        this.l = l;
        this.b = b;
        this.c = c;
    }
    public double GetArea()
    {
        return l * b;
    }
    public string GetColor()
    {
        return c;
    }
    }
   class Programs
{
    public static void Main(string[] args)
    {
        Program3 pp = new Program3(10, 5, "Black");
        Console.WriteLine("Area of Rectangle: " + pp.GetArea());
        Console.WriteLine("Color: " + pp.GetColor());
        Console.ReadLine();
    }
}
}
