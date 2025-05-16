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
        public void DataTaleToBulkInsert(DataTable dataTable)
        {
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
