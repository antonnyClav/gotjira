using DBC;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace DAL
{
    public class BulkInsertProjectsXComp
    {
        public void LoadCsvToDataTableAndBulkInsert(string filePath)
        {
            // Step 1: Cargar el CSV en un DataTable
            DataTable dataTable = new DataTable();

            // Definir las columnas del DataTable según el archivo CSV
            dataTable.Columns.Add("project", typeof(string));
            dataTable.Columns.Add("componente", typeof(string));

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
                row["componente"] = values[1];

                // Añadir la fila al DataTable
                dataTable.Rows.Add(row);
            }

            // Step 2: Insertar los datos del DataTable en la base de datos usando SqlBulkCopy
            clsDatabaseCn con = new clsDatabaseCn();
            using (SqlConnection connection = con.Conectar())
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.DestinationTableName = "dbo.in_proyectos_x_componentes"; // Nombre de la tabla de destino

                    // Mapear las columnas del DataTable con las de la tabla SQL Server
                    bulkCopy.ColumnMappings.Add("project", "project");
                    bulkCopy.ColumnMappings.Add("componente", "componente");

                    // Insertar los datos en SQL Server
                    bulkCopy.WriteToServer(dataTable);
                }
            }            
        }
    }
}
