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
        public void LoadCsvToDataTableAndBulkInsert(string filePath)
        {
            // Step 1: Cargar el CSV en un DataTable
            DataTable dataTable = new DataTable();

            // Definir las columnas del DataTable según el archivo CSV
            dataTable.Columns.Add("project", typeof(string));
            dataTable.Columns.Add("_key", typeof(string));
            dataTable.Columns.Add("issue_type", typeof(string));
            dataTable.Columns.Add("sumary", typeof(string));
            dataTable.Columns.Add("epic_link", typeof(string));
            dataTable.Columns.Add("ga", typeof(string));
            dataTable.Columns.Add("sprint", typeof(string));
            dataTable.Columns.Add("parent_issue", typeof(string));
            dataTable.Columns.Add("origen_error", typeof(string));
            dataTable.Columns.Add("fase_detectado", typeof(string));
            dataTable.Columns.Add("cliente", typeof(string));
            dataTable.Columns.Add("status", typeof(string));
            dataTable.Columns.Add("priority", typeof(string));
            dataTable.Columns.Add("category", typeof(string));
            dataTable.Columns.Add("project_custom", typeof(string));
            dataTable.Columns.Add("horas_estimadas", typeof(string));
            dataTable.Columns.Add("story_points", typeof(string));
            dataTable.Columns.Add("feature", typeof(string));

            // Leer el archivo CSV            
            string[] allLines = File.ReadAllLines(filePath);

            // Leer el archivo CSV
            foreach (string line in allLines)
            {
                // Dividir la línea por comas
                string[] values = line.Split('|');

                // Crear una nueva fila en el DataTable
                DataRow row = dataTable.NewRow();

                row["project"] = values[0];
                row["_key"] = values[1];
                row["issue_type"] = values[2];
                row["sumary"] = values[3];
                row["epic_link"] = values[4];
                row["ga"] = values[5];
                row["sprint"] = values[6];
                row["parent_issue"] = values[7];
                row["origen_error"] = values[8];
                row["fase_detectado"] = values[9];
                row["cliente"] = values[10];
                row["status"] = values[11];
                row["priority"] = values[12];
                row["category"] = values[13];
                row["project_custom"] = values[14];
                row["horas_estimadas"] = values[15];
                row["story_points"] = values[16];
                row["feature"] = values[17];

                // Añadir la fila al DataTable
                dataTable.Rows.Add(row);
            }

            // Step 2: Insertar los datos del DataTable en la base de datos usando SqlBulkCopy
            clsDatabaseCn con = new clsDatabaseCn();
            using (SqlConnection connection = con.Conectar())
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
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
            }            
        }        
    }
}
