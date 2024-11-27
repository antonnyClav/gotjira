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

        public void LoadCsvToDataTableAndBulkInsert(string filePath)
        {
            // Step 1: Cargar el CSV en un DataTable
            DataTable dataTable = new DataTable();

            // Definir las columnas del DataTable según el archivo CSV
            dataTable.Columns.Add("project_key", typeof(string));
            dataTable.Columns.Add("issue_key", typeof(string));
            dataTable.Columns.Add("issuetype", typeof(string));
            dataTable.Columns.Add("summary", typeof(string));
            dataTable.Columns.Add("priority_name", typeof(string));
            dataTable.Columns.Add("started", typeof(string));
            dataTable.Columns.Add("displayname", typeof(string));
            dataTable.Columns.Add("timespentseconds", typeof(string));
            dataTable.Columns.Add("comment", typeof(string));
            dataTable.Columns.Add("timespent", typeof(string));
            dataTable.Columns.Add("created", typeof(string));
            dataTable.Columns.Add("updated", typeof(string));
            dataTable.Columns.Add("id", typeof(string));
            dataTable.Columns.Add("issueid", typeof(string));

            // Leer el archivo CSV
            string[] allLines = File.ReadAllLines(filePath);

            // Leer el archivo CSV
            foreach (string line in allLines)
            {
                // Dividir la línea por comas
                string[] values = line.Split('|');

                // Crear una nueva fila en el DataTable
                DataRow row = dataTable.NewRow();

                row["project_key"] = values[0];
                row["issue_key"] = values[1];
                row["issuetype"] = values[2];
                row["summary"] = values[3];
                row["priority_name"] = values[4];
                row["started"] = values[5];
                row["displayname"] = values[6];
                row["timespentseconds"] = values[7];
                row["comment"] = values[8];
                row["timespent"] = values[9];
                row["created"] = values[10];
                row["updated"] = values[11];
                row["id"] = values[12];
                row["issueid"] = values[13];

                // Añadir la fila al DataTable
                dataTable.Rows.Add(row);
            }

            // Step 2: Insertar los datos del DataTable en la base de datos usando SqlBulkCopy
            clsDatabaseCn con = new clsDatabaseCn();
            using (SqlConnection connection = con.Conectar())
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
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
            }            
        }       
    }
}
