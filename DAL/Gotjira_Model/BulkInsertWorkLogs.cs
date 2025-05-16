using DBC;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace DAL
{
    public class BulkInsertWorkLogs
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
                            bulkCopy.DestinationTableName = "dbo.in_worklogs"; // Nombre de la tabla de destino

                            // Mapear las columnas del DataTable con las de la tabla SQL Server
                            bulkCopy.ColumnMappings.Add("project_key", "project_key");
                            bulkCopy.ColumnMappings.Add("issue_key", "issue_key");
                            bulkCopy.ColumnMappings.Add("issuetype", "issuetype");
                            bulkCopy.ColumnMappings.Add("summary", "summary");
                            bulkCopy.ColumnMappings.Add("priority_name", "priority_name");
                            bulkCopy.ColumnMappings.Add("started", "started");
                            bulkCopy.ColumnMappings.Add("displayname", "displayname");
                            bulkCopy.ColumnMappings.Add("timespentseconds", "timespentseconds");
                            bulkCopy.ColumnMappings.Add("comment", "comment");
                            bulkCopy.ColumnMappings.Add("timespent", "timespent");
                            bulkCopy.ColumnMappings.Add("created", "created");
                            bulkCopy.ColumnMappings.Add("updated", "updated");
                            bulkCopy.ColumnMappings.Add("id", "id");
                            bulkCopy.ColumnMappings.Add("issueid", "issueid");


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
                                    Console.WriteLine($"WorkLogs Batch de {batchTable.Rows.Count} registros insertado correctamente.");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"WorkLogs Error al insertar el batch: {ex.Message}");
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
                        Console.WriteLine($"WorkLogs Error SqlBulkCopy: {ex.Message}");
                        throw ex;
                    }
                }
            }
        }
    }
}
