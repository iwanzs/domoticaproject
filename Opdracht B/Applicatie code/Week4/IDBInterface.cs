using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Week4
{
    public interface IDBInterface
    {
        SQLiteConnection CreateConnection();
    }
}
