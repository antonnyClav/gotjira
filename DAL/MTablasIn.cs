using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using DAL.Model;
using DBC;

namespace DAL
{
    public class MTablasIn
    {
        /* GOTJIRA */

        public void DeleteWklHor(long Id)
        {
            try
            {
                clsDatabaseCn con = new clsDatabaseCn();
                using (SqlConnection cn = con.Conectar())
                {
                    SqlCommand cmd = new SqlCommand("gj_wkl_hor_delete", cn);
                    cmd.CommandTimeout = 1800;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@v_id", Id);

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

        public void TruncateTabla(string tableName)
        {
            try
            {
                // Validar nombre de tabla: letras, números, guiones bajos, punto (para esquema.tabla)
                if (!Regex.IsMatch(tableName, @"^[a-zA-Z0-9_\.]+$"))
                {
                    throw new ArgumentException("Nombre de tabla inválido.");
                }

                clsDatabaseCn con = new clsDatabaseCn();
                using (SqlConnection cn = con.Conectar())
                {

                    string query = $"TRUNCATE TABLE {tableName}";

                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        cmd.CommandTimeout = 1800;
                        cmd.ExecuteNonQuery();
                    }

                    cn.Close();
                }

                con = null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al truncar la tabla {tableName}: " + ex.Message, ex);
            }
        }


        public void LimpiarTablasIN()
        {
            try
            {
                clsDatabaseCn con = new clsDatabaseCn();
                using (SqlConnection cn = con.Conectar())
                {
                    SqlCommand cmd = new SqlCommand("gj_truncate_tablas_in", cn);
                    cmd.CommandTimeout = 1800;
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
        public string Grabar_InTimeSheet(DateTime Desde, DateTime Hasta)
        {
            try
            {
                string strMensaje = "";
                clsDatabaseCn con = new clsDatabaseCn();                
                using (SqlConnection cn = con.Conectar())
                {                    
                    SqlCommand cmd = new SqlCommand("gj_in_timesheet_insert", cn);
                    cmd.CommandTimeout = 1800;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@v_desde", Desde);
                    cmd.Parameters.AddWithValue("@v_hasta", Hasta);

                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        strMensaje = dr["mensaje"].ToString();
                    }

                    cmd.Connection.Close();
                    cmd = null;
                    cn.Close();
                }
                con = null;
                return strMensaje;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }        
        public void Timesheet_Parse_In()
        {
            try
            {
                clsDatabaseCn con = new clsDatabaseCn();
                using (SqlConnection cn = con.Conectar())
                {
                    SqlCommand cmd = new SqlCommand("gj_in_timesheet_parse_in", cn);
                    cmd.CommandTimeout = 1800;
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
        public int Grabar_InProject()
        {
            try
            {
                int iCantidad = 0;
                clsDatabaseCn con = new clsDatabaseCn();
                using (SqlConnection cn = con.Conectar())
                {                    
                    SqlCommand cmd = new SqlCommand("gj_in_proyectos_insert", cn);
                    cmd.CommandTimeout = 1800;
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        iCantidad = int.Parse(dr["cantidad"].ToString());
                    }

                    cmd.Connection.Close();
                    cmd = null;
                    cn.Close();
                }
                con = null;
                return iCantidad;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public int Grabar_InJiras(DateTime Desde, DateTime Hasta)
        {
            try
            {
                int iCantidad = 0;
                clsDatabaseCn con = new clsDatabaseCn();
                using (SqlConnection cn = con.Conectar())
                {                    
                    SqlCommand cmd = new SqlCommand("gj_in_jiras_insert", cn);
                    cmd.CommandTimeout = 1800;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@v_desde", Desde);
                    cmd.Parameters.AddWithValue("@v_hasta", Hasta);

                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        iCantidad = int.Parse(dr["cantidad"].ToString());
                    }

                    cmd.Connection.Close();
                    cmd = null;
                    cn.Close();
                }
                con = null;
                return iCantidad;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public void Grabar_InEnlaces()
        {
            try
            {
                clsDatabaseCn con = new clsDatabaseCn();
                using (SqlConnection cn = con.Conectar())
                {

                    SqlCommand cmd = new SqlCommand("gj_in_enlaces_insert", cn);
                    cmd.CommandTimeout = 1800;
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
        public void Grabar_InUsuariosJira()
        {
            try
            {
                clsDatabaseCn con = new clsDatabaseCn();
                using (SqlConnection cn = con.Conectar())
                {

                    SqlCommand cmd = new SqlCommand("gj_in_usuarios_insert", cn);
                    cmd.CommandTimeout = 1800;
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
        public void Grabar_InProyectosPorComponentes()
        {
            try
            {
                clsDatabaseCn con = new clsDatabaseCn();
                using (SqlConnection cn = con.Conectar())
                {

                    SqlCommand cmd = new SqlCommand("gj_in_proyectos_x_componentes_insert", cn);
                    cmd.CommandTimeout = 1800;
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
        public string Obtener_JirasKeyActualizar(bool v_ultimos)
        {
            string strValor = "";
            try
            {                
                clsDatabaseCn con = new clsDatabaseCn();
                using (SqlConnection cn = con.Conectar())
                {

                    SqlCommand cmd = new SqlCommand("gj_jiras_actualizar", cn);
                    cmd.CommandTimeout = 1800;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@v_ultimos", (v_ultimos? 1: 0));

                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        strValor = dr["jiras"].ToString();
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
        public string Obtener_JirasKeyEjecucionAnterior()
        {
            string strValor = "";
            try
            {
                clsDatabaseCn con = new clsDatabaseCn();
                using (SqlConnection cn = con.Conectar())
                {

                    SqlCommand cmd = new SqlCommand("gj_jiras_key_ejecucion_anterior", cn);
                    cmd.CommandTimeout = 1800;
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        strValor = dr["jiras"].ToString();
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
        public string Obtener_EpicasActualizar()
        {
            string strValor = "";
            try
            {
                clsDatabaseCn con = new clsDatabaseCn();
                using (SqlConnection cn = con.Conectar())
                {

                    SqlCommand cmd = new SqlCommand("gj_epicas_actualizar", cn);
                    cmd.CommandTimeout = 1800;
                    cmd.CommandType = CommandType.StoredProcedure;                    

                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        strValor = dr["jiras"].ToString();
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
        public string Obtener_JirasAEnlazar()
        {
            string strValor = "";
            try
            {
                clsDatabaseCn con = new clsDatabaseCn();
                using (SqlConnection cn = con.Conectar())
                {

                    SqlCommand cmd = new SqlCommand("gj_jiras_a_enlazar", cn);
                    cmd.CommandTimeout = 1800;
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        strValor = dr["jiras"].ToString();
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

        /* SUGAR */
        public void LimpiarTablasIn()
        {
            try
            {
                clsDatabaseCn con = new clsDatabaseCn();
                using (SqlConnection cn = con.Conectar())
                {
                    SqlCommand cmd = new SqlCommand("sg_truncate_tablas_in", cn);
                    cmd.CommandTimeout = 1800;
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
        public void Grabar_InUsuarios(Usuarios usuario)
        {
            try
            {
                clsDatabaseCn con = new clsDatabaseCn();
                using (SqlConnection cn = con.Conectar())
                {
                    SqlCommand cmd = new SqlCommand("sg_in_usuarios_insert", cn);
                    cmd.CommandTimeout = 1800;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@sugar_id", usuario.SugarId);
                    cmd.Parameters.AddWithValue("@nombre", usuario.Nombre);
                    cmd.Parameters.AddWithValue("@departamento", usuario.Departamento);
                    cmd.Parameters.AddWithValue("@titulo", usuario.Titulo);
                    cmd.Parameters.AddWithValue("@informa_a", usuario.InformaA);
                    cmd.Parameters.AddWithValue("@mail", usuario.Mail);
                    cmd.Parameters.AddWithValue("@telefono", usuario.Telefono);
                    cmd.Parameters.AddWithValue("@estado", usuario.Estado);
                    cmd.Parameters.AddWithValue("@fecha_creacion", usuario.FechaCreacion);
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
        public void Grabar_InCuentas(Cuentas cuenta)
        {
            try
            {
                clsDatabaseCn con = new clsDatabaseCn();
                using (SqlConnection cn = con.Conectar())
                {
                    SqlCommand cmd = new SqlCommand("sg_in_cuentas_insert", cn);
                    cmd.CommandTimeout = 1800;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@sugar_id", cuenta.SugarId);
                    cmd.Parameters.AddWithValue("@nombre", cuenta.Nombre);
                    cmd.Parameters.AddWithValue("@industria", cuenta.Industria);
                    cmd.Parameters.AddWithValue("@ciudad", cuenta.Ciudad);
                    cmd.Parameters.AddWithValue("@telefono", cuenta.Telefono);
                    cmd.Parameters.AddWithValue("@vertical", cuenta.Vertical);
                    cmd.Parameters.AddWithValue("@version_software", cuenta.VersionSoftware);
                    cmd.Parameters.AddWithValue("@usuario", cuenta.Usuario);
                    cmd.Parameters.AddWithValue("@pais_emision_factura", cuenta.PaisEmisionFactura);
                    cmd.Parameters.AddWithValue("@alcance_de_cuenta", cuenta.AlcanceDeCuenta);

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
        public void Grabar_InTareas(Tareas tarea)
        {
            try
            {
                clsDatabaseCn con = new clsDatabaseCn();
                using (SqlConnection cn = con.Conectar())
                {
                    SqlCommand cmd = new SqlCommand("sg_in_tareas_insert", cn);
                    cmd.CommandTimeout = 1800;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@sugar_id", tarea.SugarId);
                    cmd.Parameters.AddWithValue("@cuenta", tarea.Cuenta);
                    cmd.Parameters.AddWithValue("@oportunidad", tarea.Oportunidad);
                    cmd.Parameters.AddWithValue("@tipo", tarea.Tipo);
                    cmd.Parameters.AddWithValue("@fecha_vencimiento", tarea.FechaVencimiento);
                    cmd.Parameters.AddWithValue("@estado", tarea.Estado);                    
                    cmd.Parameters.AddWithValue("@usuario", tarea.Usuario);
                    cmd.Parameters.AddWithValue("@fecha_creacion", tarea.FechaCreacion);
                    cmd.Parameters.AddWithValue("@asunto", tarea.Asunto);
                    cmd.Parameters.AddWithValue("@experto_asginado", tarea.ExpertoAsignado);
                    cmd.Parameters.AddWithValue("@prioridad", tarea.Prioridad);
                    cmd.Parameters.AddWithValue("@ultima_modificacion", tarea.UltimaModificacion);

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
        public void Grabar_InAutorizaciones(Autorizaciones autorizacion)
        {
            try
            {
                clsDatabaseCn con = new clsDatabaseCn();
                using (SqlConnection cn = con.Conectar())
                {
                    SqlCommand cmd = new SqlCommand("sg_in_autorizaciones_insert", cn);
                    cmd.CommandTimeout = 1800;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@sugar_id", autorizacion.SugarId);
                    cmd.Parameters.AddWithValue("@fecha_creacion", autorizacion.FechaCreacion);
                    cmd.Parameters.AddWithValue("@oportunidad", autorizacion.Oportunidad);
                    cmd.Parameters.AddWithValue("@estado", autorizacion.Estado);
                    cmd.Parameters.AddWithValue("@usuario", autorizacion.Usuario);
                    cmd.Parameters.AddWithValue("@aprobacion", autorizacion.Aprobacion);
                    cmd.Parameters.AddWithValue("@razones", autorizacion.Razones);
                    cmd.Parameters.AddWithValue("@vigente", autorizacion.Vigente);
                    cmd.Parameters.AddWithValue("@ultima_modificacion", autorizacion.UltimaModificacion);
                    cmd.Parameters.AddWithValue("@aprobacion_obligatoria", autorizacion.AprobacionObligatoria);

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
        public void Grabar_InOportunidades(Oportunidades oportunidad)
        {
            try
            {
                clsDatabaseCn con = new clsDatabaseCn();
                using (SqlConnection cn = con.Conectar())
                {
                    SqlCommand cmd = new SqlCommand("sg_in_oportunidades_insert", cn);
                    cmd.CommandTimeout = 1800;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@sugar_id", oportunidad.SugarId);
                    cmd.Parameters.AddWithValue("@nro", oportunidad.Numero);
                    cmd.Parameters.AddWithValue("@nombre", oportunidad.Nombre);
                    cmd.Parameters.AddWithValue("@cuenta", oportunidad.Cuenta);
                    cmd.Parameters.AddWithValue("@etapa_venta", oportunidad.EtapaVenta);
                    cmd.Parameters.AddWithValue("@monto", oportunidad.Monto);
                    cmd.Parameters.AddWithValue("@fecha_cierre", oportunidad.FechaCierre);
                    cmd.Parameters.AddWithValue("@usuario", oportunidad.Usuario);
                    cmd.Parameters.AddWithValue("@previsto_probable", oportunidad.PrevistoProbable);
                    cmd.Parameters.AddWithValue("@perdido", oportunidad.Perdido);
                    cmd.Parameters.AddWithValue("@direccion_comercial", oportunidad.DireccionComercial);
                    cmd.Parameters.AddWithValue("@tipo", oportunidad.Tipo);
                    cmd.Parameters.AddWithValue("@fecha_inicio", oportunidad.FechaInicio);
                    cmd.Parameters.AddWithValue("@fecha_renovacion", oportunidad.FechaRenovacion);
                    cmd.Parameters.AddWithValue("@ultima_modificacion", oportunidad.UltimaModificacion);
                    cmd.Parameters.AddWithValue("@cantidad_convertida", oportunidad.CantidadConvertida);

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
        public void CargarTablasSugar()
        {
            try
            {
                clsDatabaseCn con = new clsDatabaseCn();
                using (SqlConnection cn = con.Conectar())
                {
                    SqlCommand cmd = new SqlCommand("sg_carga_masiva_sugar", cn);
                    cmd.CommandTimeout = 1800;
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
