namespace OopsProject
{
    public class DynPoly
    {
        public string color = "White";
    }
    public class Dog : DynPoly
    {
        public string color = "Black";
    }
    public class Program
    {
        
        public static void Main(String[] args)
        {
            // StaticPoly sp = new StaticPoly();
            //sp.SPoly(5, 6);
            //sp.SPoly("sai", "nikhil");
            DynPoly dp = new DynPoly();
            Console.Write(dp.color);
            Console.ReadLine();
        }

    }
   
   
}