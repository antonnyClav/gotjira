using DBC;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace DAL
{
    public class BulkInsertJiras
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
                            bulkCopy.DestinationTableName = "dbo.in_jiras"; // Nombre de la tabla de destino

                            // Mapear las columnas del DataTable con las de la tabla SQL Server
                            bulkCopy.ColumnMappings.Add("project", "project");
                            bulkCopy.ColumnMappings.Add("_key", "_key");
                            bulkCopy.ColumnMappings.Add("issue_type", "issue_type");
                            bulkCopy.ColumnMappings.Add("sumary", "sumary");
                            bulkCopy.ColumnMappings.Add("epic_link", "epic_link");
                            bulkCopy.ColumnMappings.Add("ga", "ga");
                            bulkCopy.ColumnMappings.Add("sprint", "sprint");
                            bulkCopy.ColumnMappings.Add("parent_issue", "parent_issue");
                            bulkCopy.ColumnMappings.Add("origen_error", "origen_error");
                            bulkCopy.ColumnMappings.Add("fase_detectado", "fase_detectado");
                            bulkCopy.ColumnMappings.Add("cliente", "cliente");
                            bulkCopy.ColumnMappings.Add("status", "status");
                            bulkCopy.ColumnMappings.Add("priority", "priority");
                            bulkCopy.ColumnMappings.Add("category", "category");
                            bulkCopy.ColumnMappings.Add("project_custom", "project_custom");
                            bulkCopy.ColumnMappings.Add("horas_estimadas", "horas_estimadas");
                            bulkCopy.ColumnMappings.Add("story_points", "story_points");
                            bulkCopy.ColumnMappings.Add("feature", "feature");

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
                                    Console.WriteLine($"Jiras Batch de {batchTable.Rows.Count} registros insertado correctamente.");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Jiras Error al insertar el batch: {ex.Message}");
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
                        Console.WriteLine($"Jiras Error SqlBulkCopy: {ex.Message}");
                        throw ex;
                    }
                }
            }
        }
    }
}
