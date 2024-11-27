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
        public void LoadCsvToDataTableAndBulkInsert(string filePath)
        {
            // Step 1: Cargar el CSV en un DataTable
            DataTable dataTable = new DataTable();

            // Definir las columnas del DataTable según el archivo CSV
            dataTable.Columns.Add("_key", typeof(string));
            dataTable.Columns.Add("componente", typeof(string));
            dataTable.Columns.Add("cliente", typeof(string));
            dataTable.Columns.Add("origen", typeof(string));
            dataTable.Columns.Add("equipo", typeof(string));
            dataTable.Columns.Add("facturable", typeof(string));
            dataTable.Columns.Add("producto", typeof(string));
            dataTable.Columns.Add("version", typeof(string));
            dataTable.Columns.Add("tipo", typeof(string));
            dataTable.Columns.Add("horas_estimadas", typeof(string));
            dataTable.Columns.Add("tipo_desarrollo", typeof(string));
            dataTable.Columns.Add("estado_desarrollo", typeof(string));
            dataTable.Columns.Add("fecha_requerida", typeof(string));
            dataTable.Columns.Add("fecha_estimacion", typeof(string));
            dataTable.Columns.Add("fecha_entrega", typeof(string));

            // Leer el archivo CSV
            string[] allLines = File.ReadAllLines(filePath);

            // Leer el archivo CSV
            foreach (string line in allLines)
            {
                // Dividir la línea por comas
                string[] values = line.Split('|');

                // Crear una nueva fila en el DataTable
                DataRow row = dataTable.NewRow();

                row["_key"] = values[0];
                row["componente"] = values[1];
                row["cliente"] = values[2];
                row["origen"] = values[3];
                row["equipo"] = values[4];
                row["facturable"] = values[5];
                row["producto"] = values[6];
                row["version"] = values[7];
                row["tipo"] = values[8];
                row["horas_estimadas"] = values[9];
                row["tipo_desarrollo"] = values[10];
                row["estado_desarrollo"] = values[11];
                row["fecha_requerida"] = values[12];
                row["fecha_estimacion"] = values[13];
                row["fecha_entrega"] = values[14];

                // Añadir la fila al DataTable
                dataTable.Rows.Add(row);
            }

            // Step 2: Insertar los datos del DataTable en la base de datos usando SqlBulkCopy
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
