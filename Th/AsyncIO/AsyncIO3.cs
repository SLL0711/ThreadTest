using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace ThreadLearning.Th.AsyncIO
{
    public class AsyncIO3
    {
        public static void Perform()
        {
            const string dataBaseName = "CustomDatabase";
            var t = ProcessAsyncIO(dataBaseName);
            t.GetAwaiter().GetResult();
            WriteLine("Process Enter to exit.");
        }

        static async Task ProcessAsyncIO(string dbName)
        {
            try
            {
                const string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;integrated Security=True;";

                string outpoutFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                string dbFileName = Path.Combine(outpoutFolder, $"{dbName}.mdf");
                string dbLogFileName = Path.Combine(outpoutFolder, $"{dbName}_log.ldf");

                string dbConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;" + $"AttachDBFileName={dbFileName};Integrated Security=True;";

                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    if (File.Exists(dbFileName))
                    {
                        WriteLine("Detaching the database.");

                        var detachCommand = new SqlCommand("sp_detach_db", connection);
                        detachCommand.CommandType = System.Data.CommandType.StoredProcedure;
                        detachCommand.Parameters.AddWithValue("@dbname", dbName);

                        await detachCommand.ExecuteNonQueryAsync();

                        WriteLine("The database was detached successfully.");
                        WriteLine("Deleting the database..."); 

                        if (File.Exists(dbLogFileName)) File.Delete(dbLogFileName);
                        File.Delete(dbFileName);

                        WriteLine("The databse was deleted successfully.");
                    }

                    WriteLine("Creating the database.");
                    string createdCommand = $"CREATE DATABASE {dbName} ON (NAME = '{dbName}',FILENAME='{dbFileName}')";
                    var cmd = new SqlCommand(createdCommand, connection);

                    await cmd.ExecuteNonQueryAsync();
                    WriteLine("The database was created successfully");
                }

                using (var connection = new SqlConnection(dbConnectionString))
                {
                    await connection.OpenAsync();

                    var cmd = new SqlCommand("SELECT NEWID()", connection);
                    var result = await cmd.ExecuteNonQueryAsync();

                    WriteLine($"NEW GUID FROM DATABSE: {result}");

                    cmd = new SqlCommand(@"CREATE TABLE [DBO].[CustomTable] ([ID] [INT] IDENTITY(1,1) NOT NULL," +
                        "[NAME] [NVARCHAR] (50) NOT NULL,CONSTRAINT [PK_ID] PRIMARY KEY CLUSTERED" +
                        "([ID] ASC) ON [PRIMARY]) ON [PRIMARY]", connection);

                    await cmd.ExecuteNonQueryAsync();

                    WriteLine("Table was created successfully");

                    cmd = new SqlCommand(
                        @"INSERT INTO [DBO].[CUSTOMTABLE] (NAME) VALUES ('JHON');" +
                        @"INSERT INTO [DBO].[CUSTOMTABLE] (NAME) VALUES ('PETER');" +
                        @"INSERT INTO [DBO].[CUSTOMTABLE] (NAME) VALUES ('JAMES');" +
                        @"INSERT INTO [DBO].[CUSTOMTABLE] (NAME) VALUES ('EUGENE');", connection);
                    await cmd.ExecuteNonQueryAsync();

                    WriteLine("inserted data successfully");
                    WriteLine("Reading data from table...");

                    cmd = new SqlCommand(@"SELECT * FROM [DBO].[CUSTOMTABLE]", connection);
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var id = reader.GetFieldValue<int>(0);
                            var name = reader.GetFieldValue<string>(1);

                            WriteLine($"table row:Id:{id} name:{name}");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                WriteLine($"Exception:{e.Message}");
            }
        }
    }
}
