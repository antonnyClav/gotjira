using DBC;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace DAL
{
    public class BulkInsertTimeSheet
    {
        // Número de registros por cada bulk insert
        int batchSize = 5000;
        public void LoadCsvToDataTableAndBulkInsert(string filePath)
        {
            // Step 1: Cargar el CSV en un DataTable
            DataTable dataTable = new DataTable();

            // Definir las columnas del DataTable según el archivo CSV
            dataTable.Columns.Add("Project", typeof(string));
            dataTable.Columns.Add("Issue_Type", typeof(string));
            dataTable.Columns.Add("_Key", typeof(string));
            dataTable.Columns.Add("Summary", typeof(string));
            dataTable.Columns.Add("Priority", typeof(string));
            dataTable.Columns.Add("Date_Started", typeof(string));
            dataTable.Columns.Add("Display_Name", typeof(string));
            dataTable.Columns.Add("Time_Spent", typeof(string));
            dataTable.Columns.Add("Work_Description", typeof(string));

            // Leer el archivo CSV
            string[] allLines = File.ReadAllLines(filePath);

            // Omitir la primera y la última fila
            var linesToProcess = allLines.Skip(1).Take(allLines.Length - 2);  // Ignorar la primera y la última fila

            // Leer el archivo CSV
            foreach (string line in linesToProcess)
            {
                // Dividir la línea por comas
                string[] values = line.Split(',');

                // Crear una nueva fila en el DataTable
                DataRow row = dataTable.NewRow();

                row["Project"] = values[0];
                row["Issue_Type"] = values[1];
                row["_Key"] = values[2];
                row["Summary"] = values[3];
                row["Priority"] = values[4];
                row["Date_Started"] = values[5];  
                row["Display_Name"] = values[6];
                row["Time_Spent"] = values[7];  
                row["Work_Description"] = values[8];

                // Añadir la fila al DataTable
                dataTable.Rows.Add(row);
            }

            // Step 2: Insertar los datos del DataTable en la base de datos usando SqlBulkCopy
            clsDatabaseCn con = new clsDatabaseCn();
            using (SqlConnection connection = con.Conectar())
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.DestinationTableName = "dbo.in_timesheet"; // Nombre de la tabla de destino

                    // Mapear las columnas del DataTable con las de la tabla SQL Server
                    bulkCopy.ColumnMappings.Add("Project", "Project");
                    bulkCopy.ColumnMappings.Add("Issue_Type", "Issue_Type");
                    bulkCopy.ColumnMappings.Add("_Key", "_Key");
                    bulkCopy.ColumnMappings.Add("Summary", "Summary");
                    bulkCopy.ColumnMappings.Add("Priority", "Priority");
                    bulkCopy.ColumnMappings.Add("Date_Started", "Date_Started");
                    bulkCopy.ColumnMappings.Add("Display_Name", "Display_Name");
                    bulkCopy.ColumnMappings.Add("Time_Spent", "Time_Spent");
                    bulkCopy.ColumnMappings.Add("Work_Description", "Work_Description");

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
                            Console.WriteLine($"TimeSheet Batch de {batchTable.Rows.Count} registros insertado correctamente.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"TimeSheet Error al insertar el batch: {ex.Message}");
                        }
                    }
                    // Insertar los datos en SQL Server
                    //bulkCopy.WriteToServer(dataTable);
                }
            }            
        }        
    }
}
