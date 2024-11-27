using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using DAL;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http;
using System.Configuration;

namespace Sugar
{
    public class EmxSugar
    {
        private string baseUrl = "https://softoffice.sugarondemand.com";

        private string VersionApi = "v11_23";

        private string Token = "";
        
        private string MaxNum = "100";

        //HORA DEL SERVIDOR
        public DateTime FechaServer {
            get
            {
                MParametros objParametro = new MParametros();
                try
                {
                    return objParametro.ObtenerFechaServerDb();
                }
                catch {
                    return DateTime.Now;
                }
                finally
                {
                    objParametro = null;
                }
            }
        }
        
        // INTERVALO DE HORAS EN LA QUE SE EJECUTA EL SERVICIO
        public int IntervaloExec
        {

            get
            {
                MParametros objParametro = new MParametros();
                try
                {
                    return int.Parse(objParametro.ObtenerParametro("Interv_Exec_ServiceSugar"));
                }
                catch {
                    return 4;
                }
                finally {
                    objParametro = null;
                }
            }
        }

        // HORA QUE ARRANCA EL SERVICIO
        private int HoraDesde
        {
            get
            {
                MParametros objParametro = new MParametros();
                try
                {
                    return int.Parse(objParametro.ObtenerParametro("HoraSincDesdeSugar"));
                }
                catch
                {
                    return 1;
                }
                finally
                {
                    objParametro = null;
                }
            }
        }

        // HORA EN QUE FINALIZA EL SERVICIO
        private int HoraHasta
        {
            get
            {
                MParametros objParametro = new MParametros();
                try
                {
                    return int.Parse(objParametro.ObtenerParametro("HoraSincHastaSugar"));
                }
                catch
                {
                    return 22;
                }
                finally
                {
                    objParametro = null;
                }
            }
        }

        // HORARIO VALIDO, SI ES FALSE TODOS LOS METODOS SALEN POR RETURN
        public bool HorarioValido
        {
            // SABER SI ES HORARIO VALIDO PARA EJECUTAR EL SERVICIO DE LUNES A SABADOS
            get
            {
                bool retHorarioValido = false;

                DateTime datetime = DateTime.Now;

                int HoraDesde = this.HoraDesde;
                int HoraHasta = this.HoraHasta;

                int horaActual = datetime.Hour;
                int diaActual = (int)datetime.DayOfWeek;

                retHorarioValido =  (horaActual >= HoraDesde && horaActual <= HoraHasta && diaActual > 0 && diaActual <= 5); /* esta en (6), antes corria los sabados, pero ahora se apagan los servers, todo lo que era sabado se paso a viernes */

                if (!retHorarioValido) {
                    GrabarProximaEjecucion();
                }

                return retHorarioValido;
                // quitar
                // return true;
            }
        }
                
        public EmxSugar(bool _GetToken)
        {            
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;                                              

                if(_GetToken) Token = GetToken();                       
            }
            catch (Exception ex)
            {
                Utilidades.LogService("Error Sugar: " + ex.Message);
            }                                    
        }

        private string GetToken()
        {
            if (!this.HorarioValido)
            {
                return "HORARIO_INVALIDO";
            }

            string Token = "";
            try
            {
                //MParametros objParametro = new MParametros();

                string User = "ycenturion"; //objParametro.ObtenerParametro("UserJira");
                string Password = "Emerix2022"; // objParametro.ObtenerParametro("UserJira");                               

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                WebRequest request = WebRequest.Create(this.baseUrl + "/rest/"+ VersionApi +"/oauth2/token");                
                string bodyData = $@"
                {{
                    ""grant_type"": ""password"",
                    ""client_id"": ""sugar"",
                    ""client_secret"": """",
                    ""username"": ""{User}"",
                    ""password"": ""{Password}"",
                    ""platform"": ""custom""
                }}
                ";                

                //Utilidades.LogService("GetToken() URL: " + bodyData);

                byte[] byteArray = Encoding.UTF8.GetBytes(bodyData);
                request.Method = "POST";
                request.Headers.Add("Cache-Control", "no-cache");
                request.ContentType = "application/json";
                request.ContentLength = byteArray.Length;

                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                WebResponse response = request.GetResponse();
                dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                String responseFromServer = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
                response.Close();

                //Console.WriteLine("\nloginResopnse:");
                //Console.WriteLine(responseFromServer);

                var result = responseFromServer;
                //Console.WriteLine(result);
                var obj = JsonConvert.DeserializeObject<Token>(result);

                //return obj;

                //objParametro = null;

                Token =  obj.access_token;

            }
            catch (Exception F)
            {

                Task.Run(() => Utilidades.LogService("Error GetToken(): " + F.Message));

                F.Data.Clear();
            }

            return Token;
        }

        public void LimpiarTablasIn()
        {
            MTablasIn objTablasIn = new MTablasIn();
            try
            {
                Utilidades.LogService("--------------------------------------------------------------------------------------------");
                Utilidades.LogService("INICIO SUGAR");
                objTablasIn.LimpiarTablasIn();
            }
            catch (Exception F)
            {
                F.Data.Clear();
            }
        }

        /// <summary>
        /// Obtiene los Usuarios paginado
        /// </summary>
        public async Task GetUsuarios(int offset = 0)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                int next_offset = 0;

                var client = new RestClient(this.baseUrl);                
                string fields = "full_name,department,title,reports_to_name,reports_to_id,email,phone_work,employee_status,date_entered,date_modified";
                string resource = String.Format(
                    "/rest/" + VersionApi + "/Employees?max_num={0}&fields={1}&offset={2}",
                    this.MaxNum,
                    fields,
                    offset
                );
                //var request = new RestRequest("/rest/"+ VersionApi +"/Employees?offset=" + offset.ToString(), Method.GET);
                var request = new RestRequest(resource, Method.GET);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", "Bearer " + this.Token);
                RestResponse response = (RestResponse)await client.ExecuteAsync(request);
                //Console.WriteLine(response.Content);
                var usuarios = JsonConvert.DeserializeObject<Empleados>(response.Content);
                next_offset = usuarios.next_offset;

                DAL.Model.Usuarios usuarioDB = new DAL.Model.Usuarios();
                MTablasIn objTablasIn = new MTablasIn();
                foreach (var usuario in usuarios.records)
                {
                    usuarioDB = new DAL.Model.Usuarios();
                    usuarioDB.SugarId = usuario.id;
                    usuarioDB.Nombre = usuario.full_name.Trim();
                    usuarioDB.Departamento = usuario.department;
                    usuarioDB.Titulo = usuario.title;
                    usuarioDB.InformaA = usuario.reports_to_id;
                    usuarioDB.Mail = usuario.email[0].email_address;
                    usuarioDB.Telefono = usuario.phone_work;
                    usuarioDB.Estado = usuario.employee_status;
                    usuarioDB.FechaCreacion = usuario.date_entered.ToString("dd/MM/yyyy hh:mm:ss"); 

                    objTablasIn = new MTablasIn();
                    objTablasIn.Grabar_InUsuarios(usuarioDB);

                    //Console.WriteLine(oportunidad.name);
                }

                usuarioDB = null;
                objTablasIn = null;

                response = null;
                client = null;
                request = null;
                usuarios = null;

                //llamo a la siguiente pagina
                if (next_offset > offset)
                    await GetUsuarios(next_offset);
                else
                    Utilidades.LogService("GetUsuarios(): OK");

            }
            catch (Exception ex)
            {
                Utilidades.LogService("Error GetUsuarios(): " + ex.Message);
                ex.Data.Clear();
            }

        }

        /// <summary>
        /// Obtiene las Cuentas paginado
        /// </summary>
        public async Task GetCuentas(int offset = 0)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                int next_offset = 0;

                var client = new RestClient(this.baseUrl);
                string fields = "name,industry,billing_address_city,phone_office,vertical_c,version_software_c,assigned_user_name,lost,billing_address_country,alcance_de_cuenta_c";
                string resource = String.Format(
                    "/rest/" + VersionApi + "/Accounts?max_num={0}&fields={1}&offset={2}",
                    this.MaxNum,
                    fields,
                    offset
                );

                //var request = new RestRequest("/rest/"+ VersionApi +"/Accounts?max_num=100&fields=name,industry,billing_address_city,phone_office,vertical_c,version_software_c,modified_by_name,lost,billing_address_country,alcance_de_cuenta_c&offset=" + offset.ToString(),MaxNum, Method.GET);
                var request = new RestRequest(resource, Method.GET);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", "Bearer " + this.Token);
                RestResponse response = (RestResponse)await client.ExecuteAsync(request);
                //Console.WriteLine(response.Content);
                var cuentas = JsonConvert.DeserializeObject<Cuentas>(response.Content);
                next_offset = cuentas.next_offset;

                DAL.Model.Cuentas cuentaDB = new DAL.Model.Cuentas();
                MTablasIn objTablasIn = new MTablasIn();
                foreach (var cuenta in cuentas.records)
                {
                    string VersionSoftware = "";
                    try{VersionSoftware = cuenta.version_software_c?[0];}
                    catch{VersionSoftware = "";}

                    cuentaDB = new DAL.Model.Cuentas();
                    cuentaDB.SugarId = cuenta.id;                    
                    cuentaDB.Nombre = cuenta.name.Trim();
                    cuentaDB.Industria = cuenta.industry;
                    cuentaDB.Ciudad = cuenta.billing_address_city;
                    cuentaDB.Telefono = cuenta.phone_office;
                    cuentaDB.Vertical = cuenta.vertical_c;
                    cuentaDB.VersionSoftware = VersionSoftware;
                    cuentaDB.Usuario = cuenta.assigned_user_name.Trim();
                    cuentaDB.PaisEmisionFactura = cuenta.billing_address_country;
                    cuentaDB.AlcanceDeCuenta = cuenta.alcance_de_cuenta_c;

                    objTablasIn = new MTablasIn();
                    objTablasIn.Grabar_InCuentas(cuentaDB);
                    
                    //Console.WriteLine(oportunidad.name);
                }

                cuentaDB = null;
                objTablasIn = null;

                response = null;
                client = null;
                request = null;
                cuentas = null;

                //llamo a la siguiente pagina
                if (next_offset > offset)
                
                    await GetCuentas(next_offset);
                else
                    Utilidades.LogService("GetCuentas(): OK");

            }
            catch (Exception ex)
            {
                Utilidades.LogService("Error GetCuentas(): " + ex.Message);
                ex.Data.Clear();
            }

        }

        /// <summary>
        /// Obtiene las Tareas paginado
        /// </summary>
        public async Task GetTareas(int offset = 0)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                int next_offset = 0;

                var client = new RestClient(this.baseUrl);
                string resource = String.Format(
                    "/rest/" + VersionApi + "/Tasks?max_num={0}&offset={1}",
                    this.MaxNum,
                    offset
                );

                //var request = new RestRequest("/rest/"+ VersionApi +"/Tasks?max_num=100&offset=" + offset.ToString(), Method.GET);
                var request = new RestRequest(resource, Method.GET);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", "Bearer " + this.Token);
                RestResponse response = (RestResponse)await client.ExecuteAsync(request);
                //Console.WriteLine(response.Content);
                var tareas = JsonConvert.DeserializeObject<Tareas>(response.Content);
                next_offset = tareas.next_offset;

                DAL.Model.Tareas tareaDB = new DAL.Model.Tareas();
                MTablasIn objTablasIn = new MTablasIn();
                foreach (var tarea in tareas.records)
                {
                    DateTime date_due = DateTime.Now;
                    try { date_due = (DateTime)tarea.date_due; } catch { date_due = DateTime.MinValue; }

                    tareaDB = new DAL.Model.Tareas();
                    tareaDB.SugarId = tarea.id;
                    tareaDB.Cuenta = tarea.parent_type == "Opportunities" ? "" : tarea.parent_id.Trim();
                    tareaDB.Oportunidad = tarea.parent_type == "Opportunities" ? tarea.parent_id.Trim() : "";
                    tareaDB.Tipo = tarea.name;
                    tareaDB.FechaVencimiento = date_due == DateTime.MinValue ? "" : date_due.ToString("dd/MM/yyyy hh:mm:ss");
                    tareaDB.Estado = tarea.status;                    
                    tareaDB.Usuario = tarea.assigned_user_link.full_name.Trim();
                    tareaDB.FechaCreacion = tarea.date_entered.ToString("dd/MM/yyyy hh:mm:ss");
                    tareaDB.Asunto = tarea.asunto_c;
                    tareaDB.ExpertoAsignado = tarea.experto_asignado_c;
                    tareaDB.Prioridad = tarea.priority;
                    tareaDB.UltimaModificacion = tarea.date_modified.ToString("dd/MM/yyyy hh:mm:ss");                    

                    objTablasIn = new MTablasIn();
                    objTablasIn.Grabar_InTareas(tareaDB);

                    //Console.WriteLine(oportunidad.name);
                }

                tareaDB = null;
                objTablasIn = null;

                response = null;
                client = null;
                request = null;
                tareas = null;

                //llamo a la siguiente pagina
                if (next_offset > offset)
                    await GetTareas(next_offset);
                else
                    Utilidades.LogService("GetTareas(): OK");

            }
            catch (Exception ex)
            {
                Utilidades.LogService("Error GetTareas(): " + ex.Message);
                ex.Data.Clear();
            }

        }

        /// <summary>
        /// Obtiene las Autorizaciones paginado
        /// </summary>
        public async Task GetAutorizaciones(int offset = 0)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                int next_offset = 0;

                var client = new RestClient(this.baseUrl);
                string fields = "date_entered,opportunities_rn_autorizaciones_op_1opportunities_ida,estado_c,assigned_user_name,aprobacion_c,razones_c,vigente_c,date_modified,aprobacion_obligatoria_c";
                string resource = String.Format(
                    "/rest/" + VersionApi + "/RN_Autorizaciones_OP?max_num={0}&fields={1}&offset={2}",
                    this.MaxNum,
                    fields,
                    offset
                );

                //var request = new RestRequest("/rest/"+ VersionApi +"/RN_Autorizaciones_OP?max_num=100&fields=date_entered,opportunities_rn_autorizaciones_op_1opportunities_ida,estado_c,assigned_user_name,aprobacion_c,razones_c,vigente_c,date_modified,aprobacion_obligatoria_c&offset=" + offset.ToString(), Method.GET);
                var request = new RestRequest(resource, Method.GET);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", "Bearer " + this.Token);
                RestResponse response = (RestResponse)await client.ExecuteAsync(request);
                //Console.WriteLine(response.Content);
                var autorizaciones = JsonConvert.DeserializeObject<Autorizaciones>(response.Content);
                next_offset = autorizaciones.next_offset;

                DAL.Model.Autorizaciones autorizacionDB = new DAL.Model.Autorizaciones();
                MTablasIn objTablasIn = new MTablasIn();
                foreach (var autorizacion in autorizaciones.records)
                {
                    autorizacionDB = new DAL.Model.Autorizaciones();
                    autorizacionDB.SugarId = autorizacion.id;
                    autorizacionDB.FechaCreacion = autorizacion.date_entered.ToString("dd/MM/yyyy hh:mm:ss");
                    autorizacionDB.Oportunidad = autorizacion.opportunities_rn_autorizaciones_op_1opportunities_ida.Trim();
                    autorizacionDB.Estado = autorizacion.estado_c;
                    autorizacionDB.Usuario = autorizacion.assigned_user_name.Trim();
                    autorizacionDB.Aprobacion = autorizacion.aprobacion_c;
                    autorizacionDB.Razones = autorizacion.razones_c;
                    autorizacionDB.Vigente = autorizacion.vigente_c;
                    autorizacionDB.UltimaModificacion = autorizacion.date_modified.ToString("dd/MM/yyyy hh:mm:ss");
                    autorizacionDB.AprobacionObligatoria = autorizacion.aprobacion_obligatoria_c;

                    objTablasIn = new MTablasIn();
                    objTablasIn.Grabar_InAutorizaciones(autorizacionDB);

                    //Console.WriteLine(oportunidad.name);
                }

                autorizacionDB = null;
                objTablasIn = null;

                response = null;
                client = null;
                request = null;
                autorizaciones = null;

                //llamo a la siguiente pagina
                if (next_offset > offset)
                    await GetAutorizaciones(next_offset);
                else
                    Utilidades.LogService("GetAutorizaciones(): OK");

            }
            catch (Exception ex)
            {
                Utilidades.LogService("Error GetAutorizaciones(): " + ex.Message);
                ex.Data.Clear();
            }

        }

        /// <summary>
        /// Obtiene las Oportunidades paginado
        /// </summary>
        public async Task GetOportunidades(int offset = 0) {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                int next_offset = 0;

                var client = new RestClient(this.baseUrl);
                string fields = "nro_oportunidad_c,name,account_id,amount,date_closed,modified_by_name,forecasted_likely,lost,direccion_comercial_c,tipo_oportunidad_c,fecha_inicio_c,fechacreacionoriginal_c,montototal_c,probability,sales_stage,date_modified";
                string resource = String.Format(
                    "/rest/" + VersionApi + "/Opportunities?max_num={0}&fields={1}&offset={2}",
                    this.MaxNum,
                    fields,
                    offset
                );

                //var request = new RestRequest("/rest/"+ VersionApi +"/Opportunities?max_num=100&fields=nro_oportunidad_c,name,account_name,amount,date_closed,modified_by_name,forecasted_likely,lost,direccion_comercial_c,tipo_oportunidad_c,fecha_inicio_c,fechacreacionoriginal_c,montototal_c,probability,sales_stage&offset=" + offset.ToString(), Method.GET);
                var request = new RestRequest(resource, Method.GET);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", "Bearer " + this.Token);
                RestResponse response = (RestResponse)await client.ExecuteAsync(request);
                //Console.WriteLine(response.Content);
                var oportunidades = JsonConvert.DeserializeObject<Oportunidades>(response.Content);
                next_offset = oportunidades.next_offset;

                DAL.Model.Oportunidades oportunidadDB = new DAL.Model.Oportunidades();
                MTablasIn objTablasIn = new MTablasIn();
                foreach (var oportunidad in oportunidades.records)
                {
                    oportunidadDB = new DAL.Model.Oportunidades();
                    //if (oportunidad.id == "3a9e259a-516c-11ee-a248-02fd9a822739")
                    //    oportunidadDB.SugarId = oportunidad.id;
                    oportunidadDB.SugarId = oportunidad.id;
                    oportunidadDB.Numero = oportunidad.nro_oportunidad_c.ToString();
                    oportunidadDB.Nombre = oportunidad.name.Trim();
                    oportunidadDB.Cuenta = oportunidad.account_id.Trim();
                    oportunidadDB.EtapaVenta = oportunidad.sales_stage; //oportunidad.probability + "% " + oportunidad.sales_stage;                    
                    oportunidadDB.Monto = oportunidad.amount;
                    oportunidadDB.FechaCierre = oportunidad.date_closed;
                    oportunidadDB.Usuario = oportunidad.modified_by_name.Trim();
                    oportunidadDB.PrevistoProbable = oportunidad.forecasted_likely;
                    oportunidadDB.Perdido = oportunidad.lost;
                    oportunidadDB.DireccionComercial = oportunidad.direccion_comercial_c;
                    oportunidadDB.Tipo = oportunidad.tipo_oportunidad_c;
                    oportunidadDB.FechaInicio = oportunidad.fecha_inicio_c;
                    oportunidadDB.FechaRenovacion = oportunidad.fechacreacionoriginal_c;
                    oportunidadDB.UltimaModificacion = oportunidad.date_modified.ToString("dd/MM/yyyy hh:mm:ss");                    
                    oportunidadDB.CantidadConvertida = oportunidad.montototal_c;

                    objTablasIn = new MTablasIn();
                    objTablasIn.Grabar_InOportunidades(oportunidadDB);

                    //Console.WriteLine(oportunidad.name);
                }

                oportunidadDB = null;
                objTablasIn = null;

                response = null;
                client = null;
                request = null;
                oportunidades = null;

                //llamo a la siguiente pagina
                if (next_offset > offset)                
                    await GetOportunidades(next_offset);
                else
                    Utilidades.LogService("GetOportunidades(): OK");
            }
            catch (Exception ex)
            {
                Utilidades.LogService("Error GetOportunidades(): " + ex.Message);
                ex.Data.Clear();
            }

        }

        /// <summary>
        /// Obtiene las Oportunidades paginado
        /// </summary>
        public async Task GetOportunidades2(int offset = 0)
        {
            int _offset = 0;
            int next_offset = 0;

            while (_offset >= 0)
            {
                string fields = "nro_oportunidad_c,name,account_name,amount,date_closed,modified_by_name,forecasted_likely,lost,direccion_comercial_c,tipo_oportunidad_c,fecha_inicio_c,fechacreacionoriginal_c,montototal_c";
                string resource = String.Format(
                    "/rest/" + VersionApi + "/Opportunities?max_num={0}&fields={1}&offset={2}",
                    this.MaxNum,
                    fields,
                    offset
                );

                var client = new RestClient(this.baseUrl);
                client.Timeout = -1;

                var request = new RestRequest(resource, Method.GET);
                request.AddHeader("Host", "softoffice.sugarondemand.com");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", "Bearer " + this.Token);
                //request.AddHeader("Cookie", "download_token_custom=lSCFXzgXJfQu53Bq6EqVCyK1t_2jtFTlkSRcvutokSLUkODEK-FTzWQ2L6im6iwNZrvLyjuu7vc1wp_nLE0hULxjM97_t7uLHez3e3EMbOCMvthttGmX3yGc_IS2IEMHbfVySMazI4CTjQgW1LHaEo08JCqoCsEjH6lcO6Y; download_token_customa=lSCFXzgXJfQu53Bq6GahBTfnh8K_012eymBbteYqiyrt3__eDtpMpRgCF76d0CEHBfLiyCuj0PdC6LLaL2ZXf-9Xbp-h7PuASdeASm4sW8e5rOtG9HiX3xGvtL-DPQAHD_1ySMazI4CTjQgW1EX0Ri8K4ZD2HPSnvLchalE");
                //string bodyData = @"{
                //                    " + "\n" +
                //                    @"  ""max-num"": ""-1"",
                //                    " + "\n" +s
                //                    @"  ""offset"": ""_offset"",
                //                    " + "\n" +
                //                    @"  ""fields"": ""nro_oportunidad_c,name,account_name,amount,date_closed,modified_by_name,forecasted_likely,lost""
                //                    " + "\n" +
                //                    @"}
                //                    " + "\n" +
                //                    @"";
                //bodyData = bodyData.Replace("_offset", _offset.ToString());

                //request.AddJsonBody(bodyData);
                //RestResponse response = (RestResponse)await client.ExecuteAsync(request);
                IRestResponse response = await client.ExecuteAsync(request);
                //Console.WriteLine(response.Content);
                if (response.IsSuccessful)
                {
                    var oportunidades = JsonConvert.DeserializeObject<Oportunidades>(response.Content);
                    next_offset = oportunidades.next_offset;
                    _offset = next_offset;
                    if (next_offset == 0) _offset = -1;
                }
            }
        }

        /// <summary>
        /// Actualiza las tablas finales
        /// </summary>
        public void ActualizarDB()
        {
            DateTime Hasta = DateTime.Now;
            DateTime FecActual = DateTime.Now;
            string FecUltimaEjec = "";
            MTablasIn objTablasIn = new MTablasIn();
            MParametros objParametro = new MParametros();

            try
            {
                Task.Run(() => Utilidades.LogService("ActualizarDB()-Sugar: INICIO"));                               
                                                
                try
                {
                    objTablasIn.CargarTablasSugar();
                }
                catch (Exception F)
                {

                    Task.Run(() => Utilidades.LogService("ActualizarDB()-Sugar Error: " + F.Message));

                    F.Data.Clear();
                }

                FecUltimaEjec = FecActual.Year.ToString() + "/" + Formating(FecActual.Month) + "/" + Formating(FecActual.Day) + " " + Formating(FecActual.Hour) + ":" + Formating(FecActual.Minute);
                objParametro.SetearParametro("FecUltimaEjecSugar", FecUltimaEjec);

                // LA SIGUIENTE EJECUCION SE HARA:
                // DESDE LA HORA FIN QUE "TERMINA EL PROCESO" + N CANTIDAD DE HORAS (PARAMETRO)                
                GrabarProximaEjecucion();

            }
            catch (Exception ex)
            {
                // Get stack trace for the exception with source file information
                StackTrace st = new StackTrace(ex, true);
                // Get the top stack frame
                StackFrame frame = st.GetFrame(0);
                // Get the line number from the stack frame
                int line = frame.GetFileLineNumber();

                string clase = frame.GetFileName().Split('\\').Last();
                string metodo = frame.GetMethod().Name;
                
                Task.Run(() => Utilidades.LogService("ActualizarDB()-Sugar Error Clase: " + clase + " Método(): " + metodo + " Linea: " + line + " Error: " + ex.Message));

                ex.Data.Clear();
            }
            finally
            {
                objTablasIn = null;
                objParametro = null;

                Task.Run(() => Utilidades.LogService("ActualizarDB()-Sugar: FIN"));
            }
        }

        /// <summary>
        /// Graba la proxima ejecucion de EmxSugar
        /// </summary>
        public void GrabarProximaEjecucion() {
            try
            {                
                MParametros objParametro = new MParametros();
                string FechaHasta;
                DateTime FecProximaEjec;

                FecProximaEjec = this.FechaServer;
                FecProximaEjec = FecProximaEjec.AddHours(this.IntervaloExec);
                FechaHasta = FecProximaEjec.Year.ToString() + "/" + Formating(FecProximaEjec.Month) + "/" + Formating(FecProximaEjec.Day) + " " + Formating(FecProximaEjec.Hour) + ":" + Formating(FecProximaEjec.Minute);

                objParametro.SetearParametro("FecProximaEjecSugar", FechaHasta);

                objParametro = null;
            }
            catch (Exception ex)
            {
                Utilidades.LogService("Error GrabarProximaEjecucion(): " + ex.Message);
            }
        }

        /// <summary>
        /// Formateo de fecha
        /// </summary>
        private string Formating(int valor)
        {
            string s_return = valor.ToString();
            if (valor < 10)
            {
                s_return = "0" + valor;
            }
            return s_return;
        }                                          
   }  

    public static class Utilidades
    {       
        public static void SaveFile(string strFileName, string strTexto)
        {
            try
            {
               using (StreamWriter writer = new StreamWriter(strFileName, true))
               {
                    writer.Write(strTexto + System.Environment.NewLine);
               }
               // File.AppendAllText(strFileName, strTexto + System.Environment.NewLine);                            
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static async Task SaveFileAsync(string strFileName, string strTexto)
        {
            await WriteTextAsync(strFileName, strTexto);
        }

        private static async Task WriteTextAsync(string filePath, string text)
        {
            byte[] encodedText = Encoding.Unicode.GetBytes(text + System.Environment.NewLine);

            var sourceStream =
                new FileStream(
                    filePath,
                    FileMode.Append, FileAccess.Write, FileShare.None,
                    bufferSize: 4096, useAsync: true);

            await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
        }
        public static void LogService(string strTexto)
        {
            try
            {
                DateTime fecha_actual = new DateTime();
                fecha_actual = DateTime.Now;

                MParametros objParametro = new MParametros();
                string strPathFile = "";

                try
                {                    
                    strPathFile = objParametro.ObtenerParametro("Pathfiles");
                }
                catch (Exception)
                {
                    // Si no puedo obtener el path, grabo log en visor de eventos.
                    try
                    {
                        //Notificar("No se puedo obtener el parametro 'Pathfiles', error log: " + strTexto);
                        //strFileName = objParametro.ObtenerParametroFromFile("Pathfiles");
                    }
                    catch 
                    {
                        strPathFile = objParametro.ObtenerParametroFromFile("Pathfiles");
                    }                                       
                }                

                strPathFile = strPathFile + "SugarServiceLogs.txt";
                SaveFile(strPathFile, fecha_actual + " --> " + strTexto);
                objParametro = null;
            }
            catch
            {
            }
        }
        public static void LogVisorService(string strTexto)
        {
            try
            {
                DateTime fecha_actual = new DateTime();
                fecha_actual = DateTime.Now;

                MParametros objParametro = new MParametros();
                string strPathFile = "";

                try
                {
                    strPathFile = objParametro.ObtenerParametro("Pathfiles");
                }
                catch (Exception)
                {
                    // Si no puedo obtener el path, grabo log en visor de eventos.
                    try
                    {
                        //Notificar("No se puedo obtener el parametro 'Pathfiles', error log: " + strTexto);
                        //strFileName = objParametro.ObtenerParametroFromFile("Pathfiles");
                    }
                    catch
                    {
                        strPathFile = objParametro.ObtenerParametroFromFile("Pathfiles");
                    }
                }

                strPathFile = strPathFile + "SugarLogVisorService.txt";
                SaveFile(strPathFile, fecha_actual + " --> " + strTexto);
                objParametro = null;
            }
            catch
            {
            }
        }
           
    }
}
