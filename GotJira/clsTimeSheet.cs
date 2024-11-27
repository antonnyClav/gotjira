using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DAL;
using RestSharp;

namespace GotJira
{
    public class clsTimeSheet
    {       
        public clsTimeSheet()
        {
            //Console.WriteLine("Started");
            //TimeSheetRequest req = new TimeSheetRequest();
            //Console.WriteLine("Finished");

            int _TopeDiasTimeSheet = 90;
            MParametros objParametro = new MParametros();
            RutaDestino = objParametro.ObtenerParametro("Pathfiles");

            Apikey = Utilidades.Desencriptar(objParametro.ObtenerParametro("Apikey"));
            
            DateTime Hoy = DateTime.Now;
            //dtDesde = Hoy.AddMonths(-1);            
            //dtDesde = DateTime.Parse(objParametro.ObtenerParametro("FecUltimaEjec"));
            //dtDesde = new DateTime(Hoy.Year, 1, 1);  
            
            //filtramos desde x cantidad de dias para atras hasta la fecha de hoy
            try
            {
                DateTime datetime = DateTime.Now;
                int diaActual = (int)datetime.DayOfWeek;
                if (diaActual != 5) // L A J
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
                if (_TopeDiasTimeSheet > 0)
                {
                    Desde = DateTime.Today.AddDays(-_TopeDiasTimeSheet);
                    Hasta = Hoy;
                }
                else
                {
                    Desde = DateTime.Parse(objParametro.ObtenerParametro("TimeSheetFechaDesde"));
                    Hasta = DateTime.Parse(objParametro.ObtenerParametro("TimeSheetFechaHasta"));
                }
            }

            
            objParametro = null;

            //Hoy = new DateTime(2018, 07, 1);
            //this.dtDesde = Hoy;
            //Hoy = new DateTime(2018, 07, 31);
            //this.dtHasta = Hoy;
        }

        public clsTimeSheet(string _strApikey, DateTime _dtDesde, DateTime _dtHasta)
        {
            Apikey = _strApikey;
            Desde = _dtDesde;
            Hasta = _dtHasta;
        }

        public string Apikey { get; set; } = "";
        public string RutaDestino { get; set; } = "";
        public DateTime Desde { get; set; } = DateTime.Now;
        public DateTime Hasta { get; set; } = DateTime.Now;
        public string Mensaje { get; set; } = "";               

        /// <summary>
        /// Retorna el TimeSheet filtrando por fecha.
        /// </summary>
        public async Task<int> ObtenerTimeSheet()
        {
            clsJira objJira = new clsJira();
            if (!objJira.HorarioValido) {
                objJira = null;
                return -1;
            }

            await Task.Run(() => Utilidades.LogService("--------------------------------------------------------------------------------------------"));
            await Task.Run(() => Utilidades.LogService("INICIO Jira"));

            //bool timesheetOK = false;
            //int intentos = 1;
            //string strError = "";
            
            //MParametros objParametro = new MParametros();
            //string strPathFile = "";
            //string strFileTimeSheet = "";
            //strPathFile = objParametro.ObtenerParametro("Pathfiles");
            //objParametro = null;
            //strFileTimeSheet = strPathFile + "ImportsTimeSheet.txt";
            //File.WriteAllText(strFileTimeSheet, string.Empty);

            MTablasIn objTablasIn = new MTablasIn();
            objTablasIn.LimpiarTablasIN();

            // TIMESHEET SUELE DAR ERROR "The remote server returned an error: (503) Server Unavailable."
            //HAGO 5 INTENTOS
            //while (intentos<=5) {
            //    try
            //    {
            //        timesheetOK = await CrearArchivo();
            //        if (timesheetOK)
            //        {
            //            intentos=50;
            //        }
            //    }
            //    catch(Exception ex)
            //    {
            //        intentos++;
            //        strError = ex.Message;
            //    }                
            //}                        

            //if (!timesheetOK) await Task.Run(() => Utilidades.LogService("ObtenerTimeSheet() Error: " + strError));            
            //await Task.Run(() => Utilidades.LogService("ObtenerTimeSheet(): FIN"));

            //if (timesheetOK) objTablasIn.Timesheet_Parse_In();
            objTablasIn = null;

            return 1;
        }
       
        private async Task<string> DownloadStringWithTimeoutAsync(string url, int timeoutMilliseconds)
        {
            using (var httpClient = new HttpClient())
            {
                using (var cts = new CancellationTokenSource(timeoutMilliseconds))
                {
                    try
                    {
                        HttpResponseMessage response = await httpClient.GetAsync(url, cts.Token);
                        response.EnsureSuccessStatusCode();
                        return await response.Content.ReadAsStringAsync();
                    }
                    catch (OperationCanceledException ex) when (!cts.Token.IsCancellationRequested)
                    {
                        Utilidades.LogService("TIMESHEET OperationCanceledException: " + ex.Message);
                        throw new TimeoutException("El tiempo de espera para la solicitud ha expirado.");
                    }
                }
            }
        }

        private async Task<bool> CrearArchivo()        
        {
            try
            {
                string URL = "https://timesheet-plugin.herokuapp.com/api/1/exportData.csv?"; 
                string gt = "&";
                //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                // 18/02/2021 agregue &allUsers=true, sin este parametro solo trae el titulo
                //se saco gt + "moreFields=components"
                DateTime DesdeMenosUnDia = Desde.AddDays(-1);
                DateTime HastaMasUnDia = Hasta.AddDays(+1);
                string UrlFinal = "";
                MParametros objParametro = new MParametros();
                string sync_only_this_user = "";

                sync_only_this_user = objParametro.ObtenerParametro("SyncOnlyThisUserTimeSheet");
                if (sync_only_this_user != "")
                    UrlFinal = URL + "start=" + DesdeMenosUnDia.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + gt + "end=" + HastaMasUnDia.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + gt + "user=" + sync_only_this_user + gt + "Apikey=" + Apikey;
                else
                    UrlFinal = URL + "start=" + DesdeMenosUnDia.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + gt + "end=" + HastaMasUnDia.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + gt + "allUsers=true" + gt + "Apikey=" + Apikey;                                                

                Utilidades.LogService("TIMESHEET URL: " + UrlFinal);
                //Thread.Sleep(1000); // Pausa por 1000 milisegundos (1 segundo)
                int timeoutMilliseconds = 60000; // 60 seconds

                string json = await DownloadStringWithTimeoutAsync(UrlFinal, timeoutMilliseconds);
                //Console.WriteLine(json);

                //quito comas ya que rompe el bulk insert por ser el limitador.                                       

                json = json.Replace(", ", " ");
                json = json.Replace(" ,", " ");
                //quito comillas dobles
                json = json.Replace("\"", "");

                //MParametros objParametro = new MParametros();
                RutaDestino = objParametro.ObtenerParametro("Pathfiles");
                objParametro = null;

                System.IO.File.WriteAllText(RutaDestino + @"ImportsTimeSheet.txt", json, System.Text.Encoding.GetEncoding(1252));

                //GuardarArchivo();

                return true;
            }
            catch (TimeoutException ex)
            {
                Utilidades.LogService("TIMESHEET TimeoutException: " + ex.Message);
                throw ex;
            }
            catch (HttpRequestException ex)
            {
                Utilidades.LogService("TIMESHEET HttpRequestException: " + ex.Message);                
                throw ex;
            }
            catch (Exception ex)
            {
                Utilidades.LogService("TIMESHEET Error: " + ex.Message);                
                throw ex;
            }
            finally
            {
                //Debe guardar el archivo aunque la peticion haya fallado, esto para evitar dejar la tabla IN llena con datos 
                //de la ejecucion anterior.
                GuardarArchivo();
            }
        }        

        private void GuardarArchivo()
        {
            try
            {
                string strPathFile = "";
                MParametros objParametro = new MParametros();
                strPathFile = objParametro.ObtenerParametro("Pathfiles");
                strPathFile = strPathFile + "ImportsTimeSheet.txt";

                BulkInsertTimeSheet obj = new BulkInsertTimeSheet();                
                try
                {
                    obj.LoadCsvToDataTableAndBulkInsert(strPathFile);
                }
                catch (Exception ex)
                {
                    ex.Data.Clear();
                }                

                obj = null;
                
            }
            catch (Exception ex)
            {
                Utilidades.LogService("Error Carga GuardarArchivo(): " + ex.Message);
                throw ex;
            }
        }
    }
}
