namespace StudentRecordManagementSystem.Utility
{
    public class ConnectionString
    {
        //private static string cName = @"Data Source=DESKTOP-8HRS1TJ\SQLEXPRESS; Initial Catalog=StudentManagement; Integrated Security = True;";

        private static string cName = @"Server=tcp:mvc-sqlserver.database.windows.net,1433;Initial Catalog=SqlMvcDatabase;Persist Security Info=False;User ID=SaiMvcServer;Password=SSai@@99;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        public static string CName   
        {
            get => cName;
        }
    }
    
}
