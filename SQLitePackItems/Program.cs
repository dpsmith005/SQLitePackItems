using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.IO;
using System.Data;
using System.Data.SQLite;

namespace SQLitePackItems
{
    internal static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string DatabaseFile = ConfigurationManager.AppSettings["DatabaseFile"];

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //if (!File.Exists(@"..\..\Files\SQLitePack.db"))
            if (!File.Exists(DatabaseFile))
            { 
               DatabaseHelper.InitializeDatabase();
            }

            Application.Run(new MainForm());
        }
    }
}
