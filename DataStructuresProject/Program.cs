namespace DataStructuresProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            LinkedList ls = new LinkedList();
            ls.Add(55);
            ls.Add(35);
            ls.Add(75);
            ls.Display();

            //To remove First Node from linked list
            ls.RemoveFirstNode();
            ls.Display();

            //To remove Last Node from Linked List
            ls.RemoveLastNode();
            ls.Display();
        }
    }
}