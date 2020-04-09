using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AppTasks.Data
{
    public static class Constants
    {
        public const SQLite.SQLiteOpenFlags Flags = SQLite.SQLiteOpenFlags.ReadWrite |
                                                    SQLite.SQLiteOpenFlags.Create |
                                                    SQLite.SQLiteOpenFlags.SharedCache;

        public static string DatabasePath
        {
            get
            {
                string basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine(basePath, "AppTasksSQLite.db3");
            }
        }
    }
}
