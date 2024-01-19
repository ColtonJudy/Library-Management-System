using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_Management_System
{
    public class LibraryDatabase
    {
        public static string GetConnectionString()
        {
            return "server=localhost;database=lmsdb;user=root;password=1234";
        }
    }
}
