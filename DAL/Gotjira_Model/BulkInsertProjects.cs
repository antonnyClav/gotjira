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
        public void DataTaleToBulkInsert(DataTable dataTable)
        {
            clsDatabaseCn con = new clsDatabaseCn();

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

            dataTable.Clear();
            dataTable.Dispose();
        }
    }
}
