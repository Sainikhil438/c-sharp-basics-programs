namespace AbstractionProject
{
    abstract class Parent
    {
        public abstract void P1();
        public void P2()
        {
            Console.WriteLine("normal method");
        }
    }
    class Child: Parent
    {
        public override void P1()
        {
            Console.WriteLine("Overrided method");
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Child cc = new Child();
            cc.P1();
            cc.P2();
            Console.ReadLine();
        }
    }
}