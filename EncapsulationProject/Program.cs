namespace EncapsulationProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Student st = new Student();

            //setting values to properties
            st.ID = "A1B2";
            st.Name = "Sai";
            st.Mail = "sai111@gmail.com";

            //getting values 
            Console.WriteLine(st.ID);
            Console.WriteLine(st.Name);
            Console.WriteLine(st.Mail);

            Console.ReadLine();
        }
    }
}