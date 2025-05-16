using DBC;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace DAL
{
    public class BulkInsertJirasAdicGDD
    {
        // Número de registros por cada bulk insert
        int batchSize = 5000;

        public void DataTaleToBulkInsert(DataTable dataTable)
        {                    
            clsDatabaseCn con = new clsDatabaseCn();
            using (SqlConnection connection = con.Conectar())
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.DestinationTableName = "dbo.in_jiras_adic_gdd"; // Nombre de la tabla de destino

                    // Mapear las columnas del DataTable con las de la tabla SQL Server
                    bulkCopy.ColumnMappings.Add("_key", "_key");
                    bulkCopy.ColumnMappings.Add("componente", "componente");
                    bulkCopy.ColumnMappings.Add("cliente", "cliente");
                    bulkCopy.ColumnMappings.Add("origen", "origen");
                    bulkCopy.ColumnMappings.Add("equipo", "equipo");
                    bulkCopy.ColumnMappings.Add("facturable", "facturable");
                    bulkCopy.ColumnMappings.Add("producto", "producto");
                    bulkCopy.ColumnMappings.Add("version", "version");
                    bulkCopy.ColumnMappings.Add("tipo", "tipo");
                    bulkCopy.ColumnMappings.Add("horas_estimadas", "horas_estimadas");
                    bulkCopy.ColumnMappings.Add("tipo_desarrollo", "tipo_desarrollo");
                    bulkCopy.ColumnMappings.Add("estado_desarrollo", "estado_desarrollo");
                    bulkCopy.ColumnMappings.Add("fecha_requerida", "fecha_requerida");
                    bulkCopy.ColumnMappings.Add("fecha_estimacion", "fecha_estimacion");
                    bulkCopy.ColumnMappings.Add("fecha_entrega", "fecha_entrega");

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
                            Console.WriteLine($"JirasGDD Batch de {batchTable.Rows.Count} registros insertado correctamente.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"JirasGDD Error al insertar el batch: {ex.Message}");
                        }
                    }
                    // Insertar los datos en SQL Server
                    //bulkCopy.WriteToServer(dataTable);
                }
            }
        }
    }

}
