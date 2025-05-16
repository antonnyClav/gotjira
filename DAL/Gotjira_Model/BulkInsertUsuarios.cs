using DBC;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace DAL
{
    public class BulkInsertUsuarios
    {
        public void DataTaleToBulkInsert(DataTable dataTable)
        {
            clsDatabaseCn con = new clsDatabaseCn();

            using (SqlConnection connection = con.Conectar())
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.DestinationTableName = "dbo.in_usuarios"; // Nombre de la tabla de destino

                    // Mapear las columnas del DataTable con las de la tabla SQL Server
                    bulkCopy.ColumnMappings.Add("displayname", "displayname");
                    bulkCopy.ColumnMappings.Add("email", "email");
                    bulkCopy.ColumnMappings.Add("username", "username");
                    bulkCopy.ColumnMappings.Add("isActive", "isActive");
                    bulkCopy.ColumnMappings.Add("accountId", "accountId");

                    // Insertar los datos en SQL Server
                    bulkCopy.WriteToServer(dataTable);
                }
            }
        }
    }
}
