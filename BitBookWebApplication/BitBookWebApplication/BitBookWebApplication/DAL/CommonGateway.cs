using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace BitBookWebApplication.App_Start.DAL
{
    public class CommonGateway
    {
        private string connectionString = WebConfigurationManager.ConnectionStrings["BitBookDB"].ConnectionString;
        private SqlConnection connection;
        private SqlCommand command;
        private SqlDataReader reader;
        private DataSet dataSet;
        private SqlDataAdapter dataAdapter;

        public SqlConnection Connection
        {
            get { return connection; }
        }

        public SqlCommand Command
        {
            get { return command; }
            set { command = value; }
        }

        public SqlDataReader Reader
        {
            get { return reader; }
            set { reader = value; }
        }

        public DataSet DataSet
        {
            get { return dataSet; }
            set { dataSet = value; }
        }

        public SqlDataAdapter DataAdapter
        {
            get { return dataAdapter; }
            set { dataAdapter = value; }
        }

        public CommonGateway()
        {
            GenarateConnection();
        }

        public void GenarateConnection()
        {
            connection = new SqlConnection(connectionString);
        }
    }
}