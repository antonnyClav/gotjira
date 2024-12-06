using System;
using System.Data;
using System.Data.SqlClient;
using DBC;

namespace DAL
{
    public class MParametros
    {
        public string ObtenerParametro(string strCodigo)
        {
            try
            {
                string strValor = "";
                clsDatabaseCn con = new clsDatabaseCn();
                using (SqlConnection cn = con.Conectar())
                {

                    SqlCommand cmd = new SqlCommand("gj_parametros_ret", cn);
                    cmd.CommandTimeout = 1800;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@v_codigo", strCodigo);

                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        strValor = dr["par_valor"].ToString();
                    }

                    cmd.Connection.Close();
                    cmd = null;
                    cn.Close();
                }
                con = null;

                return strValor;
                
                
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public void SetearParametro(string strCodigo, string strValor)
        {
            try
            {
                clsDatabaseCn con = new clsDatabaseCn();
                using (SqlConnection cn = con.Conectar())
                {
                    SqlCommand cmd = new SqlCommand("gj_parametros_upd", cn);
                    cmd.CommandTimeout = 1800;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@v_codigo", strCodigo);
                    cmd.Parameters.AddWithValue("@v_valor", strValor);

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
        public string ObtenerParametroFromFile(string strCodigo)
        {
            try
            {
                string strValor = "";

                strValor = clsConfiguracion.ObtenerCN(strCodigo);

                return strValor;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DateTime ObtenerFechaServerDb()
        {
            DateTime fecha_return= new DateTime();
            try
            {
                clsDatabaseCn con = new clsDatabaseCn();
                using (SqlConnection cn = con.Conectar())
                {
                    SqlCommand cmd = new SqlCommand("SELECT SYSDATETIMEOFFSET() AT TIME ZONE 'UTC' AT TIME ZONE 'Argentina Standard Time' as fecha_actual", cn);
                    cmd.CommandTimeout = 1800;
                    cmd.CommandType = CommandType.Text;
                                        
                    System.Data.SqlClient.SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read()) {
                        fecha_return = DateTime.Parse(dr["fecha_actual"].ToString());
                    }

                    dr.Close();
                    dr = null;
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
            return fecha_return;
        }
    }
}
