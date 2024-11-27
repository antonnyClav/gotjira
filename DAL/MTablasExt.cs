using System;
using System.Data;
using System.Data.SqlClient;
using DBC;

namespace DAL
{
    public class MTablasExt
    {
        public void PrecalculoIndicadoresProyecto()
        {
            try
            {
                clsDatabaseCn con = new clsDatabaseCn();
                using (SqlConnection cn = con.Conectar())
                {

                    SqlCommand cmd = new SqlCommand("gj_precalculo_indicadores_proyecto_planf", cn);
                    cmd.CommandTimeout = 900;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.ExecuteNonQuery();

                    cmd.Connection.Close();
                    cmd = null;
                    cn.Close();
                }
                con = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ActualizarRol()
        {
            try
            {
                clsDatabaseCn con = new clsDatabaseCn();
                using (SqlConnection cn = con.Conectar())
                {

                    SqlCommand cmd = new SqlCommand("gj_in_actualiza_rol", cn);
                    cmd.CommandTimeout = 900;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.ExecuteNonQuery();

                    cmd.Connection.Close();
                    cmd = null;
                    cn.Close();
                }
                con = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
