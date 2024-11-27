using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DBC
{
    public class clsDatabaseCn
    {
        public clsDatabaseCn(){

        }
        private SqlConnection cx = new SqlConnection();

        public SqlConnection Conectar()
        {
            try
            {
                string SqlConnectionString = clsConfiguracion.ObtenerCN("Db");

                if (cx.State == ConnectionState.Open)
                    return cx;
                cx.ConnectionString = clsConfiguracion.ObtenerCN(SqlConnectionString);
                if (cx.State == ConnectionState.Closed)
                    cx.Open();

                return cx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
