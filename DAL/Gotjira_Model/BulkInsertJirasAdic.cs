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

        public void DataTaleToBulkInsert(DataTable dataTable)
        {
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
