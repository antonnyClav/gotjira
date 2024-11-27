using DBC;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace DAL
{
    public class BulkInsertLinks
    {
        // Número de registros por cada bulk insert
        int batchSize = 5000;
        public void LoadCsvToDataTableAndBulkInsert(string filePath)
        {
            // Step 1: Cargar el CSV en un DataTable
            DataTable dataTable = new DataTable();

            // Definir las columnas del DataTable según el archivo CSV
            dataTable.Columns.Add("key1", typeof(string));
            dataTable.Columns.Add("key2", typeof(string));

            // Leer el archivo CSV
            string[] allLines = File.ReadAllLines(filePath);

            // Leer el archivo CSV
            foreach (string line in allLines)
            {
                // Dividir la línea por comas
                string[] values = line.Split('|');

                // Crear una nueva fila en el DataTable
                DataRow row = dataTable.NewRow();

                row["key1"] = values[0];
                row["key2"] = values[1];

                // Añadir la fila al DataTable
                dataTable.Rows.Add(row);
            }

            // Step 2: Insertar los datos del DataTable en la base de datos usando SqlBulkCopy
            clsDatabaseCn con = new clsDatabaseCn();
            using (SqlConnection connection = con.Conectar())
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.DestinationTableName = "dbo.in_enlaces"; // Nombre de la tabla de destino

                    // Mapear las columnas del DataTable con las de la tabla SQL Server
                    bulkCopy.ColumnMappings.Add("key1", "key1");
                    bulkCopy.ColumnMappings.Add("key2", "key2");

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
                            Console.WriteLine($"Enlaces Batch de {batchTable.Rows.Count} registros insertado correctamente.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Enlaces Error al insertar el batch: {ex.Message}");
                        }
                    }
                    // Insertar los datos en SQL Server
                    //bulkCopy.WriteToServer(dataTable);
                }
            }            
        }        
    }
}
