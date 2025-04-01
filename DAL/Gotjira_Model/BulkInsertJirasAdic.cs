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
        int batchSize = 5000;
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
            dataTable.Columns.Add("soporte_asignado_a", typeof(string));
            dataTable.Columns.Add("resolutor", typeof(string));
            dataTable.Columns.Add("ambiente", typeof(string));
            dataTable.Columns.Add("entorno", typeof(string));
            dataTable.Columns.Add("informacion_insuficiente", typeof(string));
            dataTable.Columns.Add("priorizado_gdp", typeof(string));
            dataTable.Columns.Add("indicar_si_es_core", typeof(string));
            dataTable.Columns.Add("jira_control_interno", typeof(string));


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
                row["soporte_asignado_a"] = values[38];
                row["resolutor"] = values[39];
                row["ambiente"] = values[40];
                row["entorno"] = values[41];
                row["informacion_insuficiente"] = values[42];
                row["priorizado_gdp"] = values[43];
                row["indicar_si_es_core"] = values[44];
                row["jira_control_interno"] = values[45];


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
                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
                        {
                            bulkCopy.DestinationTableName = "dbo.in_jiras_adic"; // Nombre de la tabla de destino

                            // Mapear las columnas del DataTable con las de la tabla SQL Server
                            bulkCopy.ColumnMappings.Add("_key", "_key");
                            bulkCopy.ColumnMappings.Add("created", "created");
                            bulkCopy.ColumnMappings.Add("updated", "updated");
                            bulkCopy.ColumnMappings.Add("resolutiondate", "resolutiondate");
                            bulkCopy.ColumnMappings.Add("lastViewed", "lastViewed");
                            bulkCopy.ColumnMappings.Add("area_solicitante", "area_solicitante");
                            bulkCopy.ColumnMappings.Add("informador", "informador");
                            bulkCopy.ColumnMappings.Add("calificacion", "calificacion");
                            bulkCopy.ColumnMappings.Add("request_type", "request_type");
                            bulkCopy.ColumnMappings.Add("sub_categoria", "sub_categoria");
                            bulkCopy.ColumnMappings.Add("assignee", "assignee");
                            bulkCopy.ColumnMappings.Add("tipo_falla", "tipo_falla");
                            bulkCopy.ColumnMappings.Add("motivo_de_pendiente", "motivo_de_pendiente");
                            bulkCopy.ColumnMappings.Add("sla1", "sla1");
                            bulkCopy.ColumnMappings.Add("sla2", "sla2");
                            bulkCopy.ColumnMappings.Add("sla3", "sla3");
                            bulkCopy.ColumnMappings.Add("sla4", "sla4");
                            bulkCopy.ColumnMappings.Add("cliente", "cliente");
                            bulkCopy.ColumnMappings.Add("origen", "origen");
                            bulkCopy.ColumnMappings.Add("producto", "producto");
                            bulkCopy.ColumnMappings.Add("nro_oportunidad", "nro_oportunidad");
                            bulkCopy.ColumnMappings.Add("tipo_soporte", "tipo_soporte");
                            bulkCopy.ColumnMappings.Add("causa_raiz", "causa_raiz");
                            bulkCopy.ColumnMappings.Add("nivel_resolucion", "nivel_resolucion");
                            bulkCopy.ColumnMappings.Add("tipo_riesgo", "tipo_riesgo");
                            bulkCopy.ColumnMappings.Add("proceso_vinculado", "proceso_vinculado");
                            bulkCopy.ColumnMappings.Add("probabilidad", "probabilidad");
                            bulkCopy.ColumnMappings.Add("impacto_ryo", "impacto_ryo");
                            bulkCopy.ColumnMappings.Add("estrategia_ryo", "estrategia_ryo");
                            bulkCopy.ColumnMappings.Add("impacto", "impacto");
                            bulkCopy.ColumnMappings.Add("influencia", "influencia");
                            bulkCopy.ColumnMappings.Add("estrategia", "estrategia");
                            bulkCopy.ColumnMappings.Add("clasificacion_contexto", "clasificacion_contexto");
                            bulkCopy.ColumnMappings.Add("origen2", "origen2");
                            bulkCopy.ColumnMappings.Add("origen_hallazgo", "origen_hallazgo");
                            bulkCopy.ColumnMappings.Add("accion_correctiva_cuando", "accion_correctiva_cuando");
                            bulkCopy.ColumnMappings.Add("fecha_verificacion_eficacia", "fecha_verificacion_eficacia");
                            bulkCopy.ColumnMappings.Add("complejidad", "complejidad");
                            bulkCopy.ColumnMappings.Add("soporte_asignado_a", "soporte_asignado_a");
                            bulkCopy.ColumnMappings.Add("resolutor", "resolutor");
                            bulkCopy.ColumnMappings.Add("ambiente", "ambiente");
                            bulkCopy.ColumnMappings.Add("entorno", "entorno");
                            bulkCopy.ColumnMappings.Add("informacion_insuficiente", "informacion_insuficiente");
                            bulkCopy.ColumnMappings.Add("priorizado_gdp", "priorizado_gdp");
                            bulkCopy.ColumnMappings.Add("indicar_si_es_core", "indicar_si_es_core");
                            bulkCopy.ColumnMappings.Add("jira_control_interno", "jira_control_interno");

                            // Procesar en lotes de xxx registros
                            for (int i = 0; i < dataTable.Rows.Count; i += batchSize)
                            {
                                DataTable batchTable = dataTable.Clone(); // Clonamos solo la estructura del DataTable

                                for (int j = i; j < i + batchSize && j < dataTable.Rows.Count; j++)
                                {
                                    batchTable.ImportRow(dataTable.Rows[j]);
                                }

                                try
                                {
                                    // Realizar el BulkCopy para el batch actual
                                    bulkCopy.WriteToServer(batchTable);
                                    Console.WriteLine($"JirasAdic Batch de {batchTable.Rows.Count} registros insertado correctamente.");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"JirasAdic Error al insertar el batch: {ex.Message}");
                                }
                            }
                            // Insertar los datos en SQL Server
                            //bulkCopy.WriteToServer(dataTable);
                        }

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
