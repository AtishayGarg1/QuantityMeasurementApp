using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using QuantityMeasurementModel.Entities;
using QuantityMeasurementRepository.Interfaces;

namespace QuantityMeasurementRepository.Repositories
{
    // Database repository that persists measurement records using ADO.NET (SqlClient)
    // Only supports INSERT operations to the database for audit trail persistence
    // GetAllMeasurements returns an empty list since retrieval is not required from DB
    public class QuantityMeasurementDatabaseRepository : IQuantityMeasurementRepository
    {
        // Connection string used to connect to SQL Server
        private readonly string _connectionString;

        // Constructor accepts connection string and ensures the database and table exist
        public QuantityMeasurementDatabaseRepository(string connectionString)
        {
            _connectionString = connectionString;

            // Automatically create the database and table if they do not exist
            EnsureTableCreated();
        }

        // Creates the database and the QuantityMeasurementHistory table if they do not exist
        private void EnsureTableCreated()
        {
            try
            {
                // Parse the connection string to extract the database name
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(_connectionString);
                string dbName = builder.InitialCatalog;

                // Switch to master database to create the target database if needed
                builder.InitialCatalog = "master";

                // Open connection to master database
                SqlConnection masterConnection = new SqlConnection(builder.ConnectionString);
                masterConnection.Open();

                // Build the SQL command to create the database if it does not exist
                string createDatabaseQuery = "IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = @DbName) "
                    + "CREATE DATABASE [" + dbName + "]";
                SqlCommand createDbCommand = new SqlCommand(createDatabaseQuery, masterConnection);
                createDbCommand.Parameters.AddWithValue("@DbName", dbName);
                createDbCommand.ExecuteNonQuery();

                // Close the master database connection
                masterConnection.Close();
                masterConnection.Dispose();

                // Open connection to the target database to create the table
                SqlConnection targetConnection = new SqlConnection(_connectionString);
                targetConnection.Open();

                // SQL statement to create the QuantityMeasurementHistory table if it does not exist
                string createTableQuery = "IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='QuantityMeasurementHistory' AND xtype='U') "
                    + "CREATE TABLE QuantityMeasurementHistory ("
                    + "Id INT IDENTITY(1,1) PRIMARY KEY, "
                    + "MeasurementCategory NVARCHAR(50), "
                    + "OperationType NVARCHAR(50), "
                    + "MeasurementUnit1 NVARCHAR(50), "
                    + "MeasurementValue1 FLOAT, "
                    + "MeasurementUnit2 NVARCHAR(50), "
                    + "MeasurementValue2 FLOAT, "
                    + "TargetMeasurementUnit NVARCHAR(50), "
                    + "IsSuccess BIT, "
                    + "ErrorMessage NVARCHAR(255), "
                    + "IsComparison BIT, "
                    + "AreEqual BIT, "
                    + "CalculatedValue FLOAT, "
                    + "FormattedMessage NVARCHAR(255), "
                    + "CreatedAt DATETIME DEFAULT GETDATE()"
                    + ");";

                SqlCommand createTableCommand = new SqlCommand(createTableQuery, targetConnection);
                createTableCommand.ExecuteNonQuery();

                // Close the target database connection after table creation
                targetConnection.Close();
                targetConnection.Dispose();
            }
            catch (Exception ex)
            {
                // Log database initialization error to console
                Console.WriteLine("Database Initialization Error: " + ex.Message);
            }
        }

        // Inserts a measurement entity record into the QuantityMeasurementHistory table
        // Uses parameterized queries to prevent SQL injection attacks
        public void SaveMeasurement(MeasurementEntity entity)
        {
            try
            {
                // Open a new connection to the database
                SqlConnection connection = new SqlConnection(_connectionString);
                connection.Open();

                // Parameterized INSERT query for all measurement fields
                string insertQuery = "INSERT INTO QuantityMeasurementHistory "
                    + "(MeasurementCategory, OperationType, MeasurementUnit1, MeasurementValue1, "
                    + "MeasurementUnit2, MeasurementValue2, TargetMeasurementUnit, IsSuccess, "
                    + "ErrorMessage, IsComparison, AreEqual, CalculatedValue, FormattedMessage, CreatedAt) "
                    + "VALUES "
                    + "(@MeasurementCategory, @OperationType, @MeasurementUnit1, @MeasurementValue1, "
                    + "@MeasurementUnit2, @MeasurementValue2, @TargetMeasurementUnit, @IsSuccess, "
                    + "@ErrorMessage, @IsComparison, @AreEqual, @CalculatedValue, @FormattedMessage, @CreatedAt)";

                SqlCommand command = new SqlCommand(insertQuery, connection);

                // Add parameters with null-safe values using ternary operator
                // String fields use DBNull.Value when null to avoid SQL errors
                if (entity.MeasurementCategory != null)
                    command.Parameters.AddWithValue("@MeasurementCategory", entity.MeasurementCategory);
                else
                    command.Parameters.AddWithValue("@MeasurementCategory", DBNull.Value);

                if (entity.OperationType != null)
                    command.Parameters.AddWithValue("@OperationType", entity.OperationType);
                else
                    command.Parameters.AddWithValue("@OperationType", DBNull.Value);

                if (entity.MeasurementUnit1 != null)
                    command.Parameters.AddWithValue("@MeasurementUnit1", entity.MeasurementUnit1);
                else
                    command.Parameters.AddWithValue("@MeasurementUnit1", DBNull.Value);

                // Numeric value parameter for first measurement value
                command.Parameters.AddWithValue("@MeasurementValue1", entity.MeasurementValue1);

                if (entity.MeasurementUnit2 != null)
                    command.Parameters.AddWithValue("@MeasurementUnit2", entity.MeasurementUnit2);
                else
                    command.Parameters.AddWithValue("@MeasurementUnit2", DBNull.Value);

                // Numeric value parameter for second measurement value
                command.Parameters.AddWithValue("@MeasurementValue2", entity.MeasurementValue2);

                if (entity.TargetMeasurementUnit != null)
                    command.Parameters.AddWithValue("@TargetMeasurementUnit", entity.TargetMeasurementUnit);
                else
                    command.Parameters.AddWithValue("@TargetMeasurementUnit", DBNull.Value);

                // Boolean and numeric parameters
                command.Parameters.AddWithValue("@IsSuccess", entity.IsSuccess);

                if (entity.ErrorMessage != null)
                    command.Parameters.AddWithValue("@ErrorMessage", entity.ErrorMessage);
                else
                    command.Parameters.AddWithValue("@ErrorMessage", DBNull.Value);

                command.Parameters.AddWithValue("@IsComparison", entity.IsComparison);
                command.Parameters.AddWithValue("@AreEqual", entity.AreEqual);
                command.Parameters.AddWithValue("@CalculatedValue", entity.CalculatedValue);

                if (entity.FormattedMessage != null)
                    command.Parameters.AddWithValue("@FormattedMessage", entity.FormattedMessage);
                else
                    command.Parameters.AddWithValue("@FormattedMessage", DBNull.Value);

                // Set CreatedAt to current time if not already specified
                if (entity.CreatedAt == default(DateTime))
                    command.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                else
                    command.Parameters.AddWithValue("@CreatedAt", entity.CreatedAt);

                // Execute the INSERT command
                command.ExecuteNonQuery();

                // Log success message to console
                Console.WriteLine("Measurement saved to database successfully.");

                // Clean up resources
                command.Dispose();
                connection.Close();
                connection.Dispose();
            }
            catch (Exception ex)
            {
                // Log database error to console
                Console.WriteLine("Database Insert Error: " + ex.Message);
            }
        }

        // Returns an empty list because database retrieval is not implemented
        // Only INSERT operations are supported in this repository
        public List<MeasurementEntity> GetAllMeasurements()
        {
            // Return empty list as only INSERT is supported for database persistence
            List<MeasurementEntity> emptyList = new List<MeasurementEntity>();
            return emptyList;
        }
    }
}