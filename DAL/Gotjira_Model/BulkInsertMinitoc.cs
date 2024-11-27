using DBC;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace DAL
{
    public class BulkInsertMinitoc
    {
        // Número de registros por cada bulk insert
        int batchSize = 5000;
        public void LoadCsvToDataTableAndBulkInsert(string filePath)
        {
            // Step 1: Cargar el CSV en un DataTable
            DataTable dataTable = new DataTable();

            // Definir las columnas del DataTable según el archivo CSV
            dataTable.Columns.Add("linea", typeof(string));

            // Leer el archivo CSV
            string[] allLines = File.ReadAllLines(filePath);

            // Leer el archivo CSV
            foreach (string line in allLines)
            {
                // Crear una nueva fila en el DataTable
                DataRow row = dataTable.NewRow();

                row["linea"] = line;

                // Añadir la fila al DataTable
                dataTable.Rows.Add(row);
            }

            // Step 2: Insertar los datos del DataTable en la base de datos usando SqlBulkCopy
            clsDatabaseCn con = new clsDatabaseCn();
            using (SqlConnection connection = con.Conectar())
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.DestinationTableName = "dbo.in_minitoc_aux"; // Nombre de la tabla de destino

                    // Mapear las columnas del DataTable con las de la tabla SQL Server
                    bulkCopy.ColumnMappings.Add("linea", "linea");

                    // Insertar los datos en SQL Server
                    bulkCopy.WriteToServer(dataTable);
                }
            }          

            DateTime fecha_hoy = DateTime.Now;
            string directoryPath = Path.GetDirectoryName(filePath);
            string destinationFile = directoryPath + @"\Minitoc_Procesados\" + fecha_hoy.ToString("ddMMyyyy")  + "_minitoc.csv";  

            try
            {
                // Mover el archivo
                File.Move(filePath, destinationFile);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error al mover el archivo: {ex.Message}");
                throw ex;                
            }
        }        
    }
}
