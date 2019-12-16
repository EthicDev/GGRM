using GGRMLib.DataAccess;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GGRMLib
{

    //Coded by: Macklem Curtis
    //Date: Nov/Dec 2019

    public static class GlobalConfig
    {
        public static SqlConnector Connection { get; private set; }

        public static void InitializeConnections()
        {
            SqlConnector sql = new SqlConnector();
            Connection = sql;
        }

        public static string ConString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}
