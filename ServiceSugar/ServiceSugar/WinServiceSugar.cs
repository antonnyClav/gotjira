using System;
using System.Timers;
using System.Diagnostics;
using Sugar;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ServiceProcess;

namespace Sugar
{
    public class WinServiceSugar
    {
        private DateTime FechaServerDb;
        private int IntervaloExec = 4;
        private static int Segundo = 1000;
        private static readonly int Minuto = Segundo * 60;
        private static readonly int Hora = Minuto * 60;
        private System.Timers.Timer _timer = new System.Timers.Timer();
        public WinServiceSugar() {
            try
            {
                int segundos = 300;
                //EmxSugar objSugar = new EmxSugar();
                #if (DEBUG)
                    segundos = 10;
                #endif

                _timer = new Timer(Segundo * segundos) { AutoReset = true };
                _timer.Elapsed += TimerElapsed;
                _timer.Enabled = true;

            }
            catch (Exception ex)
            {
                Notificar("Error:" + ex.Message);
                Stop();
            }
        }

        private async void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (HorarioValido())
                {
                    await SincronizarSugar();
                    WriteToEventLog("SUGAR FINALIZADO!");
                }
            }
            catch (Exception ex)
            {
                // throw;
                WriteToEventLog("Error Sincronización SUGAR: " + DateTime.Now + " -> El servicio se sigue ejecutando...");
            }
        }

        private bool HorarioValido()
        {
            EmxSugar objSugar = new EmxSugar(false);

            try
            {
                if (objSugar.HorarioValido)
                {
                    FechaServerDb = objSugar.FechaServer;
                    IntervaloExec = objSugar.IntervaloExec;

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                WriteToEventLog("objSugar.HorarioValido Error: " + DateTime.Now + " ->");
                return false;
            }
            finally {
                objSugar = null;
            }
        }

        public void Start() {
            _timer.Start();
        }
        public void Stop()
        {
            _timer.Stop();
        }

        //private bool ServiceSugarIsRunning()
        //{
        //    bool statusService = false;
        //    try
        //    {
        //        ServiceController sc = new ServiceController();
        //        sc.ServiceName = "WinServiceSugar";                
        //        statusService = (sc.Status == ServiceControllerStatus.Running);
        //        sc = null;
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.Data.Clear();
        //    }

        //    return statusService;
        //}

        private async Task SincronizarSugar()
        {
            //Si el servicio ya esta corriendo salgo.
            //if (ServiceSugarIsRunning()) {
            //    Console.WriteLine("El servicio WinServiceSugar ya esta en ejecución, NO SE EJECUTA!" );
            //    return; 
            //}

            DateTime fecha_actual = DateTime.Now;
            try
            {
                WriteToEventLog("SUGAR EJECUTADO" );
                _timer.Stop();                

                try
                {
                    WriteToEventLog("SUGAR PROCESANDO...");

                    var _LimpiarTablasIn = LimpiarTablasIn();
                    await Task.WhenAny(_LimpiarTablasIn);

                    var _GetUsuarios = GetUsuarios();
                    var _GetCuentas = GetCuentas();
                    var _GetOportunidades = GetOportunidades();                    
                    var _GetTareas = GetTareas();
                    var _GetAutorizaciones = GetAutorizaciones();

                    await Task.WhenAll(_GetUsuarios, _GetCuentas, _GetOportunidades, _GetTareas, _GetAutorizaciones);

                    ActualizarDB();
                }
                catch (Exception ex)
                {
                    WriteToEventLog("Error Sincronización SUGAR: " + DateTime.Now + " -> ERROR: " + ex.Message);
                }
                _timer = new Timer(Hora * IntervaloExec) { AutoReset = true };
                _timer.Elapsed += TimerElapsed;
                _timer.Start();
            }
            catch (Exception)
            {
                WriteToEventLog("Error Sincronización SUGAR: " + DateTime.Now + " -> ver logs errores");                
            }            
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
        protected async Task LimpiarTablasIn()
        {
            try
            {
                EmxSugar objSugar = new EmxSugar(false);

                await Task.Run(() => objSugar.LimpiarTablasIn());

                objSugar = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected async Task GetTareas()
        {
            try
            {
                EmxSugar objSugar = new EmxSugar(true);

                await Task.Run(() => objSugar.GetTareas());

                objSugar = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected async Task GetAutorizaciones()
        {
            try
            {
                EmxSugar objSugar = new EmxSugar(true);

                await Task.Run(() => objSugar.GetAutorizaciones());

                objSugar = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected async Task GetUsuarios()
        {
            try
            {
                EmxSugar objSugar = new EmxSugar(true);

                await Task.Run(() => objSugar.GetUsuarios());

                objSugar = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected async Task GetCuentas()
        {
            try
            {
                EmxSugar objSugar = new EmxSugar(true);

                await Task.Run(() => objSugar.GetCuentas());

                objSugar = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected async Task GetOportunidades()
        {
            try
            {                
                EmxSugar objSugar = new EmxSugar(true);

                await Task.Run(() => objSugar.GetOportunidades());

                objSugar = null;                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected bool ActualizarDB()
        {
            try
            {
                EmxSugar objSugar = new EmxSugar(false);

                objSugar.ActualizarDB();

                objSugar = null;
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
