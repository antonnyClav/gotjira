using DBC;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace DAL
{
    public class BulkInsertProjects
    {
        public void LoadCsvToDataTableAndBulkInsert(string filePath)
        {
            // Step 1: Cargar el CSV en un DataTable
            DataTable dataTable = new DataTable();

            // Definir las columnas del DataTable según el archivo CSV
            dataTable.Columns.Add("categoria", typeof(string));
            dataTable.Columns.Add("nombre", typeof(string));
            dataTable.Columns.Add("_key", typeof(string));
            dataTable.Columns.Add("lider", typeof(string));

            // Leer el archivo CSV
            string[] allLines = File.ReadAllLines(filePath);

            // Leer el archivo CSV
            foreach (string line in allLines)
            {
                // Dividir la línea por comas
                string[] values = line.Split('|');

                // Crear una nueva fila en el DataTable
                DataRow row = dataTable.NewRow();

                row["categoria"] = values[0];
                row["nombre"] = values[1];
                row["_key"] = values[2];
                row["lider"] = values[3];

                // Añadir la fila al DataTable
                dataTable.Rows.Add(row);
            }            
            
            clsDatabaseCn con = new clsDatabaseCn();
             
            // Step 2: Insertar los datos del DataTable en la base de datos usando SqlBulkCopy
            using (SqlConnection connection = con.Conectar())
            {
                //// Paso 3: Limpiar la tabla
                //using (SqlCommand deleteCommand = new SqlCommand("TRUNCATE TABLE dbo.in_proyectos", connection))
                //{
                //    deleteCommand.ExecuteNonQuery();
                //}

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.DestinationTableName = "dbo.in_proyectos"; // Nombre de la tabla de destino

                    // Mapear las columnas del DataTable con las de la tabla SQL Server
                    bulkCopy.ColumnMappings.Add("categoria", "categoria");
                    bulkCopy.ColumnMappings.Add("nombre", "nombre");
                    bulkCopy.ColumnMappings.Add("_key", "_key");
                    bulkCopy.ColumnMappings.Add("lider", "lider");

                    // Insertar los datos en SQL Server
                    bulkCopy.WriteToServer(dataTable);
                }
            }
        }
    }
}
