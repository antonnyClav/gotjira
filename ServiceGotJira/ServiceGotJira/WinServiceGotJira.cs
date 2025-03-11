using System;
using System.Timers;
using GotJira;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Linq;

namespace AddAtlassianGotJiraJiras
{
    public class WinServiceGotJira
    {
        private DateTime FechaServerDb;
        private int IntervaloExec = 4;
        private static int Segundo = 1000;
        private static readonly int Minuto = Segundo * 60;
        private static readonly int Hora = Minuto * 60;
        private System.Timers.Timer _timer = new System.Timers.Timer();

        public WinServiceGotJira() {
            try
            {
                _timer = new Timer(Segundo * 10) { AutoReset = true };
                _timer.Elapsed += TimerElapsed;
                _timer.Enabled = true;

            }
            catch (Exception ex)
            {
                Notificar("Error:" + ex.Message);
                Stop();
            }            
        }    
        private async void TimerElapsed(object sender, ElapsedEventArgs e) {
            try
            {
                //FILTRO DE FECHA PARA TIMESHEET Y JIRAS:
                //Se levantan los jiras que fueron informados en timesheet y que aun no estan en la tabla de jiras
                //Los TimeSheet se levantan desde (fecha actual - X dias(parametro)) hasta la fecha actual.
                //o
                //desde(parametro) hasta(parametro)), x dias tiene que ser 0

                //sincronizar solo en un rango horario permitido(parametro)
                DateTime datetime = DateTime.Now;
                DateTime datetime2 = new DateTime(2020, 12, 19, 11, 34, 40);

                if ((datetime.Year.Equals(datetime2.Year)) && (datetime.Month.Equals(datetime2.Month)) && (datetime.Day.Equals(datetime2.Day))){
                    int res = DateTime.Compare(datetime, datetime);
                }                              

                if (HorarioValido())
                {
                    //Se estopea el servicio de ServiceGotJiraLite para que no se pisen los procesos
                    //StopServiceLite();

                    await SincronizarInfoFull();                    
                    WriteToEventLog("FULL FINALIZADO!");                    
                }               
            }
            catch (Exception)
            {
                // throw;
                WriteToEventLog("Error Sincronización GotJira FULL: " + DateTime.Now + " -> El servicio se sigue ejecutando...");
            }            
        }        
        private bool HorarioValido()
        {
            clsJira objJira = new clsJira();
            
            try
            {
                if (objJira.HorarioValido)
                {
                    FechaServerDb = objJira.FechaServer;
                    IntervaloExec = objJira.IntervaloExec;

                    return true;
                }
                else
                {      
                    return false;
                }
            }
            catch (Exception)
            {
                WriteToEventLog("objJira.HorarioValido Error: " + DateTime.Now + " ->");
                return false;
            }
            finally {
                objJira = null;
            }                                   
        }
        public void Start() {
            _timer.Start();
        }
        public void Stop()
        {
            _timer.Stop();
            //try
            //{
            //    this.StopServiceLite();
            //}
            //catch (Exception ex)
            //{
            //    ex.Data.Clear();
            //}
            
        }
        private async Task SincronizarInfoFull()
        {
            DateTime fecha_actual = DateTime.Now;
            try
            {
                WriteToEventLog("FULL EJECUTADO");                                          

                _timer.Stop();

                string FechaHasta = "";
                FechaHasta = FechaServerDb.Year.ToString() + "/" + Formating(FechaServerDb.Month) + "/" + Formating(FechaServerDb.Day) + " " + Formating(FechaServerDb.Hour) + ":" + Formating(FechaServerDb.Minute);

                try
                {
                    WriteToEventLog("FULL PROCESANDO...");

                    if (ProcesarArchivos() == "1")
                    {
                        WriteToEventLog("Configurado para procesar los archivos .txt");
                        ActualizarDB(); //FECHA DESDE LA TOMA DE PARAMETROS TIMESHEET  
                    }
                    else
                    {
                        var _GetProjects = GetProjects(); //OBTENGO TODOS LOS PROYECTOS DE JIRA

                        var _GetProjectForComponent = GetProjectForComponent(); //OBTENGO TODOS LOS PROYECTOS_COMPONENTES DE JIRA

                        var _GetUsers = GetUsers(); //OBTENGO LOS USUARIOS DE JIRA Y AD                                       

                        var _GetIssues = GetIssuesLite(); //OBTENGO JIRAS FECHA DESDE LA TOMA DE PARAMETROS TIMESHEET                   

                        await Task.WhenAll(_GetProjects, _GetProjectForComponent, _GetUsers, _GetIssues);

                        ActualizarDB(); //FECHA DESDE LA TOMA DE PARAMETROS TIMESHEET                    
                    }                    
                }
                catch (Exception ex)
                {
                    WriteToEventLog("Error Sincronización GotJira: " + DateTime.Now + " -> ERROR: " + ex.Message);
                }
                _timer = new Timer(Hora * IntervaloExec) { AutoReset = true };
                _timer.Elapsed += TimerElapsed;
                _timer.Start();

            }
            catch (Exception)
            {
                WriteToEventLog("Error Sincronización GotJira: " + DateTime.Now + " -> ver logs errores");
            }

        }
        private string Formating(int valor) {
            string s_return = valor.ToString();
            if (valor < 10) {
                s_return = "0" + valor;
            }
            return s_return;
        }
        private void WriteToEventLog(string message)
        {
            // return;
            try
            {
                //Windows 10
                //string ServiceName = "WinServiceGotJira";
                //EventLog EventLog = new System.Diagnostics.EventLog();
                //EventLog.Source = ServiceName;
                //EventLog.Log = "Application";

                //((ISupportInitialize)(EventLog)).BeginInit();
                //if (!EventLog.SourceExists(EventLog.Source))
                //{
                //    EventLog.CreateEventSource(EventLog.Source, EventLog.Log);
                //}
                //((ISupportInitialize)(EventLog)).EndInit();

                //EventLog.WriteEntry(message, EventLogEntryType.Information);

                //EventLog = null;

                //Windows 7
                //string cs = "WinServiceGotJira";
                //string logName = "WinServiceGotJira";
                //EventLog elog = new EventLog();

                //if (!EventLog.SourceExists(cs))
                //{
                //    EventLog.CreateEventSource(cs, logName);
                //}

                //elog.Log = logName;
                //elog.Source = cs;
                //elog.EnableRaisingEvents = true;
                //elog.WriteEntry(message);
                //elog = null;

                Utilidades.LogVisorService(message);
            }
            catch (Exception ex)
            {
                Utilidades.LogVisorService(ex.Message);                
            }            
        }
        protected async Task GetProjects()
        {
            try
            {                
                clsJira objJira = new  clsJira();
                //objJira.Notificar += new clsJira.work(Notificar);
                int iCantProjectInsert = 0;
                iCantProjectInsert = await Task.Run(() => objJira.GetProjects());

                if(iCantProjectInsert<0)
                    WriteToEventLog("GetProjects() FIN FORZADO: NO ES HORARIO O DIA VALIDO." + DateTime.Now + " ->");

                objJira = null;                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }        
        protected async Task GetProjectForComponent()
        {
            try
            {
                clsJira objJira = new clsJira();
                //objJira.Notificar += new clsJira.work(Notificar);
                int iCantProjectInsert = 0;
                iCantProjectInsert = await Task.Run(() => objJira.GetProjectForComponent());

                if (iCantProjectInsert < 0)
                    WriteToEventLog("GetProjects() FIN FORZADO: NO ES HORARIO O DIA VALIDO." + DateTime.Now + " ->");

                objJira = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected async Task GetUsers()
        {
            try
            {
                clsJira objJira = new clsJira();
                await Task.Run(() => objJira.GetUsers());
                objJira = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }
        protected async Task<DateTime> GetIssuesLite()
        {
            DateTime FecUltimaSinIssuesLite = DateTime.MinValue;
            try
            {
                clsJira objJira = new clsJira();
                FecUltimaSinIssuesLite = await Task.Run(() => objJira.GetIssuesLite());
                objJira = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return FecUltimaSinIssuesLite;
        }
        protected bool ActualizarDB(/*string FechaHasta*/)
        {
            try
            {
                clsJira objJira = new clsJira();
                //objJira.Notificar += new clsJira.work(Notificar);

                objJira.ActualizarDB();
                                
                objJira = null;

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        string ProcesarArchivos()
        {
            string _return = "";
            // Ruta del archivo XML
            string filePath = @"EncryptConn.xml";

            // Cargar el archivo XML
            XDocument xmlDoc = XDocument.Load(filePath);

            // Buscar el elemento <ProcesarArchivos>
            XElement dbElement = xmlDoc.Descendants("ProcesarArchivos").FirstOrDefault();
            if (dbElement != null)
            {
                _return = dbElement.Value;
                if(_return=="1")
                    Console.WriteLine("NO SE CONSULTA LAS APIS DE ATLASSIAN JIRA, SE PROCESAN LOS ARCHIVOS EN DISCO.");
                else
                    Console.WriteLine("CONSULTANDO LAS APIS DE ATLASSIAN JIRA.");
            }
            else
            {
                Console.WriteLine("El tag <ProcesarArchivos> no se encontró en el archivo XML.");
            }

            return _return;
        }
        protected void Notificar(string Notificacion)
        {            
            WriteToEventLog(Notificacion);            
            //if (InvokeRequired)
            //{
            //Invoke(new Action(() => this.lstNotificaciones.Items.Add(Notificacion)));
            //Invoke(new Action(() => this.lstNotificaciones.SelectedIndex = this.lstNotificaciones.Items.Count - 1));
            //}
        }
    }
}
