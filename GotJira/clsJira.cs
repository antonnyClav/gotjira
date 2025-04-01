using AddAtlassianGotJiraLink;
using Atlassian.Jira;
using DAL;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace GotJira
{
    public class clsJira
    {
        public delegate void work(string notificacion);
        //public event work Notificar;

        public DateTime FechaServer
        {
            get
            {
                MParametros objParametro = new MParametros();
                try
                {
                    return objParametro.ObtenerFechaServerDb();
                }
                catch
                {
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
                    DateTime datetime = DateTime.Now;
                    int diaActual = (int)datetime.DayOfWeek;
                    if (diaActual != 5)
                    { // L A J /* esta en (6), antes corria los sabados, pero ahora se apagan los servers, todo lo que era sabado se paso a viernes */
                        return int.Parse(objParametro.ObtenerParametro("Interv_Exec_Service"));
                    }
                    else
                    {
                        return int.Parse(objParametro.ObtenerParametro("Interv_Exec_Service")); //Los dias sabados se corria con este parametro: Interv_Exec_ServiceOFF y una sola vez (12horas) ahora se corre full cada 4 horas
                    }
                }
                catch
                {
                    return 4;
                }
                finally
                {
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
                    DateTime datetime = DateTime.Now;
                    int diaActual = (int)datetime.DayOfWeek;
                    if (diaActual != 5)
                    { // L A J /* esta en (6), antes corria los sabados, pero ahora se apagan los servers, todo lo que era sabado se paso a viernes */
                        return int.Parse(objParametro.ObtenerParametro("HoraSincDesde"));
                    }
                    else
                    {
                        return int.Parse(objParametro.ObtenerParametro("HoraSincDesdeOFF"));
                    }
                }
                catch
                {
                    return 8;
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
                    DateTime datetime = DateTime.Now;
                    int diaActual = (int)datetime.DayOfWeek;
                    if (diaActual != 5)
                    { // L A J /* esta en (6), antes corria los sabados, pero ahora se apagan los servers, todo lo que era sabado se paso a viernes */
                        return int.Parse(objParametro.ObtenerParametro("HoraSincHasta"));
                    }
                    else
                    {
                        return int.Parse(objParametro.ObtenerParametro("HoraSincHastaOFF"));
                    }
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

                retHorarioValido = (horaActual >= HoraDesde && horaActual <= HoraHasta && diaActual > 0 && diaActual <= 5); /* esta en (6), antes corria los sabados, pero ahora se apagan los servers, todo lo que era sabado se paso a viernes */

                if (!retHorarioValido)
                {
                    GrabarProximaEjecucion();
                }

                return retHorarioValido;
                // quitar               
                //return true;
            }
        }

        private string authToken = "amlyYS5lbXhAZ21haWwuY29tOjVSNlIzVjhiM0hNR1ZmTXFSUUVqMTFDNQ==";
        public string User { get; set; } = "";
        public string ApiToken { get; set; } = "";

        //por default jira devuelve de a 100 registros, si esto cambia entonces cambiar este valor ya que los metodos estan preparados para alternar
        public int ItemsMax { get; set; } = 100;

        private static JiraRestClientSettings jiraRestClientSettings = new JiraRestClientSettings();

        private static Jira jiraConn;
        //https://softoffice.atlassian.net/sr/jira.issueviews:searchrequest-csv-current-fields/37473/SearchRequest-37473.csv?tempMax=1000
        public clsJira()
        {
            try
            {
                jiraRestClientSettings.EnableRequestTrace = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                MParametros objParametro = new MParametros();

                User = objParametro.ObtenerParametro("UserJira");
                ApiToken = Utilidades.Desencriptar(objParametro.ObtenerParametro("APItokenJira"));

                // ApiToken = objParametro.ObtenerParametro("APItokenJira");
                objParametro = null;

                jiraConn = Jira.CreateRestClient("https://softoffice.atlassian.net", User, ApiToken, jiraRestClientSettings);

            }
            catch (Exception ex)
            {
                Utilidades.LogService("Error Carga Proyectos: " + ex.Message);
            }
        }

        /// <summary>
        /// Retorna todos los jiras del proyecto consultado. 
        /// </summary>
        //public async Task<bool> GetProjectAndIssues(string ProjectKey)
        //{
        //    try
        //    {
        //        await GetProject(ProjectKey);
        //        await GetIssues("", "");
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.Data.Clear();
        //    }

        //    return true;
        //}

        /// <summary>
        /// Retorna todos los jiras del proyecto consultado filtrando por fechas.
        /// </summary>
        /// <param name="FechaDesde">Formato yyyy/mm/dd hh:mm.</param>
        /// <param name="FechaHasta">Formato yyyy/mm/dd hh:mm.</param>
        //public async Task<bool> GetProjectAndIssues(string ProjectKey, string FechaDesde = "", string FechaHasta = "")
        //{
        //    try
        //    {
        //        await GetProject(ProjectKey);
        //        await GetIssues(FechaDesde, FechaHasta);
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.Data.Clear();
        //    }

        //    return true;
        //}

        /// <summary>
        /// Retorna todos los proyectos con sus jiras, enlaces, componentes etc...
        /// </summary>
        //public async Task<bool> GetAllProjectsAndIssues()
        //{
        //    try
        //    {
        //        await GetProjects();
        //        await GetIssues();
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.Data.Clear();
        //    }

        //    return true;
        //}

        /// <summary>
        /// Retorna el proyecto solicitado
        /// </summary>
        //public async Task<int> GetProject(string ProjectKey)
        //{
        //    Project project = await jiraConn.Projects.GetProjectAsync(ProjectKey);

        //    //leer de parametro
        //    MParametros objParametro = new MParametros();
        //    string strPathFile = "";
        //    strPathFile = objParametro.ObtenerParametro("Pathfiles");
        //    objParametro = null;

        //    strPathFile = strPathFile + "projects.txt";

        //    if (project != null)
        //    {
        //        //File.Delete(strPathFile);
        //        File.WriteAllText(strPathFile, string.Empty);
        //    }

        //    try
        //    {
        //        //Notificar("Consultando Proyecto");
        //        try
        //        {
        //            Utilidades.SaveFile(strPathFile, project.Category.Name + "|" + project.Name + "|" + project.Key + "|" + project.LeadUser.DisplayName); //project.Lead
        //        }
        //        catch (Exception)
        //        {
        //            Utilidades.SaveFile(strPathFile, "" + "|" + project.Name + "|" + project.Key + "|" + project.LeadUser.DisplayName); //project.Lead
        //        }
        //        int resultado;
        //        MTablasIn objTablasIn = new MTablasIn();
        //        resultado = objTablasIn.Grabar_InProject();
        //        //Notificar("Proyecto Grabado...");

        //        //Notificar("Consultando Proyecto Por Componentes");
        //        await GetProjectForComponent();
        //        //Notificar("Proyecto Por Componentes Grabados");

        //        objTablasIn = null;

        //        return resultado;
        //    }
        //    catch (Exception ex)
        //    {
        //        //Notificar("Error Carga Proyecto!!!");
        //        throw ex;
        //    }

        //}

        /// <summary>
        /// Retorna todos los proyectos activos. 
        /// </summary>
        /// 

        //public void GetProjects2()
        //{
        //    var client = new RestClient("https://softoffice.atlassian.net/rest/api/2/project");
        //    client.Timeout = -1;
        //    var request = new RestRequest(Method.GET);
        //    request.AddHeader("Authorization", "Basic amlyYS5lbXhAZ21haWwuY29tOjVSNlIzVjhiM0hNR1ZmTXFSUUVqMTFDNQ==");
        //    request.AddHeader("Cookie", "atlassian.xsrf.token=B5S9-YRZO-3SUG-U7W9_82cea532ab9e360026a68df157a6ab4478eeffac_lin");
        //    IRestResponse response = client.Execute(request);
        //    Console.WriteLine(response.Content);
        //}

        public async Task<int> GetProjects()
        {
            int cantidad = 0;
            if (!this.HorarioValido)
            {
                return -1;
            }

            try
            {
                // Project project = await jiraConn.Projects.GetProjectAsync("BGP1038");
                IEnumerable<Project> projects = await jiraConn.Projects.GetProjectsAsync();


                MParametros objParametro = new MParametros();
                string strPathFile = "";
                strPathFile = objParametro.ObtenerParametro("Pathfiles");
                strPathFile = strPathFile + "projects.txt";

                objParametro = null;

                //Notificar("Consultando Proyectos");
                cantidad = projects.Count();
                if (cantidad > 0)
                {
                    //File.Delete(strPathFile);
                    File.WriteAllText(strPathFile, string.Empty);
                }

                foreach (Project proyecto in projects)
                {
                    //if (projecto.Key == "BGP1038")
                    //{
                    //    System.Diagnostics.Debugger.Break();
                    //}
                    string Category = "";
                    try
                    {
                        Category = proyecto?.Category?.Name;
                    }
                    catch (Exception)
                    {

                        Category = "";
                    }
                    try
                    {
                        Utilidades.SaveFile(strPathFile, Category + "|" + proyecto.Name + "|" + proyecto.Key + "|" + proyecto.LeadUser.DisplayName); //project.Lead
                    }
                    catch (Exception ex)
                    {
                        Utilidades.LogService("Error Carga Proyectos: " + ex.Message);
                        //Notificar("Error Carga Proyectos: " + ex.Message);
                        throw ex;
                    }
                }
                //int resultado;
                //MTablasIn objTablasIn = new MTablasIn();
                //resultado = objTablasIn.Grabar_InProject();
                ////Notificar("Proyectos Grabados...");

                ////Notificar("Consultando Proyecto Por Componentes");
                //await GetProjectForComponent();
                ////Notificar("Proyecto Por Componentes Grabados");

                //objTablasIn = null;

                return cantidad;
            }
            catch (Exception ex)
            {
                await Task.Run(() => Utilidades.LogService("Error Carga Proyectos: " + ex.Message));
                //Notificar("Error Carga Proyectos: " + ex.Message);
                throw ex;
            }
            finally
            {
                await Task.Run(() => Utilidades.LogService("GetProjects() FIN"));
            }
        }

        public void ActualizarDB()
        {            
            DateTime Hoy = DateTime.Now;
            DateTime Desde;
            DateTime Hasta = DateTime.Now;
            DateTime FecProximaEjec;

            string FecUltimaEjec = "";            
            int _TopeDiasTimeSheet = 0;            
            int diaActual = (int)Hoy.DayOfWeek;
            
            MTablasIn objTablasIn = new MTablasIn();
            objTablasIn.LimpiarTablasIN();

            MParametros objParametro = new MParametros();
            
            try
            {
                Utilidades.LogService("ActualizarDB(): INICIADO");
                try
                {
                    string strPathFile = "";
                    strPathFile = objParametro.ObtenerParametro("Pathfiles") + "projects.txt";
                    BulkInsertProjects pry = new BulkInsertProjects();                    
                    try
                    {
                        pry.LoadCsvToDataTableAndBulkInsert(strPathFile);
                    }
                    catch (Exception ex)
                    {
                        ex.Data.Clear();
                    }
                    pry = null;

                    objTablasIn.Grabar_InProject();
                }
                catch (Exception F)
                {

                    Utilidades.LogService("Error Grabar_InProject(): " + F.Message);

                    F.Data.Clear();
                }

                try
                {
                    string strPathFile = "";
                    strPathFile = objParametro.ObtenerParametro("Pathfiles") + "proyectos_x_componentes.txt";
                    BulkInsertProjectsXComp pxc = new BulkInsertProjectsXComp();                    
                    try
                    {
                        pxc.LoadCsvToDataTableAndBulkInsert(strPathFile);
                    }
                    catch (Exception ex)
                    {
                        ex.Data.Clear();
                    }
                    pxc = null;

                    objTablasIn.Grabar_InProyectosPorComponentes();
                }
                catch (Exception F)
                {

                    Utilidades.LogService("Error Grabar_InProyectosPorComponentes(): " + F.Message);

                    F.Data.Clear();
                }

                try
                {
                    string strPathFile = "";
                    strPathFile = objParametro.ObtenerParametro("Pathfiles") + "usuarios.txt";
                    BulkInsertUsuarios usu = new BulkInsertUsuarios();                    
                    try
                    {
                        usu.LoadCsvToDataTableAndBulkInsert(strPathFile);
                    }
                    catch (Exception ex)
                    {
                        ex.Data.Clear();
                    }
                    usu = null;

                    objTablasIn.Grabar_InUsuariosJira();
                }
                catch (Exception F)
                {

                    Utilidades.LogService("Error Grabar_InUsuariosJira(): " + F.Message);

                    F.Data.Clear();
                }

                try
                {
                    if (diaActual != 5) // L A J /* esta en (6), antes corria los sabados, pero ahora se apagan los servers, todo lo que era sabado se paso a viernes */
                    {
                        _TopeDiasTimeSheet = int.Parse(objParametro.ObtenerParametro("TopeDiasTimeSheet"));
                    }
                    else
                    {
                        _TopeDiasTimeSheet = int.Parse(objParametro.ObtenerParametro("TopeDiasTimeSheetOFF"));
                    }


                    //topes dias
                    if (_TopeDiasTimeSheet > 0)
                    {
                        Desde = DateTime.Today.AddDays(-_TopeDiasTimeSheet);
                        Hasta = Hoy;
                    }
                    else
                    {
                        //desde y hasta parametrizado
                        if (_TopeDiasTimeSheet == 0)
                        {
                            Desde = DateTime.Parse(objParametro.ObtenerParametro("TimeSheetFechaDesde"));
                            Hasta = DateTime.Parse(objParametro.ObtenerParametro("TimeSheetFechaHasta"));
                        }
                        else //todo el mes anterior + el mes actual 
                        {
                            // Obtener el primer día del mes actual
                            DateTime PrimerDiaMesActual = new DateTime(Hoy.Year, Hoy.Month, 1);

                            // Obtener el primer día del mes anterior
                            DateTime PrimerDiaMesAnterior = PrimerDiaMesActual.AddMonths(-1);
                            Desde = PrimerDiaMesAnterior;
                            Hasta = Hoy;
                        }
                    }
                }
                catch (Exception)
                {
                    Desde = DateTime.Today.AddDays(-_TopeDiasTimeSheet);
                    Hasta = DateTime.Now;
                }                               

                try
                {
                    string strPathFile = "";
                    strPathFile = objParametro.ObtenerParametro("Pathfiles") + "jiras.txt";
                    BulkInsertJiras jir = new BulkInsertJiras();                    
                    try
                    {
                        jir.LoadCsvToDataTableAndBulkInsert(strPathFile);
                    }
                    catch (Exception ex)
                    {
                        ex.Data.Clear();
                    }
                    jir = null;

                    strPathFile = objParametro.ObtenerParametro("Pathfiles") + "jiras_adic.txt";
                    BulkInsertJirasAdic jirAdic = new BulkInsertJirasAdic();
                    try
                    {
                        jirAdic.LoadCsvToDataTableAndBulkInsert(strPathFile);
                    }
                    catch(Exception ex){
                        ex.Data.Clear();
                    }                    
                    jirAdic = null;

                    strPathFile = objParametro.ObtenerParametro("Pathfiles") + "jiras_adic_gdd.txt";
                    BulkInsertJirasAdicGDD jirAdicGDD = new BulkInsertJirasAdicGDD();
                    try
                    {
                        jirAdicGDD.LoadCsvToDataTableAndBulkInsert(strPathFile);
                    }
                    catch (Exception ex)
                    {
                        ex.Data.Clear();
                    }
                    jirAdicGDD = null;

                    objTablasIn.Grabar_InJiras(Desde, Hasta);

                    //--------------------------------------------------------------------------------------------------------
                    // Llenado de worklogs
                    //--------------------------------------------------------------------------------------------------------
                    try
                    {
                        strPathFile = objParametro.ObtenerParametro("Pathfiles") + "worklogs.txt";
                        BulkInsertWorkLogs wrl = new BulkInsertWorkLogs();

                        try
                        {
                            wrl.LoadCsvToDataTableAndBulkInsert(strPathFile);
                        }
                        catch (Exception ex)
                        {
                            Utilidades.LogService("Error WorkLog: " + ex.Message);
                        }
                        wrl = null;

                        string Mensaje = "";
                        Mensaje = objTablasIn.Grabar_InTimeSheet(Desde, Hasta);

                        if (Mensaje != "") Utilidades.LogService("Grabar_InTimeSheet(): " + Mensaje);

                    }
                    catch (Exception F)
                    {

                        Utilidades.LogService("Error Grabar_InTimeSheet(): " + F.Message);

                        F.Data.Clear();
                    }
                    //--------------------------------------------------------------------------------------------------------
                    // Llenado de worklogs
                    //--------------------------------------------------------------------------------------------------------
                }
                catch (Exception F)
                {

                    Utilidades.LogService("Error Grabar_InJiras(): " + F.Message);

                    F.Data.Clear();
                }

                if (diaActual != 5) // L A J /* esta en (6), antes corria los sabados, pero ahora se apagan los servers, todo lo que era sabado se paso a viernes */
                {
                    try
                    {
                        ActualizarRol();
                    }
                    catch (Exception F)
                    {

                        Utilidades.LogService("Error ActualizarRol(): " + F.Message);

                        F.Data.Clear();
                    }

                }

                //los enlaces corre todos los dias
                try
                {
                    string strPathFile = "";
                    strPathFile = objParametro.ObtenerParametro("Pathfiles") + "enlaces.txt";
                    BulkInsertLinks lnk = new BulkInsertLinks();                    
                    try
                    {
                        lnk.LoadCsvToDataTableAndBulkInsert(strPathFile);
                    }
                    catch (Exception ex)
                    {
                        ex.Data.Clear();
                    }
                    lnk = null;

                    strPathFile = objParametro.ObtenerParametro("Pathfiles") + "minitoc.csv";
                    BulkInsertMinitoc mnt = new BulkInsertMinitoc();                    
                    try
                    {
                        mnt.LoadCsvToDataTableAndBulkInsert(strPathFile);                        
                    }
                    catch (Exception ex)
                    {
                        ex.Data.Clear();
                    }
                    mnt = null;

                    objTablasIn.Grabar_InEnlaces();
                }
                catch (Exception F)
                {

                    Utilidades.LogService("Error Grabar_InEnlaces(): " + F.Message);

                    F.Data.Clear();
                }                                

                FecUltimaEjec = Hoy.Year.ToString() + "/" + Formating(Hoy.Month) + "/" + Formating(Hoy.Day) + " " + Formating(Hoy.Hour) + ":" + Formating(Hoy.Minute);
                objParametro.SetearParametro("FecUltimaEjec", FecUltimaEjec);
                //objParametro.SetearParametro("SyncOnlyThisUserTimeSheet", "");

                // LA SIGUIENTE EJECUCION SE HARA:
                // DESDE LA HORA FIN QUE "TERMINA EL PROCESO" + N CANTIDAD DE HORAS (PARAMETRO)                
                GrabarProximaEjecucion();

                // Este metodo solo se llama en la ultima ejecucion del dia.
                // Si la hora para la proxima ejecucion no es valida, quiere decir que esta es la ultima ejecucion
                // por lo que el proceso debe ser ejecutado...
                FecProximaEjec = this.FechaServer;
                FecProximaEjec = FecProximaEjec.AddHours(this.IntervaloExec);
                if (!EsHorarioValido(FecProximaEjec))
                {
                    try
                    {
                        PrecalculoIndicadoresProyecto();

                        //si ésta la ultima ejecucion y es el ultimo dia limpiamos los logs
                        if (diaActual == 5)
                        {
                            Utilidades.LimpiarLogs();
                        }
                    }
                    catch (Exception F)
                    {

                        Utilidades.LogService("Error PrecalculoIndicadoresProyecto(): " + F.Message);

                        F.Data.Clear();
                    }

                }

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

                Utilidades.LogService("Error ActualizarDB() Clase: " + clase + " Método(): " + metodo + " Linea: " + line + " Error: " + ex.Message);

                ex.Data.Clear();
            }
            finally
            {
                objTablasIn = null;
                objParametro = null;                
            }
            Console.WriteLine("FIN");
            Utilidades.LogService("ActualizarDB(): FIN");
        }

        //public void ActualizarDBLite(DateTime FecUltimaSinIssuesLite)
        //{
        //    DateTime Desde = DateTime.Now;
        //    DateTime Hasta = DateTime.Now;
        //    DateTime datetime = DateTime.Now;
        //    int diaActual = (int)datetime.DayOfWeek;
        //    DateTime FecActual = DateTime.Now;
        //    string FecUltimaEjec = "";
        //    MTablasIn objTablasIn = new MTablasIn();
        //    MParametros objParametro = new MParametros();

        //    try
        //    {
        //        Task.Run(() => Utilidades.LogService("FIN Jira"));

        //        try
        //        {
        //            objTablasIn.Grabar_InProject();
        //        }
        //        catch (Exception F)
        //        {

        //            Task.Run(() => Utilidades.LogService("Error Grabar_InProject(): " + F.Message));

        //            F.Data.Clear();
        //        }

        //        try
        //        {
        //            objTablasIn.Grabar_InUsuariosJira();
        //        }
        //        catch (Exception F)
        //        {

        //            Task.Run(() => Utilidades.LogService("Error Grabar_InUsuariosJira(): " + F.Message));

        //            F.Data.Clear();
        //        }

        //        try
        //        {
        //            string Mensaje = "";
        //            Mensaje = objTablasIn.Grabar_InTimeSheet(Desde, Hasta);
        //            if (Mensaje != "") Task.Run(() => Utilidades.LogService("Grabar_InTimeSheet(): " + Mensaje));
        //        }
        //        catch (Exception F)
        //        {

        //            Task.Run(() => Utilidades.LogService("Error Grabar_InTimeSheet(): " + F.Message));

        //            F.Data.Clear();
        //        }

        //        try
        //        {
        //            objTablasIn.Grabar_InJiras(Desde, Hasta);
        //        }
        //        catch (Exception F)
        //        {

        //            Task.Run(() => Utilidades.LogService("Error Grabar_InJiras(): " + F.Message));

        //            F.Data.Clear();
        //        }

        //        //los enlaces corre todos los dias
        //        try
        //        {
        //            objTablasIn.Grabar_InEnlaces();
        //        }
        //        catch (Exception F)
        //        {

        //            Task.Run(() => Utilidades.LogService("Error Grabar_InEnlaces(): " + F.Message));

        //            F.Data.Clear();
        //        }


        //        FecUltimaEjec = FecActual.Year.ToString() + "/" + Formating(FecActual.Month) + "/" + Formating(FecActual.Day) + " " + Formating(FecActual.Hour) + ":" + Formating(FecActual.Minute);
        //        objParametro.SetearParametro("FecUltimaEjec", FecUltimaEjec);
        //        objParametro.SetearParametro("FecUltimaSinIssuesLite", FecUltimaSinIssuesLite.ToString());
        //        // LA SIGUIENTE EJECUCION SE HARA:
        //        // DESDE LA HORA FIN QUE "TERMINA EL PROCESO" + N CANTIDAD DE HORAS (PARAMETRO)                
        //        //GrabarProximaEjecucion();

        //    }
        //    catch (Exception ex)
        //    {
        //        // Get stack trace for the exception with source file information
        //        StackTrace st = new StackTrace(ex, true);
        //        // Get the top stack frame
        //        StackFrame frame = st.GetFrame(0);
        //        // Get the line number from the stack frame
        //        int line = frame.GetFileLineNumber();

        //        string clase = frame.GetFileName().Split('\\').Last();
        //        string metodo = frame.GetMethod().Name;

        //        Task.Run(() => Utilidades.LogService("Error ActualizarDBLite() Clase: " + clase + " Método(): " + metodo + " Linea: " + line + " Error: " + ex.Message));

        //        ex.Data.Clear();
        //    }
        //    finally
        //    {
        //        objTablasIn = null;
        //        objParametro = null;

        //        Task.Run(() => Utilidades.LogService("ActualizarDBLite(): FIN"));
        //    }
        //}

        public void GrabarProximaEjecucion()
        {
            try
            {
                MParametros objParametro = new MParametros();
                string FechaHasta;
                DateTime FecProximaEjec;

                FecProximaEjec = this.FechaServer;
                FecProximaEjec = FecProximaEjec.AddHours(this.IntervaloExec);
                FechaHasta = FecProximaEjec.Year.ToString() + "/" + Formating(FecProximaEjec.Month) + "/" + Formating(FecProximaEjec.Day) + " " + Formating(FecProximaEjec.Hour) + ":" + Formating(FecProximaEjec.Minute);

                objParametro.SetearParametro("FecProximaEjec", FechaHasta);

                objParametro = null;
            }
            catch (Exception ex)
            {
                Utilidades.LogService("Error GrabarProximaEjecucion(): " + ex.Message);
            }
        }

        /// <summary>
        /// Retorna el jira solicitado
        /// </summary>
        //public async Task<int> GetIssue(string IssueKey)
        //{
        //    MParametros objParametro = new MParametros();

        //    int resultado;
        //    string strPathFile = "";
        //    strPathFile = objParametro.ObtenerParametro("Pathfiles");
        //    strPathFile = strPathFile + "jiras.txt";

        //    objParametro = null;

        //    try
        //    {
        //        //Notificar("Consultando Jira");
        //        //no siempre este metodo funciona, existe para esto un metodo custom SearchJiraAsync()
        //        var _issues = await jiraConn.Issues.GetIssuesAsync(IssueKey);
        //        resultado = _issues.Count;
        //        if (resultado > 0)
        //        {
        //            //File.Delete(strPathFile);
        //            File.WriteAllText(strPathFile, string.Empty);
        //        }

        //        foreach (var issue in _issues)
        //        {

        //            Utilidades.SaveFile(strPathFile, issue.Value.Project + "|" + issue.Key + "|" + issue.Value.Type.Name + "|"
        //                    + issue.Value.Summary.Trim() + "|" + issue.Value["Epic Link"] + "|" + issue.Value["Grupo de Actividad"] + "|" + issue.Value["Sprint"] + "|" + issue.Value.ParentIssueKey
        //                    + "|" + issue.Value["Origen del error"] + "|" + issue.Value["Fase detectado"] + "|" + (issue.Value.CustomFields[0].Name == "Cliente" ? issue.Value.CustomFields[0].Values[0] : "") + "|" + issue.Value["Marco Producto"]);

        //            //Notificar("Consultando Enlace...");
        //            await GetLinkForIssue(issue.Key);
        //            //Notificar("Enlace cargado!");
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        ex.Data.Clear();
        //    }

        //    MTablasIn objTablasIn = new MTablasIn();
        //    resultado = objTablasIn.Grabar_InJiras(DateTime.Parse("XXXX"), DateTime.Parse("XXXX"));
        //    //Notificar("Jira Grabado...");

        //    objTablasIn = null;
        //    return resultado;
        //}

        /// <summary>
        /// Retorna los jiras del proyecto
        /// </summary>
        /// <param name="FechaDesde">Formato yyyy/mm/dd hh:mm.</param>
        /// <param name="FechaHasta">Formato yyyy/mm/dd hh:mm.</param>        
        //public async Task<int> GetIssues(string FechaDesde = "", string FechaHasta = "", bool v_ultimos = false)
        //{
        //    int resultado = 0;
        //    try
        //    {
        //        string strPathFile = "";
        //        string strPathFileEnlacesJira = "";
        //        string strPathFileJira = "";
        //        string strPathFileJiraAdic = "";
        //        string strPathFileJiraAdicGdd = "";
        //        string strPathFileWorkLogs = "";
        //        bool ArchivoBorrado = false;

        //        if (!this.HorarioValido)
        //        {
        //            return -1;
        //        }

        //        MParametros objParametro = new MParametros();

        //        strPathFile = objParametro.ObtenerParametro("Pathfiles");

        //        strPathFileEnlacesJira = strPathFile + "enlaces.txt";
        //        strPathFileJira = strPathFile + "jiras.txt";
        //        strPathFileJiraAdic = strPathFile + "jiras_adic.txt";
        //        strPathFileJiraAdicGdd = strPathFile + "jiras_adic_gdd.txt";
        //        strPathFileWorkLogs = strPathFile + "worklogs.txt";
        //        //---------------------------------------------- NUEVOS JIRAS ----------------------------------
        //        //Notificar("Obteniendo Jiras...");

        //        int cantOk = 0;
        //        int cantError = 0;
        //        MTablasIn objJirasNuevos = new MTablasIn();
        //        string Jiras_key = objJirasNuevos.Obtener_JirasKeyActualizar(v_ultimos);

        //        //obtengo jiras epicos despues de la primera ejecucion de jiras nuevos
        //        string Jiras_keyEpic = "";
        //        if (v_ultimos == true)
        //        {
        //            Jiras_keyEpic = objJirasNuevos.Obtener_EpicasActualizar();
        //        }

        //        if (Jiras_key == "" && Jiras_keyEpic == "")
        //        {
        //            //Notificar("GetIssues(): NO HAY JIRAS PARA ACTUALIZAR!!!");
        //        }
        //        else
        //        {
        //            Array A_Jiras_key = Jiras_key.Split(',').ToArray();
        //            Array A_Jiras_key_Epica = Jiras_keyEpic.Split(',').ToArray();


        //            var NewIssueKeys = new List<string>((string[])A_Jiras_key);
        //            var NewEpicKeys = new List<string>((string[])A_Jiras_key_Epica);

        //            if (v_ultimos == true)
        //            {
        //                var EpicKeys = new List<string>();

        //                //busco los hijos de cada epica
        //                foreach (string IssueKey in NewEpicKeys)
        //                {
        //                    //ingreso al padre para ser consultado
        //                    EpicKeys.Add(IssueKey);

        //                    //busco a los hijos
        //                    List<string> tmpEpicKeys = SearchEpicJiraAsync(IssueKey);
        //                    //ingreso a los hijos
        //                    foreach (string epic in tmpEpicKeys)
        //                    {
        //                        EpicKeys.Add(epic);
        //                    }
        //                }

        //                //Cargo los jiras de la ejecucion anterior (v_ultimos=false) para verificar de no duplicar las key
        //                string Jiras_key_Anterior = objJirasNuevos.Obtener_JirasKeyEjecucionAnterior();
        //                Array A_Jiras_key_Anterior = Jiras_key_Anterior.Split(',').ToArray();
        //                var NewIssueKeys_Anterior = new List<string>((string[])A_Jiras_key_Anterior);

        //                //agrego la key del jira siempre y cuando no este en los arrays de ejecuciones
        //                foreach (string epic in EpicKeys)
        //                {

        //                    if (!NewIssueKeys.Contains(epic) && !NewIssueKeys_Anterior.Contains(epic))
        //                        NewIssueKeys.Add(epic);
        //                }
        //            }

        //            resultado = NewIssueKeys.Count();
        //            foreach (string IssueKey in NewIssueKeys)
        //            {
        //                if (IssueKey != "")
        //                {
        //                    // if (IssueKey.Trim() == "EMX3051-179") System.Diagnostics.Debugger.Break();

        //                    try
        //                    {
        //                        if (v_ultimos)
        //                            ArchivoBorrado = v_ultimos;

        //                        if (ArchivoBorrado == false)
        //                        {
        //                            //File.Delete(strPathFileEnlacesJira);
        //                            File.WriteAllText(strPathFileEnlacesJira, string.Empty);

        //                            //File.Delete(strPathFileJira);
        //                            File.WriteAllText(strPathFileJira, string.Empty);

        //                            //File.Delete(strPathFileJiraAdic);
        //                            File.WriteAllText(strPathFileJiraAdic, string.Empty);

        //                            //File.Delete(strPathFileJiraAdicGdd);
        //                            File.WriteAllText(strPathFileJiraAdicGdd, string.Empty);

        //                            //File.Delete(strPathFileWorkLogs);
        //                            File.WriteAllText(strPathFileWorkLogs, string.Empty);

        //                            ArchivoBorrado = true;
        //                        }
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        Utilidades.LogService("Error Carga GetIssues(): " + "ERROR, NO SE PUDO ELIMINAR " + strPathFileJira + " ERROR: " + ex.Message);
        //                    }
        //                    AddAtlassianGotJiraJiras.AddJira issue = new AddAtlassianGotJiraJiras.AddJira();
        //                    try
        //                    {

        //                        issue = new AddAtlassianGotJiraJiras.AddJira();
        //                        issue = SearchJiraAsync(IssueKey);

        //                        if (issue.key == null) continue;
        //                        //SaveIssueToFile(issue);
        //                        SaveIssueToFile((dynamic)issue);

        //                        cantOk++;
        //                    }
        //                    catch (Exception t)
        //                    {
        //                        t.Data.Clear();
        //                        cantError++;
        //                    }

        //                    //obtengo los enlaces de este jira                        
        //                    await GetLinkForIssue(IssueKey);

        //                    //SE OBTIENEN LAS HORAS DE ESTE JIRA:
        //                    await GetWorklogAsync(issue.fields.project.key, issue.key, issue.fields.summary, issue.fields.issuetype.name, issue.fields.priority.name);

        //                    issue = null;
        //                }
        //            }

        //            objJirasNuevos = null;
        //        }
        //        //Notificar("Jiras Erroneos: " + cantError.ToString());
        //        //Notificar("Jiras OK: " + cantOk.ToString());
        //        //---------------------------------------------- FIN NUEVOS JIRAS ----------------------------------

        //        //MTablasIn objTablasIn = new MTablasIn();
        //        //resultado = objTablasIn.Grabar_InJiras(Desde, DateTime.Parse(FechaHasta));
        //        //Notificar("Jiras Grabados...");



        //        //objParametro.SetearParametro("FecUltimaEjec", FechaHasta);

        //        //// LA SIGUIENTE EJECUCION SE HARA:
        //        //// DESDE LA HORA FIN QUE "TERMINA EL PROCESO" + N CANTIDAD DE HORAS (PARAMETRO)
        //        //FecProximaEjec = this.FechaServer;
        //        //FecProximaEjec = FecProximaEjec.AddHours(this.IntervaloExec);
        //        //FechaHasta = FecProximaEjec.Year.ToString() + "/" + Formating(FecProximaEjec.Month) + "/" + Formating(FecProximaEjec.Day) + " " + Formating(FecProximaEjec.Hour) + ":" + Formating(FecProximaEjec.Minute);                       

        //        //objParametro.SetearParametro("FecProximaEjec", FechaHasta);

        //        //// Este metodo solo se llama en la ultima ejecucion del dia.
        //        //// Si la hora para la proxima ejecucion no es valida, quiere decir que esta es la ultima ejecucion
        //        //// por lo que el proceso debe ser ejecutado...
        //        //if (!EsHorarioValido(FecProximaEjec))
        //        //{
        //        //    PrecalculoIndicadoresProyecto();
        //        //}

        //        //objTablasIn = null;
        //        objParametro = null;
        //    }
        //    catch (Exception ex)
        //    {
        //        // Get stack trace for the exception with source file information
        //        StackTrace st = new StackTrace(ex, true);
        //        // Get the top stack frame
        //        StackFrame frame = st.GetFrame(0);
        //        // Get the line number from the stack frame
        //        int line = frame.GetFileLineNumber();

        //        string clase = frame.GetFileName().Split('\\').Last();
        //        string metodo = frame.GetMethod().Name;
        //        await Task.Run(() => Utilidades.LogService("Error GetIssues() Clase: " + clase + " Método(): " + metodo + " Linea: " + line + " Error: " + ex.Message));
        //    }
        //    finally
        //    {
        //        if (v_ultimos)
        //            await Task.Run(() => Utilidades.LogService("GetIssues(): FIN"));
        //    }

        //    return resultado;
        //}

        /// <summary>
        /// Eliminacion de Worklogs
        /// </summary>  
        public async Task DeleteWorklogsAndHours()
        {
            try
            {
                DateTime fechaHoy = new DateTime();
                fechaHoy = DateTime.Today.AddDays(-5); //Eliminaciones de horas desde hoy a 5 dias para atras. (estos 5 puede ser poco)

                DateTime fecha = new DateTime(fechaHoy.Year, fechaHoy.Month, fechaHoy.Day, 0, 0, 0, DateTimeKind.Utc);
                long FechaMiliSegundos = new DateTimeOffset(fecha).ToUnixTimeMilliseconds();
                //Console.WriteLine(FechaMiliSegundos);

                await DeleteWorklogsAndHoursAsync(FechaMiliSegundos);
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }

        /// <summary>
        /// Retorna los issues del dia
        /// </summary>     
        public async Task<DateTime> GetIssuesLite()
        {
            MParametros objParametro = new MParametros();
            int startAt = 0;
            int _TopeDiasJiras = 7;
            DateTime Desde = DateTime.Now;
            DateTime Hasta = DateTime.Now;
            int diaActual = (int)Desde.DayOfWeek;

            if (!this.HorarioValido)
            {
                return Hasta;
            }

            try
            {
                if (diaActual != 5) // L A J /* esta en (6), antes corria los sabados, pero ahora se apagan los servers, todo lo que era sabado se paso a viernes */
                {
                    _TopeDiasJiras = int.Parse(objParametro.ObtenerParametro("TopeDiasJiras"));
                }
                else
                {
                    _TopeDiasJiras = int.Parse(objParametro.ObtenerParametro("TopeDiasJirasOFF"));
                }


                if (_TopeDiasJiras > 0)
                {
                    Desde = DateTime.Today.AddDays(-_TopeDiasJiras);
                    Hasta = DateTime.Now;
                }
                else
                {
                    if (_TopeDiasJiras == 0)
                    {
                        Desde = DateTime.Now; // .Today.AddDays(-7);
                        Hasta = DateTime.Now;
                    }
                    else if(_TopeDiasJiras == -1) { //esto es solo para que no busque jiras, enlanes ni worklogs
                        Desde = DateTime.Today.AddDays(1); 
                        Hasta = DateTime.Today.AddDays(-1);
                    }                        
                }
            }
            catch (Exception)
            {
                Desde = DateTime.Today.AddDays(-7);
                Hasta = DateTime.Now;
            }

            Desde = new DateTime(Desde.Year, Desde.Month, Desde.Day, 00, 00, 00);
            Hasta = new DateTime(Hasta.Year, Hasta.Month, Hasta.Day, 23, 59, 59);

            //quitar
            //Desde = DateTime.Parse(objParametro.ObtenerParametro("TimeSheetFechaDesde"));
            //Hasta = DateTime.Parse(objParametro.ObtenerParametro("TimeSheetFechaHasta"));

            Utilidades.LogService("ISSUES DESDE: " + Desde.ToString("dd/M/yyyy", CultureInfo.InvariantCulture) + " HASTA: " + Hasta.ToString("dd/M/yyyy", CultureInfo.InvariantCulture));

            string strPathFile = "";
            string strPathFileEnlacesJira = "";
            string strPathFileJira = "";
            string strPathFileJiraAdic = "";
            string strPathFileJiraAdicGdd = "";
            string strPathFileWorkLogs = "";

            strPathFile = objParametro.ObtenerParametro("Pathfiles");

            strPathFileEnlacesJira = strPathFile + "enlaces.txt";
            strPathFileJira = strPathFile + "jiras.txt";
            strPathFileJiraAdic = strPathFile + "jiras_adic.txt";
            strPathFileJiraAdicGdd = strPathFile + "jiras_adic_gdd.txt";
            strPathFileWorkLogs = strPathFile + "worklogs.txt";

            try
            {

                //File.Delete(strPathFileEnlacesJira);
                File.WriteAllText(strPathFileEnlacesJira, string.Empty);

                //File.Delete(strPathFileJira);
                File.WriteAllText(strPathFileJira, string.Empty);

                //File.Delete(strPathFileJiraAdic);
                File.WriteAllText(strPathFileJiraAdic, string.Empty);

                //File.Delete(strPathFileJiraAdicGdd);
                File.WriteAllText(strPathFileJiraAdicGdd, string.Empty);

                //File.Delete(strPathFileWorkLogs);
                File.WriteAllText(strPathFileWorkLogs, string.Empty);

            }
            catch (Exception ex)
            {
                Utilidades.LogService("Error GetIssuesLite(): " + "ERROR, NO SE PUDO ELIMINAR ARCHIVOS " + " ERROR: " + ex.Message);
                ex.Data.Clear();
            }

            //fecha_desde = DateTime.Parse(objParametro.ObtenerParametro("FecUltimaSinIssuesLite"));

            int Descargado = 0;
            int Total = 0;
            AddAtlassianGotJiraJirasLite.AddJiraLite issues = new AddAtlassianGotJiraJirasLite.AddJiraLite();
            issues = SearchJiraLiteAsync(0, Desde, Hasta);

            Total = issues.total;
            while (Total > 0)
            {
                //Console.WriteLine("startAt: " + startAt);
                await GetIssuesLiteFilter(startAt, Desde, Hasta);
                Descargado = issues.maxResults + Descargado;
                startAt = Descargado;
                if ((Total - Descargado) > 0)
                    Total = Total - Descargado;
                else
                    Total = issues.total - Descargado;
            }

            await GetIssuesImports();

            return Hasta;
        }

        /// <summary>
        /// Retorna los issues que se solicitan manualmente desde la tabla in_jiras_imports
        /// </summary> 
        private async Task GetIssuesImports()
        {
            try 
            {
                AddAtlassianGotJiraJirasLite.AddJiraLite issues = new AddAtlassianGotJiraJirasLite.AddJiraLite();
                MTablasIn objJirasNuevos = new MTablasIn();
                string Jiras_key = objJirasNuevos.Obtener_JirasKeyActualizar(false);

                Array A_Jiras_key = Jiras_key.Split(',').ToArray();
                var NewIssueKeys = new List<string>((string[])A_Jiras_key);

                foreach (string IssueKey in NewIssueKeys)
                {
                    if (IssueKey != "")
                    {

                        try
                        {
                            issues = new AddAtlassianGotJiraJirasLite.AddJiraLite();
                            issues = SearchJiraLiteAsync(IssueKey);

                            foreach (AddAtlassianGotJiraJirasLite.Issue issue in issues.issues)
                            {

                                // if (IssueKey.Trim() == "EMX3051-179") System.Diagnostics.Debugger.Break();                                                                          
                                if (issue.key != null)
                                {
                                    //SaveIssueToFile((dynamic)issue);
                                    SaveIssueToFile(issue);

                                    //obtengo los enlaces de este jira                        
                                    await GetLinkForIssue(issue.key);

                                    //SE OBTIENEN LAS HORAS DE ESTE JIRA
                                    await GetWorklogAsync(issue.fields.project.key, issue.key, issue.fields.summary, issue.fields.issuetype.name, issue.fields.priority.name);
                                }                                                                                      
                            }
                        }
                        catch (Exception t)
                        {
                            await Task.Run(() => Utilidades.LogService("Error GetIssuesImports(): " + IssueKey + " " + t.Message));
                            t.Data.Clear();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await Task.Run(() => Utilidades.LogService("Error GetIssuesImports(): " + ex.Message));
            }
        }

        /// <summary>
        /// Retorna los issues del dia
        /// </summary>     
        public async Task<DateTime> GetIssuesLiteFilter(int startAt, DateTime fecha_desde, DateTime fecha_hasta)
        {
            DateTime fecha_actual = DateTime.Now;
            try
            {
                if (!this.HorarioValido)
                {
                    return fecha_actual;
                }

                MParametros objParametro = new MParametros();

                //---------------------------------------------- NUEVOS JIRAS ----------------------------------
                //Notificar("Obteniendo Jiras...");

                MTablasIn objJirasNuevos = new MTablasIn();

                AddAtlassianGotJiraJirasLite.AddJiraLite issues = new AddAtlassianGotJiraJirasLite.AddJiraLite();
                issues = SearchJiraLiteAsync(startAt, fecha_desde, fecha_hasta);

                foreach (AddAtlassianGotJiraJirasLite.Issue issue in issues.issues)
                {

                    // if (IssueKey.Trim() == "EMX3051-179") System.Diagnostics.Debugger.Break();                                                
                    try
                    {

                        if (issue.key != null)
                        {
                            //SaveIssueToFile((dynamic)issue);
                            SaveIssueToFile(issue);

                            //obtengo los enlaces de este jira                        
                            await GetLinkForIssue(issue.key);

                            //SE OBTIENEN LAS HORAS DE ESTE JIRA
                            await GetWorklogAsync(issue.fields.project.key, issue.key, issue.fields.summary, issue.fields.issuetype.name, issue.fields.priority.name);
                        }
                            
                    }
                    catch (Exception t)
                    {
                        await Task.Run(() => Utilidades.LogService("Error GetIssuesLiteFilter(): " + issue.key + " " + t.Message));
                        t.Data.Clear();
                    }                                       

                }

                objJirasNuevos = null;

                //Notificar("Jiras Erroneos: " + cantError.ToString());
                //Notificar("Jiras OK: " + cantOk.ToString());
                //---------------------------------------------- FIN NUEVOS JIRAS ----------------------------------

                //Notificar("Jiras Grabados...");

                objParametro = null;
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
                await Task.Run(() => Utilidades.LogService("Error GetIssuesLite() Clase: " + clase + " Método(): " + metodo + " Linea: " + line + " Error: " + ex.Message));
            }

            return fecha_actual;
        }

        private void SaveIssueToFile(/*AddAtlassianGotJiraJiras.AddJira*/ AddAtlassianGotJiraJirasLite.Issue issue)
        {
            //AddAtlassianGotJiraJiras.AddJira issue3 = new AddAtlassianGotJiraJiras.AddJira();
            //AddAtlassianGotJiraJirasLite.Issue issue4 = new AddAtlassianGotJiraJirasLite.Issue();                        

            MParametros objParametro = new MParametros();
            string strPathFile = "";
            string strPathFileJira = "";
            string strPathFileJiraAdic = "";
            string strPathFileJiraAdicGdd = "";

            strPathFile = objParametro.ObtenerParametro("Pathfiles");

            strPathFileJira = strPathFile + "jiras.txt";
            strPathFileJiraAdic = strPathFile + "jiras_adic.txt";
            strPathFileJiraAdicGdd = strPathFile + "jiras_adic_gdd.txt";

            if (issue.key == null) return;
            string Grupo_De_Actividad = "";
            if (issue.fields?.customfield_18768 != null)
            {
                try { Grupo_De_Actividad = issue.fields?.customfield_18768?.value?.Trim(); } catch (Exception x) { x.Data.Clear(); }
            }
            if (Grupo_De_Actividad == null) Grupo_De_Actividad = "";

            string Origen_Del_Error = "";
            if (issue.fields?.customfield_18715 != null)
            {
                try { Origen_Del_Error = issue.fields?.customfield_18715?.value?.Trim(); } catch (Exception x) { x.Data.Clear(); }
            }
            if (Origen_Del_Error == null) Origen_Del_Error = "";

            string Fase_Detectado = "";
            if (issue.fields?.customfield_15000 != null)
            {
                try { Fase_Detectado = issue.fields?.customfield_15000?.value?.Trim(); } catch (Exception x) { x.Data.Clear(); }
            }
            if (Fase_Detectado == null) Fase_Detectado = "";

            //pedido por Analia (CUSTOM_FIELD)
            string Cliente = "";
            if (issue.fields?.customfield_10501 != null)
            {
                try { Cliente = issue.fields.customfield_10501?.value.Trim(); } catch (Exception x) { x.Data.Clear(); }
            }
            if (Cliente == null) Cliente = "";

            string Epic_Link = "";
            if (issue.fields?.customfield_10008 != null)
            {
                try { Epic_Link = issue.fields.customfield_10008?.Trim(); } catch (Exception x) { x.Data.Clear(); }
            }
            if (Epic_Link == null) Epic_Link = "";

            string Sprint = "";
            try
            {
                //string tmpSprint = "";
                //Array A_obj = issue.fields.customfield_10007[0].Split(',').ToArray();
                //var NewA_obj = new List<string>((string[])A_obj);
                //tmpSprint = NewA_obj[3];
                //A_obj = tmpSprint.Split('=').ToArray();
                //NewA_obj = new List<string>((string[])A_obj);
                //Sprint = NewA_obj[1].Trim();
                if (issue.fields?.customfield_10007 != null)
                {
                    foreach (var sprint in issue.fields?.customfield_10007)
                    {
                        Sprint = sprint.name;
                    }
                }
            }
            catch (Exception x) { x.Data.Clear(); }
            if (Sprint == null) Sprint = "";

            string ParentIssueKey = "";
            try { ParentIssueKey = issue?.fields?.parent?.key?.Trim(); } catch (Exception x) { x.Data.Clear(); }
            if (ParentIssueKey == null) ParentIssueKey = "";

            //Antes se llamaba Marco Producto
            //pedido por Analia (CUSTOM_FIELD)
            string Categoria = "";
            if (issue.fields?.customfield_18758 != null)
            {
                try { Categoria = issue?.fields?.customfield_18758?.value?.Trim(); } catch (Exception x) { x.Data.Clear(); }
            }
            if (Categoria == null) Categoria = "";

            //pedido por Analia (CUSTOM_FIELD)
            string Proyecto_custom = "";
            if (issue.fields?.customfield_18792 != null)
            {
                try { Proyecto_custom = issue?.fields?.customfield_18792?.value?.Trim(); } catch (Exception x) { x.Data.Clear(); }
            }
            if (Proyecto_custom == null) Proyecto_custom = "";

            //pedido por Analia
            string Prioridad = "";
            try { Prioridad = issue?.fields?.priority?.name?.Trim(); } catch (Exception x) { x.Data.Clear(); }
            if (Prioridad == null) Prioridad = "";

            //pedido por Analia
            string HorasEstimadas = "";
            try { HorasEstimadas = CalcularTiempo(issue?.fields?.timetracking?.originalEstimateSeconds).ToString(); } catch (Exception x) { x.Data.Clear(); }
            if (HorasEstimadas == "0.00") try { HorasEstimadas = issue?.fields?.customfield_10306?.ToString(); } catch (Exception x) { x.Data.Clear(); }
            if (HorasEstimadas == null) HorasEstimadas = "0.00";

            //pedido por Analia (CUSTOM_FIELD)
            string StoryPoints = "";
            if (issue.fields?.customfield_10004 != null)
            {
                try { StoryPoints = issue?.fields?.customfield_10004?.ToString(); } catch (Exception x) { x.Data.Clear(); }
            }
            if (StoryPoints == null) StoryPoints = "";

            //pedido por Analia (CUSTOM_FIELD)
            string Features = "";
            if (issue.fields?.customfield_18714 != null)
            {
                try { Features = issue?.fields?.customfield_18714?.ToString(); } catch (Exception x) { x.Data.Clear(); }
            }
            if (Features == null) Features = "";

            //pedido por Fede Agostino (CUSTOM_FIELD)
            string AreaSolicitante = "";
            if (issue.fields?.customfield_12100 != null)
            {
                try { AreaSolicitante = issue?.fields?.customfield_12100?.value?.ToString(); } catch (Exception x) { x.Data.Clear(); }
            }
            if (AreaSolicitante == null) AreaSolicitante = "";

            //pedido por Fede Agostino (CUSTOM_FIELD)
            string Informador = "";
            try { Informador = issue?.fields?.reporter?.displayName?.ToString(); } catch (Exception x) { x.Data.Clear(); }
            if (Informador == null) Informador = "";

            //pedido por Analia (CUSTOM_FIELD)
            string Calificacion = "";
            if (issue.fields?.customfield_18500 != null)
            {
                try { Calificacion = issue?.fields?.customfield_18500?.ToString(); } catch (Exception x) { x.Data.Clear(); }
            }
            if (Calificacion == null) Calificacion = "";

            // pedido por Viviana (CUSTOM_FIELD)
            string Origen = "";
            if (issue.fields?.customfield_18838 != null)
            {
                try { Origen = issue?.fields?.customfield_18838?.value?.ToString(); } catch (Exception x) { x.Data.Clear(); }
            }
            if (Origen == null) Origen = "";

            // pedido por Viviana (CUSTOM_FIELD)
            string Equipo = "";
            if (issue.fields?.customfield_18837 != null)
            {
                try { Equipo = issue?.fields?.customfield_18837?[0]?.value?.ToString(); } catch (Exception x) { x.Data.Clear(); }
            }
            if (Equipo == null) Equipo = "";

            // pedido por Viviana (CUSTOM_FIELD)
            string Componente = "";
            if (issue.fields?.customfield_18834 != null)
            {
                try { Componente = issue?.fields?.customfield_18834?.ToString(); } catch (Exception x) { x.Data.Clear(); }
            }
            if (Componente == null) Componente = "";

            // pedido por Viviana (CUSTOM_FIELD)                                
            string Facturable = "";
            if (issue.fields?.customfield_18840 != null)
            {
                try { Facturable = issue?.fields?.customfield_18840?.value?.ToString(); } catch (Exception x) { x.Data.Clear(); }
            }
            if (Facturable == null) Facturable = "";

            // pedido por Viviana (CUSTOM_FIELD)
            string FechaEstimacion = "";
            if (issue.fields?.customfield_18841 != null)
            {
                try { FechaEstimacion = issue?.fields.customfield_18841?.ToString(); } catch (Exception x) { x.Data.Clear(); }
            }
            if (FechaEstimacion == null) FechaEstimacion = "";

            // pedido por Viviana (CUSTOM_FIELD)
            string Producto = "";
            if (issue.fields?.customfield_18833 != null)
            {
                try { Producto = issue?.fields?.customfield_18833[0]?.value?.ToString(); } catch (Exception x) { x.Data.Clear(); }
            }
            if (Producto == null) Producto = "";

            // pedido por Viviana (CUSTOM_FIELD)
            string Version = "";
            if (issue.fields?.customfield_18844 != null)
            {
                try { Version = issue?.fields?.customfield_18844[0]?.value?.ToString(); } catch (Exception x) { x.Data.Clear(); }
            }
            if (Version == null) Version = "";


            // pedido por Viviana (CUSTOM_FIELD)
            string Tipo = "";
            if (issue.fields?.customfield_18845 != null)
            {
                try { Tipo = issue?.fields?.customfield_18845[0]?.value?.ToString(); } catch (Exception x) { x.Data.Clear(); }
            }
            if (Tipo == null) Tipo = "";

            // pedido por Viviana (CUSTOM_FIELD)
            string TipoDesarrollo = "";
            if (issue.fields?.customfield_18839 != null)
            {
                try { TipoDesarrollo = issue?.fields?.customfield_18839.value?.ToString(); } catch (Exception x) { x.Data.Clear(); }
            }
            if (TipoDesarrollo == null) TipoDesarrollo = "";

            // pedido por Viviana (CUSTOM_FIELD)
            string EstadoDesarrollo = "";
            if (issue.fields?.customfield_18832 != null)
            {
                try { EstadoDesarrollo = issue?.fields?.customfield_18832?.value?.ToString(); } catch (Exception x) { x.Data.Clear(); }
            }
            if (EstadoDesarrollo == null) EstadoDesarrollo = "";

            // pedido por Viviana (CUSTOM_FIELD)
            string TipoServicio = "";
            if (issue.fields?.customfield_17700 != null)
            {
                try { TipoServicio = issue?.fields.customfield_17700?.ToString(); } catch (Exception x) { x.Data.Clear(); }
            }
            if (TipoServicio == null) TipoServicio = "";

            // pedido por Viviana (CUSTOM_FIELD)
            string FechaEntrega = "";
            if (issue.fields?.customfield_18835 != null)
            {
                try { FechaEntrega = issue?.fields?.customfield_18835?.ToString(); } catch (Exception x) { x.Data.Clear(); }
            }
            if (FechaEntrega == null) FechaEntrega = "";

            // pedido por Viviana (CUSTOM_FIELD)
            string FechaRequerida = "";
            if (issue.fields?.customfield_18836 != null)
            {
                try { FechaRequerida = issue?.fields?.customfield_18836?.ToString(); } catch (Exception x) { x.Data.Clear(); }
            }
            if (FechaRequerida == null) FechaRequerida = "";

            // pedido por Diego Paez
            string TipoSolicitudMDA = "";
            if (issue.fields?.customfield_18736 != null)
            {
                try
                {
                    TipoSolicitudMDA = issue.fields.customfield_18736.requestType?.name;
                }
                catch (Exception)
                {
                    TipoSolicitudMDA = "";
                }

            }

            if (TipoSolicitudMDA == null || TipoSolicitudMDA == "")
            {
                //Pedido por Analia
                if (issue.fields?.customfield_18898 != null)
                    TipoSolicitudMDA = issue.fields.customfield_18898.value;
                else
                    TipoSolicitudMDA = "";
            }

            if (TipoSolicitudMDA == null || TipoSolicitudMDA == "")
            {
                //Pedido por Analia
                if (issue.fields?.customfield_18931 != null)
                    TipoSolicitudMDA = issue.fields.customfield_18931.value;
                else
                    TipoSolicitudMDA = "";
            }
            if (TipoSolicitudMDA == null) TipoSolicitudMDA = "";

            //pedido por Analia (CUSTOM_FIELD)
            //string PersonasCapacitar = "";
            //try
            //{
            //    foreach (var per in issue.fields.customfield_13804)
            //    {
            //        try { PersonasCapacitar = per.displayName.ToString(); } catch (Exception x) { x.Data.Clear(); }
            //        break;
            //    }
            //}
            //catch (Exception x)                            {

            //    x.Data.Clear();
            //}                            
            //if (PersonasCapacitar == null) PersonasCapacitar = "";


            // pedido por Fede Agostino
            string CustomCategoria = "";
            if (issue.fields?.customfield_18903 != null)
            {
                CustomCategoria = issue.fields.customfield_18903.value;
            }
            if (CustomCategoria == null) CustomCategoria = "";

            //pedido por Analia (CUSTOM_FIELD)
            string Assignee = "";
            if (issue?.fields?.assignee?.displayName != null)
            {
                try { Assignee = issue?.fields?.assignee?.displayName.ToString(); } catch (Exception x) { x.Data.Clear(); }
            }
            if (Assignee == null) Assignee = "";

            //pedido por Analia (CUSTOM_FIELD)
            string TipoFalla = "";
            if (issue.fields?.customfield_18908 != null)
            {
                TipoFalla = issue.fields.customfield_18908.value;
            }
            if (TipoFalla == null) TipoFalla = "";

            //pedido por Analia (CUSTOM_FIELD)
            string MotivoDePendiente = "";
            if (issue.fields?.customfield_18907 != null)
            {
                MotivoDePendiente = issue.fields.customfield_18907.value;
            }
            if (MotivoDePendiente == null) MotivoDePendiente = "";

            //pedido por Analia (CUSTOM_FIELD)
            string SLA1 = "";
            if (issue.fields?.customfield_18800 != null)
            {
                if (issue.fields.customfield_18800.completedCycles != null)
                    if (issue.fields.customfield_18800.completedCycles.Count > 0)
                    {
                        try { SLA1 = issue.fields.customfield_18800.completedCycles[0].remainingTime.friendly; } catch (Exception x) { x.Data.Clear(); }
                    }
                //else if (issue.fields.customfield_18800.ongoingCycle?.remainingTime?.friendly != null)
                //{
                //    try { SLA1 = issue.fields.customfield_18800.ongoingCycle.remainingTime.friendly; } catch (Exception x) { x.Data.Clear(); }
                //}

            }
            if (SLA1 == null) SLA1 = "";

            //pedido por Analia (CUSTOM_FIELD)
            string SLA2 = "";
            if (issue.fields?.customfield_18918 != null)
            {
                if (issue.fields.customfield_18918.completedCycles != null)
                    if (issue.fields.customfield_18918.completedCycles.Count > 0)
                    {
                        try { SLA2 = issue.fields.customfield_18918.completedCycles[0].remainingTime.friendly; } catch (Exception x) { x.Data.Clear(); }
                    }
                    else if (issue.fields.customfield_18918.ongoingCycle?.remainingTime?.friendly != null)
                    {
                        try { SLA2 = issue.fields.customfield_18918.ongoingCycle.remainingTime.friendly; } catch (Exception x) { x.Data.Clear(); }
                    }
            }
            if (SLA2 == null) SLA2 = "";

            //pedido por Analia (CUSTOM_FIELD)
            string SLA3 = "";
            if (issue.fields?.customfield_18799 != null)
            {
                if (issue.fields.customfield_18799.completedCycles != null)
                    if (issue.fields.customfield_18799.completedCycles.Count > 0)
                    {
                        try { SLA3 = issue.fields.customfield_18799.completedCycles[0].remainingTime.friendly; } catch (Exception x) { x.Data.Clear(); }
                    }
                    else if (issue.fields.customfield_18799.ongoingCycle?.remainingTime?.friendly != null)
                    {
                        try { SLA3 = issue.fields.customfield_18799.ongoingCycle.remainingTime.friendly; } catch (Exception x) { x.Data.Clear(); }
                    }
            }
            if (SLA3 == null) SLA3 = "";

            //pedido por Analia (CUSTOM_FIELD)
            string SLA4 = "";
            if (issue.fields?.customfield_18895 != null)
            {
                if (issue.fields.customfield_18895.ongoingCycle != null)
                    if (issue.fields.customfield_18895.ongoingCycle?.remainingTime?.friendly != null)
                    {
                        try { SLA4 = issue.fields.customfield_18895.ongoingCycle.remainingTime.friendly; } catch (Exception x) { x.Data.Clear(); }
                    }
            }
            if (SLA4 == "")
                if (issue.fields?.customfield_18895 != null)
                {
                    if (issue.fields.customfield_18895.completedCycles != null)
                        if (issue.fields.customfield_18895.completedCycles.Count > 0)
                        {
                            try { SLA4 = issue.fields.customfield_18895.completedCycles[0].remainingTime.friendly; } catch (Exception x) { x.Data.Clear(); }
                        }
                        else if (issue.fields.customfield_18895.ongoingCycle?.remainingTime?.friendly != null)
                        {
                            try { SLA4 = issue.fields.customfield_18895.ongoingCycle.remainingTime.friendly; } catch (Exception x) { x.Data.Clear(); }
                        }
                }

            if (SLA4 == null) SLA4 = "";

            // pedido por Fede (CUSTOM_FIELD 18911)
            string Origen2 = "";
            if (issue.fields?.customfield_18911 != null)
            {
                try { Origen2 = issue.fields.customfield_18911?.value?.ToString(); } catch (Exception x) { x.Data.Clear(); }
            }
            if (Origen2 == null) Origen2 = "";


            // pedido por Fede (CUSTOM_FIELD 18926)
            string NroOportunidad = "";
            if (issue.fields?.customfield_18926 != null)
            {
                try { NroOportunidad = issue.fields.customfield_18926?.ToString(); } catch (Exception x) { x.Data.Clear(); }
            }
            if (NroOportunidad == null) NroOportunidad = "";

            // pedido por Fede (CUSTOM_FIELD 18910)
            string TipoSoporte = "";
            if (issue.fields?.customfield_18910 != null)
            {
                try { TipoSoporte = issue.fields.customfield_18910?.value.ToString(); } catch (Exception x) { x.Data.Clear(); }
            }
            if (TipoSoporte == null) TipoSoporte = "";

            // pedido por Fede (CUSTOM_FIELD 18914)
            string CausaRaiz = "";
            if (issue.fields?.customfield_18914 != null)
            {
                try { CausaRaiz = issue.fields.customfield_18914?.value.ToString(); } catch (Exception x) { x.Data.Clear(); }
            }
            if (CausaRaiz == null) CausaRaiz = "";

            // pedido por Fede (CUSTOM_FIELD 18915)
            string NivelResolucion = "";
            if (issue.fields?.customfield_18915 != null)
            {
                try { NivelResolucion = issue.fields.customfield_18915?.value.ToString(); } catch (Exception x) { x.Data.Clear(); }
            }
            if (NivelResolucion == null) NivelResolucion = "";

            // pedido por Fede (CUSTOM_FIELD 18878)
            string TipoRiesgo = "";
            if (issue.fields?.customfield_18878 != null)
            {
                try { TipoRiesgo = issue.fields.customfield_18878?.value.ToString(); } catch (Exception x) { x.Data.Clear(); }
            }
            if (TipoRiesgo == null) TipoRiesgo = "";

            // pedido por Fede (CUSTOM_FIELD 13200)
            string ProcesoVinculado = "";
            if (issue.fields?.customfield_13200 != null)
            {
                try { ProcesoVinculado = issue.fields.customfield_13200?.value.ToString(); } catch (Exception x) { x.Data.Clear(); }
            }
            if (ProcesoVinculado == null) ProcesoVinculado = "";

            // pedido por Fede (CUSTOM_FIELD 18880)
            string Probabilidad = "";
            if (issue.fields?.customfield_18880 != null)
            {
                try { Probabilidad = issue.fields.customfield_18880?.value.ToString(); } catch (Exception x) { x.Data.Clear(); }
            }
            if (Probabilidad == null) Probabilidad = "";

            // pedido por Fede (CUSTOM_FIELD 18881)
            string ImpactoRyo = "";
            if (issue.fields?.customfield_18881 != null)
            {
                try { ImpactoRyo = issue.fields.customfield_18881?.value.ToString(); } catch (Exception x) { x.Data.Clear(); }
            }
            if (ImpactoRyo == null) ImpactoRyo = "";

            // pedido por Fede (CUSTOM_FIELD 18882)
            string EstrategiaRyo = "";
            if (issue.fields?.customfield_18882 != null)
            {
                try { EstrategiaRyo = issue.fields.customfield_18882?.value.ToString(); } catch (Exception x) { x.Data.Clear(); }
            }
            if (EstrategiaRyo == null) EstrategiaRyo = "";

            // pedido por Fede (CUSTOM_FIELD 18872)
            string Impacto = "";
            if (issue.fields?.customfield_18872 != null)
            {
                try { Impacto = issue.fields.customfield_18872?.value.ToString(); } catch (Exception x) { x.Data.Clear(); }
            }
            if (Impacto == null) Impacto = "";

            // pedido por Fede (CUSTOM_FIELD 18873)
            string Influencia = "";
            if (issue.fields?.customfield_18873 != null)
            {
                try { Influencia = issue.fields.customfield_18873?.value.ToString(); } catch (Exception x) { x.Data.Clear(); }
            }
            if (Influencia == null) Influencia = "";

            // pedido por Fede (CUSTOM_FIELD 18874)
            string Estrategia = "";
            if (issue.fields?.customfield_18874 != null)
            {
                try { Estrategia = issue.fields.customfield_18874?.value.ToString(); } catch (Exception x) { x.Data.Clear(); }
            }
            if (Estrategia == null) Estrategia = "";

            // pedido por Fede (CUSTOM_FIELD 18876)
            string ClasificacionContexto = "";
            if (issue.fields?.customfield_18876 != null)
            {
                try { ClasificacionContexto = issue.fields.customfield_18876?.value.ToString(); } catch (Exception x) { x.Data.Clear(); }
            }
            if (ClasificacionContexto == null) ClasificacionContexto = "";

            // pedido por Fede (CUSTOM_FIELD 18875)
            string Origen_2 = "";
            if (issue.fields?.customfield_18875 != null)
            {
                try { Origen_2 = issue.fields.customfield_18875?.value.ToString(); } catch (Exception x) { x.Data.Clear(); }
            }
            if (Origen_2 == null) Origen_2 = "";

            // pedido por Fede (CUSTOM_FIELD 18782)
            string OrigenHallazgo = "";
            if (issue.fields?.customfield_18782 != null)
            {
                try { OrigenHallazgo = issue.fields.customfield_18782?.value.ToString(); } catch (Exception x) { x.Data.Clear(); }
            }
            if (OrigenHallazgo == null) OrigenHallazgo = "";

            // pedido por Fede (CUSTOM_FIELD 13111)
            string AccionCorrectivaCuando = "";
            if (issue.fields?.customfield_13111 != null)
            {
                try { AccionCorrectivaCuando = issue.fields.customfield_13111?.ToString(); } catch (Exception x) { x.Data.Clear(); }
            }
            if (AccionCorrectivaCuando == null) AccionCorrectivaCuando = "";

            // pedido por Fede (CUSTOM_FIELD 14601)
            string FechaVerificacionEficacia = "";
            if (issue.fields?.customfield_14601 != null)
            {
                try { FechaVerificacionEficacia = issue.fields.customfield_14601?.ToString(); } catch (Exception x) { x.Data.Clear(); }
            }
            if (FechaVerificacionEficacia == null) FechaVerificacionEficacia = "";

            // pedido por Analia (CUSTOM_FIELD 12401)
            string Complejidad = "";
            if (issue.fields?.customfield_12402 != null)
            {
                //try {
                //    issue.fields.customfield_12402.ForEach(complejidad =>
                //    {
                //        Complejidad = complejidad.value;
                //        Complejidad = Complejidad + "|";
                //    });
                //    Complejidad = Complejidad.Substring(0, Complejidad.Length - 1);
                //} 
                //catch (Exception x) { x.Data.Clear(); }
                try { Complejidad = issue.fields.customfield_12402?.value.ToString(); } catch (Exception x) { x.Data.Clear(); }
            }
            if (Complejidad == null) Complejidad = "";


            // pedido por Analia (customfield_19130)    FALTA RESOLVER
            string SoporteAsignadoA = "";
            try
            {
                if(issue.fields.customfield_19130 != null)
                {
                    issue.fields.customfield_19130.ForEach(sa =>
                    {
                        SoporteAsignadoA = sa.value;
                        SoporteAsignadoA = SoporteAsignadoA + "|";
                    });
                    SoporteAsignadoA = SoporteAsignadoA.Substring(0, SoporteAsignadoA.Length - 1);
                }                
            }
            catch (Exception x) { x.Data.Clear(); }

            //if (issue.fields?.customfield_19130 != null)
            //{                
            //    try { SoporteAsignadoA = issue.fields.customfield_19130?.value.ToString(); } catch (Exception x) { x.Data.Clear(); }
            //}
            //if (SoporteAsignadoA == null) SoporteAsignadoA = "";

            // pedido por Analia (customfield_18960)


            string Resolutor = "";
            if (issue.fields?.customfield_18960 != null)
            {
                try { Resolutor = issue.fields.customfield_18960?.displayName.ToString(); } catch (Exception x) { x.Data.Clear(); }
            }
            if (Resolutor == null) Resolutor = "";

            // pedido por Analia (customfield_18849)
            string Ambiente = "";
            if (issue.fields?.customfield_18849 != null)
            {
                try { Ambiente = issue.fields.customfield_18849?.value.ToString(); } catch (Exception x) { x.Data.Clear(); }
            }
            if (Ambiente == null) Ambiente = "";

            // pedido por Analia (customfield_18916)
            string Entorno = "";
            if (issue.fields?.customfield_18916 != null)
            {
                try { Entorno = issue.fields.customfield_18916?.value.ToString(); } catch (Exception x) { x.Data.Clear(); }
            }
            if (Entorno == null) Entorno = "";


            // pedido por Analia (customfield_18998)
            string InformacionInsuficiente = "";
            if (issue.fields?.customfield_18998 != null)
            {
                try { InformacionInsuficiente = issue.fields.customfield_18998?.value.ToString(); } catch (Exception x) { x.Data.Clear(); }
            }
            if (InformacionInsuficiente == null) InformacionInsuficiente = "";

            // pedido por Analia (customfield_19031)
            string PriorizadoGPD = "";
            if (issue.fields?.customfield_19031 != null)
            {
                try { PriorizadoGPD = issue.fields.customfield_19031?.value.ToString(); } catch (Exception x) { x.Data.Clear(); }
            }
            if (PriorizadoGPD == null) PriorizadoGPD = "";

            // pedido por Analia (customfield_19064) FALTA RESOLVER
            string IndicarSiEsCore = "";
            try
            {
                if(issue.fields.customfield_19064 != null)
                {
                    issue.fields.customfield_19064.ForEach(isec =>
                    {
                        IndicarSiEsCore = isec.value;
                        IndicarSiEsCore = IndicarSiEsCore + "|";
                    });
                    IndicarSiEsCore = IndicarSiEsCore.Substring(0, IndicarSiEsCore.Length - 1);
                }                
            }
            catch (Exception x) { x.Data.Clear(); }

            //if (issue.fields?.customfield_19064 != null)
            //{
            //    try { IndicarSiEsCore = issue.fields.customfield_19064?.value.ToString(); } catch (Exception x) { x.Data.Clear(); }
            //}
            //if (IndicarSiEsCore == null) IndicarSiEsCore = "";


            // pedido por Analia (customfield_19097) FALTA RESOLVER
            string JiraControlInterno = "";
            if (issue.fields.customfield_19097 != null)
            {
                issue.fields.customfield_19097.ForEach(jci =>
                {
                    JiraControlInterno = jci.value;
                    JiraControlInterno = JiraControlInterno + "|";
                });
                JiraControlInterno = JiraControlInterno.Substring(0, JiraControlInterno.Length - 1);
            }

            //if (issue.fields?.customfield_19097 != null)
            //{
            //    try { JiraControlInterno = issue.fields.customfield_19097?.value.ToString(); } catch (Exception x) { x.Data.Clear(); }
            //}
            //if (JiraControlInterno == null) JiraControlInterno = "";

            //datos adicionales del jira
            try
            {
                DateTime? Created = new DateTime();
                DateTime? Updated = new DateTime();
                DateTime? Resolutiondate = new DateTime();
                DateTime? LastViewed = new DateTime();
                Created = issue?.fields?.created;
                Updated = issue?.fields?.updated;
                Resolutiondate = issue?.fields?.resolutiondate;
                LastViewed = issue?.fields?.lastViewed;

                Utilidades.SaveFile(strPathFileJiraAdic, issue.key + "|" + Created + "|" + Updated + "|" + Resolutiondate + "|" + LastViewed + "|" + AreaSolicitante.Replace("|", "") + "|" + Informador + "|" + Calificacion.Replace("|", "") + "|" + TipoSolicitudMDA + "|" + CustomCategoria + "|" + Assignee + "|" + TipoFalla + "|" + MotivoDePendiente + "|" + SLA1 + "|" + SLA2 + "|" + SLA3 + "|" + SLA4 + "|" + Cliente + "|" + Origen2 + "|" + Producto + "|" + NroOportunidad + "|" + TipoSoporte + "|" + CausaRaiz + "|" + NivelResolucion + "|" + TipoRiesgo + "|" + ProcesoVinculado + "|" + Probabilidad + "|" + ImpactoRyo + "|" + EstrategiaRyo + "|" + Impacto + "|" + Influencia + "|" + Estrategia + "|" + ClasificacionContexto + "|" + Origen_2 + "|" + OrigenHallazgo + "|" + AccionCorrectivaCuando + "|" + FechaVerificacionEficacia + "|" + Complejidad + "|" + SoporteAsignadoA + "|" + Resolutor + "|" + Ambiente + "|" + Entorno + "|" + InformacionInsuficiente + "|" + PriorizadoGPD + "|" + IndicarSiEsCore + "|" + JiraControlInterno);
            }
            catch (Exception F)
            {
                F.Data.Clear();
            }

            try
            {
                //solo jiras para este proyecto
                if (issue?.key?.Substring(0, 7) == "EMX3051")
                {
                    Utilidades.SaveFile(strPathFileJiraAdicGdd, issue.key + "|" + Componente + "|" + Cliente + "|" + Origen + "|" + Equipo + "|" + Facturable + "|" + Producto + "|" + Version + "|" + Tipo + "|" + HorasEstimadas + "|" + TipoDesarrollo + "|" + EstadoDesarrollo + "|" + FechaRequerida + "|" + FechaEstimacion + "|" + FechaEntrega);
                }
            }
            catch (Exception F)
            {
                F.Data.Clear();
            }

            Utilidades.SaveFile(strPathFileJira, issue.fields.project.key + "|" + issue.key + "|" + issue.fields.issuetype.name
                    + "|" + issue.fields.summary.Replace("|", "").Trim() + "|" + Epic_Link.Replace("|", "") + "|" + Grupo_De_Actividad.Replace("|", "") + "|" + Sprint.Replace("|", "") + "|" + ParentIssueKey
                    + "|" + Origen_Del_Error.Replace("|", "") + "|" + Fase_Detectado.Replace("|", "") + "|" + Cliente.Replace("|", "") + "|" + issue.fields.status.name + "|" + Prioridad + "|" + Categoria.Replace("|", "") + "|" + Proyecto_custom.Replace("|", "")
                    + "|" + HorasEstimadas + "|" + StoryPoints.Replace(",", ".") + "|" + Features.Replace("|", ""));

        }
        private bool EsHorarioValido(DateTime FecProximaEjec)
        {
            // SABER SI ES HORARIO VALIDO PARA EJECUTAR EL SERVICIO DE LUNES A SABADOS
            DateTime datetime = FecProximaEjec;

            int HoraDesde = this.HoraDesde;
            int HoraHasta = this.HoraHasta;

            int horaActual = datetime.Hour;
            int diaActual = (int)datetime.DayOfWeek;

            return horaActual >= HoraDesde && horaActual <= HoraHasta && diaActual > 0 && diaActual <= 5; /* esta en (6), antes corria los sabados, pero ahora se apagan los servers, todo lo que era sabado se paso a viernes */
        }

        private string Formating(int valor)
        {
            string s_return = valor.ToString();
            if (valor < 10)
            {
                s_return = "0" + valor;
            }
            return s_return;
        }

        private void ActualizarRol()
        {
            try
            {
                MTablasExt objTablasExt = new MTablasExt();
                objTablasExt.ActualizarRol();
                objTablasExt = null;
            }
            catch (Exception F)
            {
                F.Data.Clear();
            }
        }

        private void PrecalculoIndicadoresProyecto()
        {
            try
            {
                MTablasExt objTablasExt = new MTablasExt();
                objTablasExt.PrecalculoIndicadoresProyecto();
                objTablasExt = null;
            }
            catch (Exception F)
            {
                F.Data.Clear();
            }
        }

        private String CalcularTiempo(long? tsegundos = 0)
        {
            string _return = "";
            try
            {
                long? horas = (tsegundos / 3600);
                long? minutos = ((tsegundos - horas * 3600) / 60);
                // long segundos = tsegundos - (horas * 3600 + minutos * 60);
                _return = horas.ToString() + "." + ((minutos < 10) ? "0" + minutos.ToString() : minutos.ToString()); // + ":" + segundos.ToString();
            }
            catch (Exception)
            {
                _return = "0.00";
            }
            return _return;
        }

        //public async Task SincronizaEnlaces()
        //{
        //    try
        //    {
        //        //Notificar("Sincronización de Enlaces Inicio");
        //        MTablasIn objJirasEnlaces = new MTablasIn();
        //        string Jiras_key = objJirasEnlaces.Obtener_JirasAEnlazar();
        //        if (Jiras_key == "")
        //        {
        //            //Notificar("Sincronización de Enlaces: No se obtuvieron Jiras");
        //        }
        //        else
        //        {
        //            Array A_Jiras_key = Jiras_key.Split(',').ToArray();
        //            var IssueKeys = new List<string>((string[])A_Jiras_key);
        //            await GetLinkForIssues(IssueKeys);
        //            objJirasEnlaces = null;
        //        }
        //        //Notificar("Sincronización de Enlaces Finalizado");

        //    }
        //    catch (Exception ex)
        //    {
        //        //Notificar("Sincronización de Enlaces Error: " + ex.Message);
        //        throw ex;
        //    }

        //}

        private async Task<AddIssueLnk> GetLinksForIssueAsync(string IssueKey)
        {
            var client = new RestClient("https://softoffice.atlassian.net/rest/api/2/issue/" + IssueKey + "?fields=issuelinks,created,subtasks,parent");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            //request.AddHeader("Accept", "application/json");
            //request.AddHeader("Content-Type", "application/json");
            //request.AddHeader("Authorization", "Basic amlyYS5lbXhAZ21haWwuY29tOjVSNlIzVjhiM0hNR1ZmTXFSUUVqMTFDNQ==");
            //request.AddHeader("Cookie", "atlassian.xsrf.token=B5S9-YRZO-3SUG-U7W9_05fa5ac255df7ea7bb7a3f9a0e5367b5a08ada15_lin");
            request.AddHeader("Host", "softoffice.atlassian.net");
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Authorization", $"Basic {authToken}");
            IRestResponse response = await client.ExecuteAsync(request);
            AddIssueLnk LxI = new AddIssueLnk();

            if (response.IsSuccessful)
            {
                var result = response.Content;

                try
                {
                    LxI = JsonConvert.DeserializeObject<AddIssueLnk>(result);
                }
                catch (Exception ex)
                {
                    Utilidades.LogService("Error GetLinksForIssueAsync() " + IssueKey + ": " + ex.Message);
                }
            }

            return LxI;
        }

        private async Task DeleteWorklogsAndHoursAsync(long FechaMiliSegundos)
        {          
            var resource = String.Format("https://softoffice.atlassian.net/rest/api/3/worklog/deleted?since={0}&startAt=0&maxResults=5000&fields=worklogId", FechaMiliSegundos);

            var client = new RestClient(resource);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Host", "softoffice.atlassian.net");
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Authorization", $"Basic {authToken}");
            IRestResponse response = await client.ExecuteAsync(request);
            Add_Worklogs_Deleted Wlk = new Add_Worklogs_Deleted();
            MTablasIn objTablasIn = new MTablasIn();
            if (response.IsSuccessful)
            {
                var result = response.Content;

                try
                {
                    Wlk = JsonConvert.DeserializeObject<Add_Worklogs_Deleted>(result);
                    foreach (var item in Wlk.values)
                    {
                        objTablasIn.DeleteWklHor(item.worklogId);
                        objTablasIn = new MTablasIn();
                    }

                    objTablasIn = null;
                }
                catch (Exception ex)
                {
                    Utilidades.LogService("Error DeleteWorklogsAndHours(): " + ex.Message);
                }
                //hay mas registros, llamada recursiva
                if (!Wlk.lastPage)
                {
                    await DeleteWorklogsAndHoursAsync(Wlk.until);
                }
            }           
        }

        private async Task GetLinkForIssue(string IssueKey)
        {
            MParametros objParametro = new MParametros();
            AddIssueLnk LxI = new AddIssueLnk();

            string strPathFile = "";
            strPathFile = objParametro.ObtenerParametro("Pathfiles");
            strPathFile = strPathFile + "enlaces.txt";

            try
            {

                LxI = new AddIssueLnk();

                try
                {
                    //busco los links por jira
                    LxI = await GetLinksForIssueAsync(IssueKey.ToString());
                }
                catch (Exception ex)
                {
                    ex.Data.Clear();
                }

                if (LxI.fields == null) return;

                foreach (var issuelinks in LxI.fields?.issuelinks)
                {
                    //Incidencias vinculadas
                    if (issuelinks?.inwardIssue != null)
                    {
                        Utilidades.SaveFile(strPathFile, IssueKey.ToString() + "|" + issuelinks.inwardIssue.key);
                    }

                    //otro jira enlaza a este jira
                    if (issuelinks?.outwardIssue != null)
                    {
                        Utilidades.SaveFile(strPathFile, IssueKey.ToString() + "|" + issuelinks.outwardIssue.key);
                    }
                }
                //subtareas que tiene el jira
                foreach (var subTaks in LxI.fields?.subtasks)
                {
                    if (subTaks?.fields != null)
                    {
                        Utilidades.SaveFile(strPathFile, LxI.key + "|" + subTaks.key);
                    }
                }

                //padre del jira
                if (LxI.fields.parent != null)
                {
                    Utilidades.SaveFile(strPathFile, LxI.fields.parent.key + "|" + IssueKey.ToString());
                }

                LxI = null;
            }
            catch (Exception ex)
            {
                Utilidades.LogService("Error Carga GetLinkForIssue() key: " + IssueKey + " Error: " + ex.Message);
                ex.Data.Clear();
            }

            objParametro = null;
        }

        //private async Task GetLinkForIssues(List<string> IssueKeys)
        //{
        //    IssueKeys = IssueKeys.Distinct().ToList();
        //    MParametros objParametro = new MParametros();

        //    int Cantidad = 0;
        //    string strPathFile = "";
        //    strPathFile = objParametro.ObtenerParametro("Pathfiles");
        //    strPathFile = strPathFile + "enlaces.txt";

        //    //CancellationToken token = default(CancellationToken);
        //    bool eliminoArchivo = false;
        //    AddIssueLnk LxI = new AddIssueLnk();
        //    foreach (string key in IssueKeys)
        //    {
        //        try
        //        {
        //            //issueFunction in hasLinks()
        //            //token = default(CancellationToken);

        //            LxI = new AddIssueLnk();
        //            //busco los links por cada jira
        //            try
        //            {
        //                //LxI = await jiraConn.Links.GetLinksForIssueAsync(key.ToString(), token);
        //                LxI = await GetLinksForIssueAsync(key.ToString());
        //            }
        //            catch (Exception ex)
        //            {
        //                ex.Data.Clear();
        //            }

        //            Cantidad = LxI.fields.issuelinks.Count();
        //            if (Cantidad > 0 && !eliminoArchivo)
        //            {
        //                //File.Delete(strPathFile);
        //                File.WriteAllText(strPathFile, string.Empty);
        //                eliminoArchivo = true;
        //            }

        //            foreach (var issuelinks in LxI.fields.issuelinks)
        //            {
        //                if (issuelinks.outwardIssue != null)
        //                {
        //                    Utilidades.SaveFile(strPathFile, LxI.key + "|" + issuelinks.outwardIssue.key);
        //                }

        //            }

        //            LxI = null;
        //        }
        //        catch (Exception ex)
        //        {
        //            Utilidades.LogService("Error Carga GetLinkForIssue(): " + ex.Message);
        //            ex.Data.Clear();
        //        }
        //    }

        //    Utilidades.LogService("GetLinkForIssue(): FIN");
        //    //MTablasIn objTablasIn = new MTablasIn();
        //    //objTablasIn.Grabar_InEnlaces();
        //    objParametro = null;
        //    //objTablasIn = null;
        //    //return true;
        //}

        public async Task GetUsers()
        {
            try
            {
                MParametros objParametro = new MParametros();

                string strPathFile = "";
                string strPathFileUsers2 = "";
                strPathFile = objParametro.ObtenerParametro("Pathfiles");
                strPathFile = strPathFile + "usuarios.txt";

                strPathFileUsers2 = objParametro.ObtenerParametro("Pathfiles");
                strPathFileUsers2 = strPathFileUsers2 + "usuariosAD.txt";

                IEnumerable<JiraUser> _JiraUser = new List<JiraUser>();

                _JiraUser = await SearchAllUsersAsync(JiraUserStatus.Active, 1000, 0);

                if (_JiraUser.Count() != 0)
                {
                    //File.Delete(strPathFile);
                    File.WriteAllText(strPathFile, string.Empty);
                    //File.Delete(strPathFileUsers2);
                    File.WriteAllText(strPathFileUsers2, string.Empty);
                }

                Active_Directory ad = new Active_Directory();
                List<Active_Directory.UserAD> usersAD = new List<Active_Directory.UserAD>();
                usersAD = ad.GetADUsers();

                //Active_Directory.UserAD user3 = new Active_Directory.UserAD();
                //user3 = ad.GetADUsers().Where(x => x.DisplayName == "usuario_buscar").First();

                bool encontrado = false;
                string _EmailAD = "";
                string _UsrAD = "";
                string _DisplayNameAD = "";
                string _DisplayNameJira = "";

                //Guardo usuarios AD en archivo
                foreach (Active_Directory.UserAD UsrAD in usersAD)
                {
                    if (String.IsNullOrEmpty(UsrAD.DisplayName))
                        _DisplayNameAD = "";
                    else
                        _DisplayNameAD = UsrAD.DisplayName;

                    if (String.IsNullOrEmpty(UsrAD.Email))
                        _EmailAD = "";
                    else
                        _EmailAD = UsrAD.Email;

                    if (String.IsNullOrEmpty(UsrAD.UserName))
                        _UsrAD = "";
                    else
                        _UsrAD = UsrAD.UserName;

                    Utilidades.SaveFile(strPathFileUsers2, _DisplayNameAD + "|" + _EmailAD + "|" + _UsrAD);
                }

                string acount_id = "";

                foreach (JiraUser jira_user in _JiraUser)
                {
                    Uri myUri = new Uri(jira_user.Self);
                    acount_id = System.Web.HttpUtility.ParseQueryString(myUri.Query).Get("accountId");


                    _DisplayNameJira = jira_user.DisplayName;
                    _DisplayNameJira = Utilidades.QuitarAcentos(_DisplayNameJira);

                    foreach (Active_Directory.UserAD UsrAD in usersAD)
                    {

                        if (String.IsNullOrEmpty(UsrAD.DisplayName))
                            _DisplayNameAD = "";
                        else
                            _DisplayNameAD = UsrAD.DisplayName;

                        if (String.IsNullOrEmpty(UsrAD.Email))
                            _EmailAD = "";
                        else
                            _EmailAD = UsrAD.Email;

                        if (String.IsNullOrEmpty(UsrAD.UserName))
                            _UsrAD = "";
                        else
                            _UsrAD = UsrAD.UserName;

                        _DisplayNameAD = Utilidades.QuitarAcentos(_DisplayNameAD);

                        bool EstaEnJira = _DisplayNameJira.Like(_DisplayNameAD + "%");
                        bool EstaEnAd = _DisplayNameAD.Like(_DisplayNameJira + "%");
                        bool EsUsuarioCorrecto = UsuarioValido(_DisplayNameAD, _DisplayNameJira);
                        if ((_DisplayNameJira == "") || (_DisplayNameAD == ""))
                        {
                            EstaEnJira = false;
                            EstaEnAd = false;
                        }

                        if ((_DisplayNameJira + " - emerix" == _DisplayNameAD) || (_DisplayNameJira == _DisplayNameAD) || (EstaEnJira) || (EstaEnAd) || (EsUsuarioCorrecto))
                        {
                            Utilidades.SaveFile(strPathFile, jira_user.DisplayName + "|" + _EmailAD + "|" + _UsrAD + "|" + (jira_user.IsActive == true ? 1 : 0) + "|" + acount_id);
                            encontrado = true;
                            break;
                        }
                    }
                    if (!encontrado)
                    {
                        Utilidades.SaveFile(strPathFile, jira_user.DisplayName + "|" + jira_user.Email + "|" + jira_user.Username + "|" + (jira_user.IsActive == true ? 1 : 0) + "|" + acount_id);
                    }
                    encontrado = false;
                }
                //MTablasIn objTablasIn = new MTablasIn();
                //objTablasIn.Grabar_InUsuariosJira();
                //objTablasIn = null;
                objParametro = null;
                ad = null;

            }
            catch (Exception ex)
            {
                await Task.Run(() => Utilidades.LogService("Error GetUsers(): " + ex.Message));
            }
            finally
            {
                await Task.Run(() => Utilidades.LogService("GetUsers(): FIN"));
            }
        }

        private bool UsuarioValido(string _DisplayNameAD, string _DisplayNameJira)
        {
            bool _return = false;
            int encontrado = 0;
            try
            {
                if (_DisplayNameAD == "" || _DisplayNameJira == "")
                {
                    return false;
                }
                _DisplayNameAD = _DisplayNameAD.Replace("emerix", "");
                _DisplayNameAD = _DisplayNameAD.Replace("-", "");
                _DisplayNameJira = _DisplayNameJira.Replace("emerix", "");
                _DisplayNameJira = _DisplayNameJira.Replace("-", "");

                _DisplayNameAD = _DisplayNameAD.Trim();
                _DisplayNameJira = _DisplayNameJira.Trim();

                if (_DisplayNameAD.Length > _DisplayNameJira.Length)
                {
                    Array A_Usu_Jira = _DisplayNameJira.Split(' ').ToArray();
                    var UsuJira = new List<string>((string[])A_Usu_Jira);

                    Array A_Usu_AD = _DisplayNameAD.Split(' ').ToArray();
                    var UsuAD = new List<string>((string[])A_Usu_AD);

                    foreach (string uj in UsuJira)
                    {
                        foreach (string uad in UsuAD)
                        {
                            if (uj == uad)
                            {
                                encontrado++;
                                break;
                            }
                        }
                    }
                    if ((encontrado > 0) && (encontrado == A_Usu_Jira.Length))
                    {
                        _return = true;
                    }
                }
                else
                {
                    Array A_Usu_Jira = _DisplayNameJira.Split(' ').ToArray();
                    var UsuJira = new List<string>((string[])A_Usu_Jira);

                    Array A_Usu_AD = _DisplayNameAD.Split(' ').ToArray();
                    var UsuAD = new List<string>((string[])A_Usu_AD);

                    foreach (string uad in UsuAD)
                    {
                        foreach (string uj in UsuJira)
                        {
                            if (uad == uj)
                            {
                                encontrado++;
                                break;
                            }
                        }
                    }
                    if ((encontrado > 0) && (encontrado == A_Usu_AD.Length))
                    {
                        _return = true;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }

            return _return;
        }
        /// <summary>
        /// Metodo Propio, retorna el jira solicitado deserializando un json
        /// </summary>
        //private AddAtlassianGotJiraJiras.AddJira SearchJiraAsync(string issueKey)
        //{
        //    AddAtlassianGotJiraJiras.AddJira obj = new AddAtlassianGotJiraJiras.AddJira();
        //    if (issueKey.Trim() == "") return obj;

        //    var client = new RestClient("https://softoffice.atlassian.net/rest/api/2/issue/" + issueKey.Trim());
        //    //var client = new RestClient("https://softoffice.atlassian.net/rest/api/latest/issue/" + issueKey);

        //    var request = new RestRequest(Method.GET);
        //    request.AddHeader("Host", "softoffice.atlassian.net");
        //    request.AddHeader("Cache-Control", "no-cache");
        //    request.AddHeader("Authorization", $"Basic {authToken}");
        //    IRestResponse response = client.Execute(request);

        //    if (response.IsSuccessful)
        //    {
        //        var result = response.Content;

        //        try
        //        {
        //            obj = JsonConvert.DeserializeObject<AddAtlassianGotJiraJiras.AddJira>(result);
        //        }
        //        catch (Exception ex)
        //        {
        //            Utilidades.LogService("Error AddAtlassianGotJira() " + issueKey + ": " + ex.Message);
        //        }
        //    }
        //    else
        //    {
        //        Utilidades.JirasErrores(issueKey, response.Content);
        //        return obj;
        //    }
        //    return obj;
        //}

        /// <summary>
        /// Metodo Propio, retorna el jiras deserializando un json, Filtros: startAt, fecha_desde, fecha_hasta
        /// </summary>
        private AddAtlassianGotJiraJirasLite.AddJiraLite SearchJiraLiteAsync(int startAt, DateTime fecha_desde, DateTime fecha_hasta)
        {
            AddAtlassianGotJiraJirasLite.AddJiraLite obj = new AddAtlassianGotJiraJirasLite.AddJiraLite();

            //fecha_actual_menos_x_minutos = fecha_actual - new TimeSpan(0, 3, 0);                                  

            jiraRestClientSettings.EnableRequestTrace = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            WebRequest request = WebRequest.Create("https://softoffice.atlassian.net/rest/api/3/search");
            //string postData = @"{
            //            " + "\n" +
            //            @"  ""jql"": ""(updated >='fecha_desde' && updated <='fecha_hasta') || (created >='fecha_desde' && created <='fecha_hasta')"",  
            //            " + "\n" +
            //            @"  ""maxResults"": 100,
            //            " + "\n" +
            //            @"  ""startAt"": _startAt,
            //            " + "\n" +
            //            @"  ""fields"":[""id"", ""key"", ""summary"", ""description"", ""project"", ""worklog"" , ""timetracking"",""priority"",""reporter"",""assignee"",""resolutiondate"",""lastViewed"",""parent"",""issuetype"",""status"",""updated"",""created""
            //            " + "\n" +
            //            @"  ""customfield_18768"", ""customfield_18715"",""customfield_15000"",""customfield_10501"",""customfield_10008"",
            //            " + "\n" +
            //            @"  ""customfield_10007"",""customfield_18758"",""customfield_18792"",""customfield_10306"",""customfield_10004"",
            //            " + "\n" +
            //            @"  ""customfield_18714"",""customfield_12100"",""customfield_18500"",""customfield_18838"",""customfield_18837"",
            //            " + "\n" +
            //            @"  ""customfield_18834"",""customfield_18840"",""customfield_18841"",""customfield_18833"",""customfield_18844"",
            //            " + "\n" +
            //            @"  ""customfield_18845"",""customfield_18839"",""customfield_18832"",""customfield_17700"",""customfield_18835"",
            //            " + "\n" +
            //            @"  ""customfield_18836"",""customfield_18736"",""customfield_18898"",""customfield_18931"",""customfield_18903"",
            //            " + "\n" +
            //            @"  ""customfield_18908"",""customfield_18907"",""customfield_18800"",""customfield_18918"",""customfield_18799"",
            //            " + "\n" +
            //            @"  ""customfield_18895"",""customfield_18911"",""customfield_18926"",""customfield_18910"",""customfield_18914"",
            //            " + "\n" +
            //            @"  ""customfield_18915"",""customfield_18878"",""customfield_13200"",""customfield_18880"",""customfield_18881"",
            //            " + "\n" +
            //            @"  ""customfield_18882"",""customfield_18872"",""customfield_18873"",""customfield_18874"",""customfield_18876"",
            //            " + "\n" +
            //            @"  ""customfield_18875"",""customfield_18782"",""customfield_13111"",""customfield_14601""
            //            " + "\n" +
            //            @"  ]
            //            " + "\n" +
            //            @"}
            //            " + "\n" +
            //            @"";
            //postData = postData.Replace("_startAt", startAt.ToString());
            //postData = postData.Replace("fecha_desde", fecha_desde.ToString("yyyy/MM/dd HH:mm", CultureInfo.InvariantCulture));
            //postData = postData.Replace("fecha_hasta", fecha_hasta.ToString("yyyy/MM/dd HH:mm", CultureInfo.InvariantCulture));

            string filter = $@"
                ""jql"": ""(updated >= '{fecha_desde:yyyy/MM/dd HH:mm}' AND updated <= '{fecha_hasta:yyyy/MM/dd HH:mm}') OR (created >= '{fecha_desde:yyyy/MM/dd HH:mm}' AND created <= '{fecha_hasta:yyyy/MM/dd HH:mm}' )""
            ";

            string postData = GetPostDataJira(filter, startAt);

            //Utilidades.LogService("JiraLite URL Issue: " + postData);

            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.Method = "POST";
            //request.Headers.Add("Host", "softoffice.atlassian.net");
            request.Headers.Add("Cache-Control", "no-cache");
            request.Headers.Add("Authorization", $"Basic {authToken}");
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
            obj = JsonConvert.DeserializeObject<AddAtlassianGotJiraJirasLite.AddJiraLite>(result);

            return obj;
        }

        /// <summary>
        /// Metodo Propio, retorna el jiras deserializando un json, Filtros: key
        /// </summary>
        private AddAtlassianGotJiraJirasLite.AddJiraLite SearchJiraLiteAsync(string key)
        {
            AddAtlassianGotJiraJirasLite.AddJiraLite obj = new AddAtlassianGotJiraJirasLite.AddJiraLite();

            //fecha_actual_menos_x_minutos = fecha_actual - new TimeSpan(0, 3, 0);                                  

            jiraRestClientSettings.EnableRequestTrace = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            WebRequest request = WebRequest.Create("https://softoffice.atlassian.net/rest/api/3/search");
            //string postData = @"{
            //            " + "\n" +
            //            @"  ""jql"": ""(key ='key_filtrar')"",  
            //            " + "\n" +
            //            @"  ""maxResults"": 100,
            //            " + "\n" +
            //            @"  ""startAt"": 0,
            //            " + "\n" +
            //            @"  ""fields"":[""id"", ""key"", ""summary"", ""description"", ""project"", ""worklog"" , ""timetracking"",""priority"",""reporter"",""assignee"",""resolutiondate"",""lastViewed"",""parent"",""issuetype"",""status"",, ""updated"", ""created"",
            //            " + "\n" +
            //            @"  ""customfield_18768"", ""customfield_18715"",""customfield_15000"",""customfield_10501"",""customfield_10008"",
            //            " + "\n" +
            //            @"  ""customfield_10007"",""customfield_18758"",""customfield_18792"",""customfield_10306"",""customfield_10004"",
            //            " + "\n" +
            //            @"  ""customfield_18714"",""customfield_12100"",""customfield_18500"",""customfield_18838"",""customfield_18837"",
            //            " + "\n" +
            //            @"  ""customfield_18834"",""customfield_18840"",""customfield_18841"",""customfield_18833"",""customfield_18844"",
            //            " + "\n" +
            //            @"  ""customfield_18845"",""customfield_18839"",""customfield_18832"",""customfield_17700"",""customfield_18835"",
            //            " + "\n" +
            //            @"  ""customfield_18836"",""customfield_18736"",""customfield_18898"",""customfield_18931"",""customfield_18903"",
            //            " + "\n" +
            //            @"  ""customfield_18908"",""customfield_18907"",""customfield_18800"",""customfield_18918"",""customfield_18799"",
            //            " + "\n" +
            //            @"  ""customfield_18895"",""customfield_18911"",""customfield_18926"",""customfield_18910"",""customfield_18914"",
            //            " + "\n" +
            //            @"  ""customfield_18915"",""customfield_18878"",""customfield_13200"",""customfield_18880"",""customfield_18881"",
            //            " + "\n" +
            //            @"  ""customfield_18882"",""customfield_18872"",""customfield_18873"",""customfield_18874"",""customfield_18876"",
            //            " + "\n" +
            //            @"  ""customfield_18875"",""customfield_18782"",""customfield_13111"",""customfield_14601""
            //            " + "\n" +
            //            @"  ]
            //            " + "\n" +
            //            @"}
            //            " + "\n" +
            //            @"";
            //postData = postData.Replace("key_filtrar", key);

            string filter = $@"
                ""jql"": ""(key = '{key}')""
            ";

            string postData = GetPostDataJira(filter,0);

            //Utilidades.LogService("JiraLite URL Issue: " + postData);

            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.Method = "POST";
            //request.Headers.Add("Host", "softoffice.atlassian.net");
            request.Headers.Add("Cache-Control", "no-cache");
            request.Headers.Add("Authorization", $"Basic {authToken}");
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
            obj = JsonConvert.DeserializeObject<AddAtlassianGotJiraJirasLite.AddJiraLite>(result);

            return obj;
        }

        private string GetPostDataJira(string filter, int startAt)
        {
            string postData = $@"
            {{
                {filter},
                ""maxResults"": 100,
                ""startAt"": {startAt},
                ""fields"": [
                    ""id"", ""key"", ""summary"", ""description"", ""project"", ""worklog"", ""timetracking"", ""priority"", ""reporter"", 
                    ""assignee"", ""resolutiondate"", ""lastViewed"", ""parent"", ""issuetype"", ""status"", ""updated"", ""created"",
                    ""customfield_18768"", ""customfield_18715"", ""customfield_15000"", ""customfield_10501"", ""customfield_10008"",
                    ""customfield_10007"", ""customfield_18758"", ""customfield_18792"", ""customfield_10306"", ""customfield_10004"",
                    ""customfield_18714"", ""customfield_12100"", ""customfield_18500"", ""customfield_18838"", ""customfield_18837"",
                    ""customfield_18834"", ""customfield_18840"", ""customfield_18841"", ""customfield_18833"", ""customfield_18844"",
                    ""customfield_18845"", ""customfield_18839"", ""customfield_18832"", ""customfield_17700"", ""customfield_18835"",
                    ""customfield_18836"", ""customfield_18736"", ""customfield_18898"", ""customfield_18931"", ""customfield_18903"",
                    ""customfield_18908"", ""customfield_18907"", ""customfield_18800"", ""customfield_18918"", ""customfield_18799"",
                    ""customfield_18895"", ""customfield_18911"", ""customfield_18926"", ""customfield_18910"", ""customfield_18914"",
                    ""customfield_18915"", ""customfield_18878"", ""customfield_13200"", ""customfield_18880"", ""customfield_18881"",
                    ""customfield_18882"", ""customfield_18872"", ""customfield_18873"", ""customfield_18874"", ""customfield_18876"",
                    ""customfield_18875"", ""customfield_18782"", ""customfield_13111"", ""customfield_14601"", ""customfield_12402"",
                    ""customfield_19130"", ""customfield_18960"", ""customfield_18849"", ""customfield_18916"", ""customfield_18998"",
                    ""customfield_19031"", ""customfield_19064"", ""customfield_19097""
                ]
            }}";

            return postData;
        }
        //private List<string> SearchEpicJiraAsync(string issueKey)
        //{
        //    List<string> IssueKeys = new List<string>();
        //    if (issueKey.Trim() == "") return IssueKeys;

        //    var client = new RestClient("https://softoffice.atlassian.net/rest/api/2/search?jql= \"Epic Link\" IN('" + issueKey + "') order by created desc");
        //    client.Timeout = -1;
        //    var request = new RestRequest(Method.GET);
        //    request.AddHeader("Accept", "application/json");
        //    request.AddHeader("Content-Type", "application/json");
        //    request.AddHeader("Authorization", $"Basic {authToken}");
        //    request.AddHeader("Cookie", "atlassian.xsrf.token=B5S9-YRZO-3SUG-U7W9_05fa5ac255df7ea7bb7a3f9a0e5367b5a08ada15_lin");
        //    IRestResponse response = client.Execute(request);

        //    if (response.IsSuccessful)
        //    {

        //        try
        //        {
        //            var Resultado = JsonConvert.DeserializeObject<AtlassianGotJiraEpicJira.AddEpicJira>(response.Content);
        //            foreach (var issue in Resultado.issues)
        //            {
        //                IssueKeys.Add(issue.key);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Utilidades.LogService("Error SearchEpicJiraAsync() " + issueKey + ": " + ex.Message);
        //        }
        //    }
        //    else
        //    {
        //        Utilidades.JirasErrores(issueKey, response.Content);
        //        return IssueKeys;
        //    }
        //    return IssueKeys;
        //}




        //private Issue[] SearchJiraEpicAsync(string issuesKey)
        //{
        //    AddAtlassianGotJira.RootObject obj = new AddAtlassianGotJira.RootObject();
        //    string strPath = "";
        //    string strPathFileErrores = "";
        //    MParametros objParametro = new MParametros();
        //    strPath = objParametro.ObtenerParametro("Pathfiles");
        //    strPathFileErrores = strPath + "errores_ObtenerJiras.txt";
        //    objParametro = null;

        //    var client = new RestClient("https://softoffice.atlassian.net/rest/api/2/search?jql=issuetype='Epic' and key not in("+ issuesKey + ") order by created desc");

        //    var request = new RestRequest(Method.GET);
        //    request.AddHeader("Host", "softoffice.atlassian.net");
        //    request.AddHeader("Cache-Control", "no-cache");
        //    request.AddHeader("Authorization", "Basic amlyYS5lbXhAZ21haWwuY29tOjVSNlIzVjhiM0hNR1ZmTXFSUUVqMTFDNQ==");
        //    IRestResponse response = client.Execute(request);

        //    string string_isu = response.Content;

        //    if (response.IsSuccessful)
        //    {
        //        var result = response.Content;

        //        try
        //        {
        //            obj = JsonConvert.DeserializeObject<AddAtlassianGotJira.RootObject>(result);
        //        }
        //        catch (Exception ex)
        //        {
        //            Utilidades.LogService("Error AddAtlassianGotJira() " + issuesKey + ": " + ex.Message);
        //        }
        //    }
        //    else
        //    {
        //        return obj;
        //    }
        //    return obj;
        //}

        private async Task<IEnumerable<JiraUser>> SearchAllUsersAsync(JiraUserStatus userStatus = JiraUserStatus.Active, int maxResults = 100, int startAt = 0, CancellationToken token = default(CancellationToken))
        {

            //busqueda por accountId: /rest/api/2/user/search?accountId=557058:67f4ef83-e059-4deb-8346-3c2c8d141997&includeActive=1&includeInactive=2&startAt=0&maxResults=100
            //busqueda por username:  /rest/api/3/user/search?query=username=aclavero&includeActive=1&includeInactive=2&startAt=0&maxResults=100
            string resource = "";
            var users = new List<JiraUser>();
            string Letra = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,ñ,o,p,q,r,s,t,u,v,w,x,y,z";
            Array A_Letras = Letra.Split(',').ToArray();
            var ListaLetras = new List<string>((string[])A_Letras);

            foreach (string l in ListaLetras)
            {
                //no anda mas: "rest/api/2/user/search?username={0}&includeActive={1}&includeInactive={2}&startAt={3}&maxResults={4}",

                string strSQL = l;
                resource = String.Format(
                "rest/api/3/user/search?query=username={0}&includeActive={1}&includeInactive={2}&startAt={3}&maxResults={4}",
                strSQL,
                userStatus.HasFlag(JiraUserStatus.Active),
                userStatus.HasFlag(JiraUserStatus.Inactive),
                startAt,
                maxResults);

                IEnumerable<JiraUser> search_result = await jiraConn.RestClient.ExecuteRequestAsync<IEnumerable<JiraUser>>(Method.GET, resource, null, token);

                foreach (var user in search_result)
                {
                    if (!users.Any(x => x.DisplayName.Equals(user.DisplayName, StringComparison.CurrentCultureIgnoreCase)))
                        users.Add(user);
                }
            }
            return users;
        }

        //private async Task<JiraUser> SearchUserByAccountIdAsync(string AccountId, CancellationToken token = default(CancellationToken))
        //{
        //    string resource = "";
        //    resource = String.Format(
        //    "rest/api/3/user/search?accountId=accountId={0}",
        //    AccountId);

        //    JiraUser search_result = await jiraConn.RestClient.ExecuteRequestAsync<JiraUser>(Method.GET, resource, null, token);

        //    return search_result;
        //}

        //private async Task<JiraUser> SearchUserByUserNameAsync(string UserName, CancellationToken token = default(CancellationToken))
        //{
        //    string resource = "";
        //    resource = String.Format(
        //    "rest/api/3/user/search?query=username={0}",
        //    UserName);

        //    JiraUser search_result = await jiraConn.RestClient.ExecuteRequestAsync<JiraUser>(Method.GET, resource, null, token);

        //    return search_result;
        //}

        //private Task<IEnumerable<IssueLink>> GetIssueLinksAsync(string _originalIssue, CancellationToken token = default(CancellationToken))
        //{
        //    if (String.IsNullOrEmpty(_originalIssue))
        //    {
        //        throw new InvalidOperationException("Unable to get issue links issues, issue has not been created.");
        //    }

        //    return GetLinksForIssueAsync(_originalIssue, token);
        //}
        private async Task<IEnumerable<IssueLink>> GetLinksForIssueAsync(string issueKey, CancellationToken token)
        {

            var serializerSettings = jiraConn.RestClient.Settings.JsonSerializerSettings;
            var resource = String.Format("rest/api/2/issue/{0}?fields=issuelinks,created", issueKey);
            var issueLinksResult = await jiraConn.RestClient.ExecuteRequestAsync(Method.GET, resource, null, token).ConfigureAwait(false);
            var issueLinksJson = issueLinksResult["fields"]["issuelinks"];

            if (issueLinksJson == null)
            {
                throw new InvalidOperationException("There is no 'issueLinks' field on the issue data, make sure issue linking is turned on in JIRA.");
            }

            var issueLinks = issueLinksJson.Cast<JObject>();
            var issuesToGet = issueLinks.Select(issueLink =>
            {
                var issueJson = issueLink["outwardIssue"] ?? issueLink["inwardIssue"];
                return issueJson["key"].Value<string>();
            }).ToList();
            issuesToGet.Add(issueKey);

            var issuesMap = await jiraConn.Issues.GetIssuesAsync(issuesToGet, token).ConfigureAwait(false);
            var issue = issuesMap[issueKey];
            return issueLinks.Select(issueLink =>
            {
                var linkType = JsonConvert.DeserializeObject<IssueLinkType>(issueLink["type"].ToString(), serializerSettings);
                var outwardIssue = issueLink["outwardIssue"];
                var inwardIssue = issueLink["inwardIssue"];
                var outwardIssueKey = outwardIssue != null ? (string)outwardIssue["key"] : null;
                var inwardIssueKey = inwardIssue != null ? (string)inwardIssue["key"] : null;
                return new IssueLink(
                    linkType,
                    outwardIssueKey == null ? issue : issuesMap[outwardIssueKey],
                    inwardIssueKey == null ? issue : issuesMap[inwardIssueKey]);
            });
        }
        public async Task<int> GetProjectForComponent(string ProjectKey = "")
        {
            int cantidad = 0;
            MParametros objParametro = new MParametros();
            string strPathFile = "";
            strPathFile = objParametro.ObtenerParametro("Pathfiles");
            strPathFile = strPathFile + "proyectos_x_componentes.txt";
            //File.Delete(strPathFile);                                                        
            File.WriteAllText(strPathFile, string.Empty);

            try
            {
                //FILTRAMOS UN PROYECTO EN PARTICULAR
                if (ProjectKey != "")
                {
                    CancellationToken token = default(CancellationToken);
                    Project project = await jiraConn.Projects.GetProjectAsync(ProjectKey, token);
                    try
                    {
                        IEnumerable<ProjectComponent> proyComp = await jiraConn.Components.GetComponentsAsync(project.Key);

                        foreach (ProjectComponent pxc in proyComp)
                        {
                            Utilidades.SaveFile(strPathFile, pxc.ProjectKey + "|" + pxc.Name);
                            cantidad++;
                        }
                    }
                    catch (Exception ex)
                    {
                        ex.Data.Clear();
                    }
                }
                else
                {
                    IEnumerable<Project> projects = new List<Project>();
                    projects = await jiraConn.Projects.GetProjectsAsync();
                    foreach (Project proy in projects)
                    {
                        try
                        {
                            IEnumerable<ProjectComponent> proyComp = await jiraConn.Components.GetComponentsAsync(proy.Key);

                            foreach (ProjectComponent pxc in proyComp)
                            {
                                Utilidades.SaveFile(strPathFile, pxc.ProjectKey + "|" + pxc.Name);
                                cantidad++;
                            }
                        }
                        catch (Exception ex)
                        {
                            ex.Data.Clear();
                        }
                    }
                }
                //MTablasIn objTablasIn = new MTablasIn();
                //objTablasIn.Grabar_InProyectosPorComponentes();
                //objTablasIn = null;
                //objParametro = null;
            }
            catch (Exception ex)
            {
                await Task.Run(() => Utilidades.LogService("Error ProjectForComponent: " + ex.Message));
                ex.Data.Clear();
            }
            finally
            {
                await Task.Run(() => Utilidades.LogService("GetProjectForComponent() FIN"));
            }
            return cantidad;
        }


        //el AddAtlassianGotJira.RootObject.fields.worklog.worklogs solo trae 20 arreglos... no sirve... se usa GetWorklogAsync()
        //public void GetWorklog_(AddAtlassianGotJiraJiras.AddJira RootObject)
        //{
        //    MParametros objParametro = new MParametros();
        //    string strPathFile = "";
        //    strPathFile = objParametro.ObtenerParametro("Pathfiles");
        //    strPathFile = strPathFile + "worklogs.txt";

        //    foreach (var worklog in RootObject.fields.worklog.worklogs)
        //    {

        //        //if (worklog.id == "371168")
        //        //{
        //        //    string a;
        //        //    a = "A";
        //        //}


        //        // SI VIENE ALGUN ÑUFO LO REEMPLAZO POR UN GUION
        //        string summary = RootObject.fields.summary?.Replace("|", "-");

        //        string comment = worklog.comment?.Replace("|", "-");

        //        DateTime date_started = (DateTime)worklog.started;
        //        string string_started = date_started.ToString("dd/MM/yyyy HH:mm:ss");

        //        DateTime date_created = (DateTime)worklog.created;
        //        string string_created = date_created.ToString("dd/MM/yyyy HH:mm:ss");

        //        DateTime date_updated = (DateTime)worklog.updated;
        //        string string_updated = date_updated.ToString("dd/MM/yyyy HH:mm:ss");



        //        // Format Datetime in different formats and display them
        //        //Console.WriteLine(aDate.ToString("MM/dd/yyyy"));
        //        //Console.WriteLine(aDate.ToString("dddd, dd MMMM yyyy"));
        //        //Console.WriteLine(aDate.ToString("dddd, dd MMMM yyyy"));
        //        //Console.WriteLine(aDate.ToString("dddd, dd MMMM yyyy"));
        //        //Console.WriteLine(aDate.ToString("dddd, dd MMMM yyyy"));
        //        //Console.WriteLine(aDate.ToString("dddd, dd MMMM yyyy"));
        //        //Console.WriteLine(aDate.ToString("dddd, dd MMMM yyyy HH:mm:ss"));
        //        //Console.WriteLine(aDate.ToString("MM/dd/yyyy HH:mm"));
        //        //Console.WriteLine(aDate.ToString("MM/dd/yyyy hh:mm tt"));
        //        //Console.WriteLine(aDate.ToString("MM/dd/yyyy H:mm"));
        //        //Console.WriteLine(aDate.ToString("MM/dd/yyyy h:mm tt"));
        //        //Console.WriteLine(aDate.ToString("MM/dd/yyyy HH:mm:ss"));
        //        //Console.WriteLine(aDate.ToString("MMMM dd"));
        //        //Console.WriteLine(aDate.ToString("yyyy’-‘MM’-‘dd’T’HH’:’mm’:’ss.fffffffK"));
        //        //Console.WriteLine(aDate.ToString("ddd, dd MMM yyy HH’:’mm’:’ss ‘GMT’"));
        //        //Console.WriteLine(aDate.ToString("yyyy’-‘MM’-‘dd’T’HH’:’mm’:’ss"));
        //        //Console.WriteLine(aDate.ToString("HH:mm"));
        //        //Console.WriteLine(aDate.ToString("hh:mm tt"));
        //        //Console.WriteLine(aDate.ToString("H:mm"));
        //        //Console.WriteLine(aDate.ToString("h:mm tt"));
        //        //Console.WriteLine(aDate.ToString("HH:mm:ss"));
        //        //Console.WriteLine(aDate.ToString("yyyy MMMM"));

        //        //var worklog = await jiraConn.Issues.GetWorklogAsync(issuKey, wk.Id, token);   
        //        if (worklog.id != null)
        //            Utilidades.SaveFile(strPathFile,
        //                RootObject.fields.project.key + "|" +
        //                RootObject.key + "|" +
        //                RootObject.fields.issuetype.name + "|" +
        //                summary + "|" +
        //                RootObject.fields.priority.name + "|" +
        //                string_started + "|" +
        //                worklog.author.displayName + "|" +
        //                worklog.timeSpentSeconds + "|" +
        //                comment + "|" +

        //                /*campos extras...*/
        //                worklog.timeSpent + "|" +
        //                string_created + "|" +
        //                string_updated + "|" +
        //                worklog.id + "|" +
        //                worklog.issueId
        //            );
        //    }
        //}
        public async Task GetWorklogAsync(string project_key, string issue_key, string summary, string issuetype_name, string priority_name, int startAt = 0, int maxResults = 5000, int Descargado = 0)
        {
            try
            {
                int tmpstartAt = 0;
                int tmpmaxResults = 0;
                MParametros objParametro = new MParametros();
                string strPathFile = "";
                strPathFile = objParametro.ObtenerParametro("Pathfiles");
                strPathFile = strPathFile + "worklogs.txt";
                CancellationToken token = default(CancellationToken);

                //--------------------------------------------------------------------------------------------------------------
                //ATENCION!!!!!!!
                //LA API POR UN TEMA DE SEGURIDAD YA NO DEVUELVE LOS NOMBRES DE LOS USUARIOS: => Author Y AuthorUser
                //https://community.atlassian.com/t5/Jira-Software-questions/Work-log-author-always-null/qaq-p/1314025
                //--------------------------------------------------------------------------------------------------------------

                //var Worklogs = await jiraConn.Issues.GetWorklogsAsync(issuKey, token);
                //foreach (Atlassian.Jira.Worklog worklog in Worklogs)
                //{
                //var worklog = await jiraConn.Issues.GetWorklogAsync(issuKey, wk.Id, token);

                //if (worklog.Id != null) Utilidades.SaveFile(strPathFile, worklog.Id + "|" + worklog.Author + "|" + worklog.AuthorUser + "|" + worklog.Comment + "|" + worklog.TimeSpent + "|" + worklog.StartDate + "|" + worklog.TimeSpentInSeconds + "|" + worklog.CreateDate  + "|" + worklog.UpdateDate);               

                //}

                //RECOMIENDAN PEGARLE A LA API DE ESTA FORMA:            
                string resource = "";
                resource = String.Format("rest/api/3/issue/{0}/worklog?startAt={1}&maxResults={2}", issue_key, startAt, maxResults);

                jiraConn.RestClient.RestSharpClient.Timeout = -1;
                AtlassianGotJiraWorkLogs.Root response = await jiraConn.RestClient.ExecuteRequestAsync<AtlassianGotJiraWorkLogs.Root>(Method.GET, resource, null, token);
                Descargado = response.maxResults + Descargado;
                if ((response.total - Descargado) > 0)
                {
                    tmpstartAt = Descargado;
                    tmpmaxResults = response.total - Descargado;
                }
                else
                {
                    tmpstartAt = 0;
                    tmpmaxResults = 0;
                }
                //Console.WriteLine("tmpstartAt 1: " + tmpstartAt);
                //Console.WriteLine("tmpmaxResults 1: " + tmpmaxResults);

                string sync_only_this_user = objParametro.ObtenerParametro("SyncOnlyThisUserTimeSheet");
                IEnumerable<AtlassianGotJiraWorkLogs.AddWorklog> WorkLogs = new List<AtlassianGotJiraWorkLogs.AddWorklog>();
                if (sync_only_this_user != "")
                    //WorkLogs = response.worklogs.Where(x => (x.author.active == true && x.author.accountId == sync_only_this_user));
                    WorkLogs = response.worklogs.Where(x => (x.author.accountId == sync_only_this_user)); //el usuario puede estar inactivo pero worklogs me retorna las horas pero jira no me retorna el usuario, paso con betty rivero
                else
                    //WorkLogs = response.worklogs.Where(x => x.author.active == true);
                    WorkLogs = response.worklogs; ////el usuario puede estar inactivo pero worklogs me retorna las horas pero jira no me retorna el usuari, paso con betty rivero

                foreach (AtlassianGotJiraWorkLogs.AddWorklog worklog in WorkLogs)
                {
                    string comment = "";
                    try
                    {
                        comment = worklog.comment?.content[0]?.content[0]?.text + "";
                    }
                    catch
                    {
                        comment = "";
                    }

                    DateTime date_started = (DateTime)worklog.started;
                    string string_started = date_started.ToString("dd/MM/yyyy HH:mm:ss");

                    DateTime date_created = (DateTime)worklog.created;
                    string string_created = date_created.ToString("dd/MM/yyyy HH:mm:ss");

                    DateTime date_updated = (DateTime)worklog.updated;
                    string string_updated = date_updated.ToString("dd/MM/yyyy HH:mm:ss");

                    if (worklog.id != null)
                        Utilidades.SaveFile(strPathFile,
                            project_key + "|" +
                            issue_key + "|" +
                            issuetype_name + "|" +
                            Regex.Replace(summary ?? "", @"\r\n|\r|\n", "").Replace("|", "-") + "|" + // SI VIENE ALGUN PIPE LO REEMPLAZO POR UN GUION Y SACO LOS ENTERS
                            priority_name + "|" +
                            string_started + "|" +
                            worklog.author.displayName + "|" +
                            worklog.timeSpentSeconds + "|" +
                            Regex.Replace(comment ?? "", @"\r\n|\r|\n", "").Replace("|", "-") + "|" + // SI VIENE ALGUN PIPE LO REEMPLAZO POR UN GUION Y SACO LOS ENTERS

                            /*campos extras...*/
                            worklog.timeSpent + "|" +
                            string_created + "|" +
                            string_updated + "|" +
                            worklog.id + "|" +
                            worklog.issueId
                        );
                }

                if (tmpstartAt > 0)
                    await GetWorklogAsync(project_key, issue_key, summary, issuetype_name, priority_name, tmpstartAt, tmpmaxResults, Descargado);
            }
            catch (Exception x)
            {
                await Task.Run(() => Utilidades.LogService("GetWorklogAsync Error: " + x.Message));
                x.Data.Clear();

                try
                {
                    await Task.Run(() => Utilidades.LogService("Llamada a GetWorklogAsync2()"));
                    //en vez de la api usa RestClient con timeout infinito
                    await GetWorklogAsync2(project_key, issue_key, summary, issuetype_name, priority_name, 0, 5000, 0);
                }
                catch (Exception ex)
                {
                    //Notificar("GetWorklogAsync() ERR: " + issue_key);
                    ex.Data.Clear();
                }
            }
        }

        public async Task GetWorklogAsync2(string project_key, string issue_key, string summary, string issuetype_name, string priority_name, int startAt = 0, int maxResults = 5000, int Descargado = 0)
        {
            try
            {
                int tmpstartAt = 0;
                int tmpmaxResults = 0;
                MParametros objParametro = new MParametros();
                string strPathFile = "";
                strPathFile = objParametro.ObtenerParametro("Pathfiles");
                strPathFile = strPathFile + "worklogs.txt";
                
                string resource = "";
                resource = String.Format("https://softoffice.atlassian.net/rest/api/3/issue/{0}/worklog?startAt={1}&maxResults={2}", issue_key, startAt, maxResults);

                var client = new RestClient(resource);
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("Host", "softoffice.atlassian.net");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Authorization", $"Basic {authToken}");
                IRestResponse response = await client.ExecuteAsync(request);
                //Console.WriteLine(response.Content);

                AtlassianGotJiraWorkLogs.Root Root = new AtlassianGotJiraWorkLogs.Root();
                Descargado = Root.maxResults + Descargado;
                if ((Root.total - Descargado) > 0)
                {
                    tmpstartAt = Descargado;
                    tmpmaxResults = Root.total - Descargado;
                }
                else
                {
                    tmpstartAt = 0;
                    tmpmaxResults = 0;
                }
                //Console.WriteLine("tmpstartAt 2: " + tmpstartAt);
                //Console.WriteLine("tmpmaxResults 2: " + tmpmaxResults);

                if (response.IsSuccessful)
                {
                    var result = response.Content;

                    try
                    {
                        Root = JsonConvert.DeserializeObject<AtlassianGotJiraWorkLogs.Root>(result);
                    }
                    catch (Exception ex)
                    {
                        //Notificar("GetWorklogAsync2() ERR: " + issue_key);
                        ex.Data.Clear();
                    }
                }

                string sync_only_this_user = objParametro.ObtenerParametro("SyncOnlyThisUserTimeSheet");
                IEnumerable<AtlassianGotJiraWorkLogs.AddWorklog> WorkLogs = new List<AtlassianGotJiraWorkLogs.AddWorklog>();
                if (sync_only_this_user != "")
                    WorkLogs = Root.worklogs.Where(x => (x.author.accountId == sync_only_this_user));
                    //WorkLogs = Root.worklogs.Where(x => (x.author.active == true && x.author.accountId == sync_only_this_user)); //el usuario puede estar inactivo pero worklogs me retorna las horas pero jira no me retorna el usuario, paso con betty rivero
                else
                    WorkLogs = Root.worklogs;
                    //WorkLogs = Root.worklogs.Where(x => x.author.active == true); //el usuario puede estar inactivo pero worklogs me retorna las horas pero jira no me retorna el usuari, paso con betty rivero

                foreach (AtlassianGotJiraWorkLogs.AddWorklog worklog in WorkLogs /*response.worklogs*/)
                {
                    string comment = "";
                    try
                    {
                        comment = worklog.comment?.content[0]?.content[0]?.text + "";
                    }
                    catch
                    {
                        comment = "";
                    }

                    DateTime date_started = (DateTime)worklog.started;
                    string string_started = date_started.ToString("dd/MM/yyyy HH:mm:ss");

                    DateTime date_created = (DateTime)worklog.created;
                    string string_created = date_created.ToString("dd/MM/yyyy HH:mm:ss");

                    DateTime date_updated = (DateTime)worklog.updated;
                    string string_updated = date_updated.ToString("dd/MM/yyyy HH:mm:ss");

                    if (worklog.id != null)
                        Utilidades.SaveFile(strPathFile,
                            project_key + "|" +
                            issue_key + "|" +
                            issuetype_name + "|" +
                            Regex.Replace(summary ?? "", @"\r\n|\r|\n", "").Replace("|", "-") + "|" + // SI VIENE ALGUN PIPE LO REEMPLAZO POR UN GUION
                            priority_name + "|" +
                            string_started + "|" +
                            worklog.author.displayName + "|" +
                            worklog.timeSpentSeconds + "|" +
                            Regex.Replace(comment ?? "", @"\r\n|\r|\n", "").Replace("|", "-") + "|" + // SI VIENE ALGUN PIPE LO REEMPLAZO POR UN GUION

                            /*campos extras...*/
                            worklog.timeSpent + "|" +
                            string_created + "|" +
                            string_updated + "|" +
                            worklog.id + "|" +
                            worklog.issueId
                        );
                }

                if (tmpstartAt > 0)
                    await GetWorklogAsync2(project_key, issue_key, summary, issuetype_name, priority_name, tmpstartAt, tmpmaxResults, Descargado);
            }
            catch (Exception ex)
            {
                //Notificar("GetWorklogAsync2() ERR: " + issue_key);
                ex.Data.Clear();
            }
        }
    }

    public static class Utilidades
    {
        public delegate void work(string notificacion);
        //public static event work Notificar;

        public static bool Like(this string toSearch, string toFind)
        {
            return new Regex(@"\A" + new Regex(@"\.|\$|\^|\{|\[|\(|\||\)|\*|\+|\?|\\").Replace(toFind, ch => @"\" + ch).Replace('_', '.').Replace("%", ".*") + @"\z", RegexOptions.Singleline).IsMatch(toSearch);
        }
        public static void MoveFile(string strFileNameOrigin, string strFileNameDest)
        {
            try
            {
                File.Move(strFileNameOrigin, strFileNameDest);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
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
        public static void JirasErrores(string key, string mensaje)
        {
            try
            {
                DateTime fecha_actual = new DateTime();
                fecha_actual = DateTime.Now;

                MParametros objParametro = new MParametros();
                string strPathFile = "";
                strPathFile = objParametro.ObtenerParametro("Pathfiles");
                strPathFile = strPathFile + "GotJiraErrores.txt";
                SaveFile(strPathFile, fecha_actual + " --> " + key + ';' + mensaje);
                objParametro = null;
            }
            catch
            {
            }
        }

        public static void LimpiarLogs()
        {
            try
            {
                MParametros objParametro = new MParametros();
                string strPathFile = "";
                strPathFile = objParametro.ObtenerParametro("Pathfiles");

                File.WriteAllText(strPathFile + "GotJiraLogVisorService.txt", string.Empty);

                File.WriteAllText(strPathFile + "GotJiraServiceLogs.txt", string.Empty);

                LogService("Archivo Reseteado.");

                LogVisorService("Archivo Reseteado.");

                objParametro = null;

            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
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

                strPathFile = strPathFile + "GotJiraServiceLogs.txt";
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

                strPathFile = strPathFile + "GotJiraLogVisorService.txt";
                SaveFile(strPathFile, fecha_actual + " --> " + strTexto);
                objParametro = null;
            }
            catch
            {
            }
        }
        public static async Task LogServiceAsync(string strTexto)
        {
            try
            {
                DateTime fecha_actual = new DateTime();
                fecha_actual = DateTime.Now;

                MParametros objParametro = new MParametros();
                string strPathFile = "";
                strPathFile = objParametro.ObtenerParametro("Pathfiles");
                strPathFile = strPathFile + "GotJiraServiceLogs.txt";
                await SaveFileAsync(strPathFile, fecha_actual + " --> " + strTexto);
                objParametro = null;
            }
            catch
            {
            }
        }

        public static void DownloadData(IEnumerable<Issue> _issues, string strPathFileJira)
        {
            try
            {
                string _key = "";
                foreach (Issue issue in _issues)
                {
                    try
                    {
                        _key = issue.Key.ToString();
                        SaveFile(strPathFileJira, issue.Project + "|" + issue.Key + "|" + issue.Type.Name + "|"
                                    + issue.Summary.Trim() + "|" + issue["Epic Link"] + "|" + issue["Grupo de Actividad"] + "|" + issue["Sprint"] + "|" + issue.ParentIssueKey
                                    + "|" + issue["Origen del error"] + "|" + issue["Fase detectado"] + "|" + (issue.CustomFields[0].Name == "Cliente" ? issue.CustomFields[0].Values[0] : "") + "|" + issue["Marco Producto"]);
                    }
                    catch (Exception ex)
                    {
                        // //Notificar("Error DownloadData() jira_key: " + _key + " Error: " + ex.Message);
                        ex.Data.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
            }
        }
        public static string QuitarAcentos(string valor)
        {
            valor = valor.Trim();
            valor = valor.ToLower();
            valor = Regex.Replace(valor, @"\s+", " ");

            var inputString = valor;
            var normalizedString = inputString.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            for (int i = 0; i < normalizedString.Length; i++)
            {
                var uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(normalizedString[i]);
                if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(normalizedString[i]);
                }
            }
            return (sb.ToString().Normalize(NormalizationForm.FormC));
        }

        public static string Encriptar(string Input)
        {
            var IV = Encoding.ASCII.GetBytes("qualityi"); // La clave debe ser de 8 caracteres
            var EncryptionKey = Convert.FromBase64String("rpaSPvIvVLlrcmtzPU9/c67Gkj7yL1S5"); // No se puede alterar la cantidad de caracteres pero si la clave
            var buffer = Encoding.UTF8.GetBytes(Input);
            var des = new TripleDESCryptoServiceProvider();
            des.Key = EncryptionKey;
            des.IV = IV;
            return Convert.ToBase64String(des.CreateEncryptor().TransformFinalBlock(buffer, 0, buffer.Length));
        }

        public static string Desencriptar(string Input)
        {
            var IV = Encoding.ASCII.GetBytes("qualityi"); // La clave debe ser de 8 caracteres
            var EncryptionKey = Convert.FromBase64String("rpaSPvIvVLlrcmtzPU9/c67Gkj7yL1S5"); // No se puede alterar la cantidad de caracteres pero si la clave
            var buffer = Convert.FromBase64String(Input);
            var des = new TripleDESCryptoServiceProvider();
            des.Key = EncryptionKey;
            des.IV = IV;
            return Encoding.UTF8.GetString(des.CreateDecryptor().TransformFinalBlock(buffer, 0, buffer.Length));
        }

    }
}
