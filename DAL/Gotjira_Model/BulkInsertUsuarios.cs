﻿using DBC;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace DAL
{
    public class BulkInsertUsuarios
    {
        public void LoadCsvToDataTableAndBulkInsert(string filePath)
        {
            // Step 1: Cargar el CSV en un DataTable
            DataTable dataTable = new DataTable();

            // Definir las columnas del DataTable según el archivo CSV
            dataTable.Columns.Add("displayname", typeof(string));
            dataTable.Columns.Add("email", typeof(string));
            dataTable.Columns.Add("username", typeof(string));
            dataTable.Columns.Add("isActive", typeof(string));
            dataTable.Columns.Add("accountId", typeof(string));

            // Leer el archivo CSV
            string[] allLines = File.ReadAllLines(filePath);

            // Leer el archivo CSV
            foreach (string line in allLines)
            {
                // Dividir la línea por comas
                string[] values = line.Split('|');

                // Crear una nueva fila en el DataTable
                DataRow row = dataTable.NewRow();

                row["displayname"] = values[0];
                row["email"] = values[1];
                row["username"] = values[2];
                row["isActive"] = values[3];
                row["accountId"] = values[4];

                // Añadir la fila al DataTable
                dataTable.Rows.Add(row);
            }

            // Step 2: Insertar los datos del DataTable en la base de datos usando SqlBulkCopy
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
