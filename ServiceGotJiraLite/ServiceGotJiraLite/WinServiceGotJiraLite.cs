using System;
using System.Timers;
using System.Diagnostics;
using GotJira;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ServiceProcess;

namespace AddAtlassianGotJiraJiras
{
    public class WinServiceGotJiraLite
    {
        private DateTime FechaServerDb;
        private int IntervaloExec = 4;
        private static int Segundo = 1000;
        private static readonly int Minuto = Segundo * 60;
        private static readonly int Hora = Minuto * 60;
        private System.Timers.Timer _timerLite = new System.Timers.Timer();
        public WinServiceGotJiraLite() {
            try
            {
                _timerLite = new Timer(Segundo * 10) { AutoReset = true };
                _timerLite.Elapsed += TimerElapsedLite;
                _timerLite.Enabled = true;
            }
            catch (Exception ex)
            {
                Notificar("Error:" + ex.Message);
                Stop();
            }            
        }
    
        private async void TimerElapsedLite(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (HorarioValido())
                {
                    //this.StopServiceGotjira();
                    await SincronizarInfoLite();
                    WriteToEventLog("LITE FINALIZADO!");
                }
            }
            catch (Exception ex)
            {
                // throw;
                WriteToEventLog("Error Sincronización GotJira LITE: " + DateTime.Now + " -> El servicio se sigue ejecutando...");
            }
        }

        private void StopServiceGotjira()
        {
            try
            {
                // Compruebe si el v está corriendo.
                ServiceController sc = new ServiceController();
                sc.ServiceName = "WinServiceGotJira";
                Console.WriteLine("El estado del servicio WinServiceGotJira está actualmente configurado en {0}", sc.Status.ToString());
                if (sc.Status == ServiceControllerStatus.Running)
                {
                    // stopear el servicio si el estado actual es en ejecucion.
                    Console.WriteLine("Detener el servicio WinServiceGotJira...");
                    try
                    {
                        // Stopeo el servicio y espere hasta que su estado sea "stopeado".
                        sc.Stop();
                        sc.WaitForStatus(ServiceControllerStatus.Stopped);

                        // Muestra el estado actual del servicio.
                        Console.WriteLine("El estado del servicio WinServiceGotJira ahora está configurado en {0}.", sc.Status.ToString());
                    }
                    catch (InvalidOperationException)
                    {
                        Console.WriteLine("No se pudo detener el servicio WinServiceGotJira.");
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();
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
            _timerLite.Start();
        }
        public void Stop()
        {
            _timerLite.Stop();
        }

        private async Task SincronizarInfoLite()
        {          
            DateTime fecha_actual = DateTime.Now;
            try
            {
                WriteToEventLog("LITE EJECUTADO" );
                _timerLite.Stop();                

                try
                {
                    WriteToEventLog("LITE PROCESANDO...");

                    var _GetTimeSheet = GetTimeSheetLite(); //SOLO LAS HORAS DE HOY    

                    var _GetProjects = GetProjects();

                    var _GetUsers = GetUsers(); //OBTENGO LOS USUARIOS DE JIRA Y AD                                       

                    var _GetIssuesLite = GetIssuesLite(); // SOLO JIRAS QUE SE EDITARON O CREARON HOY
                    
                    await Task.WhenAll(_GetTimeSheet, _GetProjects, _GetIssuesLite, _GetUsers);

                    ActualizarDBLite(_GetIssuesLite.Result); //SE GRABA LO BASICO
                }
                catch (Exception ex)
                {
                    WriteToEventLog("Error Sincronización Lite GotJira: " + DateTime.Now + " -> ERROR: " + ex.Message);
                }
                _timerLite = new Timer(Segundo * 10) { AutoReset = true };
                _timerLite.Elapsed += TimerElapsedLite;
                _timerLite.Start();
            }
            catch (Exception)
            {
                WriteToEventLog("Error Sincronización Lite GotJira: " + DateTime.Now + " -> ver logs errores");                
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

        protected async Task GetTimeSheetLite()
        {
            try
            {
                int iCantHorasInsert;
                clsTimeSheet TimeSheet = new clsTimeSheet();
                
                iCantHorasInsert = await Task.Run(() => TimeSheet.ObtenerTimeSheetLite());
                if (iCantHorasInsert < 0)
                    WriteToEventLog("ObtenerTimeSheetLite() FIN FORZADO: NO ES HORARIO O DIA VALIDO." + DateTime.Now + " ->");

                TimeSheet = null;
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

        protected bool ActualizarDBLite(DateTime FecUltimaSinIssuesLite)
        {
            try
            {
                clsJira objJira = new clsJira();
                //objJira.Notificar += new clsJira.work(Notificar);

                objJira.ActualizarDBLite(FecUltimaSinIssuesLite);

                objJira = null;
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
