using DBC;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace DAL
{
    public class BulkInsertJirasAdic
    {
        // Número de registros por cada bulk insert
        //int batchSize = 5000;
        public void LoadCsvToDataTableAndBulkInsert(string filePath)
        {
            // Step 1: Cargar el CSV en un DataTable
            DataTable dataTable = new DataTable();

            // Definir las columnas del DataTable según el archivo CSV
            dataTable.Columns.Add("_key", typeof(string));
            dataTable.Columns.Add("created", typeof(string));
            dataTable.Columns.Add("updated", typeof(string));
            dataTable.Columns.Add("resolutiondate", typeof(string));
            dataTable.Columns.Add("lastViewed", typeof(string));
            dataTable.Columns.Add("area_solicitante", typeof(string));
            dataTable.Columns.Add("informador", typeof(string));
            dataTable.Columns.Add("calificacion", typeof(string));
            dataTable.Columns.Add("request_type", typeof(string));
            dataTable.Columns.Add("sub_categoria", typeof(string));
            dataTable.Columns.Add("assignee", typeof(string));
            dataTable.Columns.Add("tipo_falla", typeof(string));
            dataTable.Columns.Add("motivo_de_pendiente", typeof(string));
            dataTable.Columns.Add("sla1", typeof(string));
            dataTable.Columns.Add("sla2", typeof(string));
            dataTable.Columns.Add("sla3", typeof(string));
            dataTable.Columns.Add("sla4", typeof(string));
            dataTable.Columns.Add("cliente", typeof(string));
            dataTable.Columns.Add("origen", typeof(string));
            dataTable.Columns.Add("producto", typeof(string));
            dataTable.Columns.Add("nro_oportunidad", typeof(string));
            dataTable.Columns.Add("tipo_soporte", typeof(string));
            dataTable.Columns.Add("causa_raiz", typeof(string));
            dataTable.Columns.Add("nivel_resolucion", typeof(string));
            dataTable.Columns.Add("tipo_riesgo", typeof(string));
            dataTable.Columns.Add("proceso_vinculado", typeof(string));
            dataTable.Columns.Add("probabilidad", typeof(string));
            dataTable.Columns.Add("impacto_ryo", typeof(string));
            dataTable.Columns.Add("estrategia_ryo", typeof(string));
            dataTable.Columns.Add("impacto", typeof(string));
            dataTable.Columns.Add("influencia", typeof(string));
            dataTable.Columns.Add("estrategia", typeof(string));
            dataTable.Columns.Add("clasificacion_contexto", typeof(string));
            dataTable.Columns.Add("origen2", typeof(string));
            dataTable.Columns.Add("origen_hallazgo", typeof(string));
            dataTable.Columns.Add("accion_correctiva_cuando", typeof(string));
            dataTable.Columns.Add("fecha_verificacion_eficacia", typeof(string));
            dataTable.Columns.Add("complejidad", typeof(string));

            // Leer el archivo CSV
            string[] allLines = File.ReadAllLines(filePath);

            // Leer el archivo CSV
            foreach (string line in allLines)
            {
                // Dividir la línea por comas
                string[] values = line.Split('|');

                // Crear una nueva fila en el DataTable
                DataRow row = dataTable.NewRow();

                row["_key"] = values[0];
                row["created"] = values[1];
                row["updated"] = values[2];
                row["resolutiondate"] = values[3];
                row["lastViewed"] = values[4];
                row["area_solicitante"] = values[5];
                row["informador"] = values[6];
                row["calificacion"] = values[7];
                row["request_type"] = values[8];
                row["sub_categoria"] = values[9];
                row["assignee"] = values[10];
                row["tipo_falla"] = values[11];
                row["motivo_de_pendiente"] = values[12];
                row["sla1"] = values[13];
                row["sla2"] = values[14];
                row["sla3"] = values[15];
                row["sla4"] = values[16];
                row["cliente"] = values[17];
                row["origen"] = values[18];
                row["producto"] = values[19];
                row["nro_oportunidad"] = values[20];
                row["tipo_soporte"] = values[21];
                row["causa_raiz"] = values[22];
                row["nivel_resolucion"] = values[23];
                row["tipo_riesgo"] = values[24];
                row["proceso_vinculado"] = values[25];
                row["probabilidad"] = values[26];
                row["impacto_ryo"] = values[27];
                row["estrategia_ryo"] = values[28];
                row["impacto"] = values[29];
                row["influencia"] = values[30];
                row["estrategia"] = values[31];
                row["clasificacion_contexto"] = values[32];
                row["origen2"] = values[33];
                row["origen_hallazgo"] = values[34];
                row["accion_correctiva_cuando"] = values[35];
                row["fecha_verificacion_eficacia"] = values[36];
                row["complejidad"] = values[37];

                // Añadir la fila al DataTable
                dataTable.Rows.Add(row);
            }

            // Step 2: Insertar los datos del DataTable en la base de datos usando SqlBulkCopy
            clsDatabaseCn con = new clsDatabaseCn();
            using (SqlConnection connection = con.Conectar())
            {

                using (SqlTransaction transaction = connection.BeginTransaction()) // Inicia la transacción
                {
                    try
                    {
                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))

                        transaction.Commit(); // Si todo va bien, se confirma la inserción
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback(); // Si hay error, deshacer todo
                        Console.WriteLine($"JirasAdic Error SqlBulkCopy: {ex.Message}");
                        throw ex;
                    }
                }
            }            
        }
    }
}
