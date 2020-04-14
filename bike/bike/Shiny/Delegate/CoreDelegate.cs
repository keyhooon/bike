using Shiny.Notifications;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace bike.Shiny.Delegate
{
    public class CoreDelegateServices
    {
        public CoreDelegateServices(SqliteConnection conn)
        {
            Connection = conn;
        }


        public SqliteConnection Connection { get; }



    }
}
