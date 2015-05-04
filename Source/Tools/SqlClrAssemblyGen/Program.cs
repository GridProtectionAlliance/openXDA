using System;
using System.IO;
using System.Reflection;
using GSF.IO;

namespace SqlClrAssemblyGen
{
    // This program creates a script that will add the entire .NET library for use in SQL Server - this is not a recommended practice
    class Program
    {
        static void Main(string[] args)
        {
            // TODO: Make these parameters for a more generic tool
            const string DatabaseName = "MeterData";
            const string SourceFolder = @"C:\Windows\Microsoft.NET\Framework64\v4.0.30319";
            const string DestinationFile = "SqlClrAssemblies.sql";

            string[] assemblyList = FilePath.GetFileList(SourceFolder + "\\*.dll");

            using (FileStream stream = File.Create(DestinationFile))
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.WriteLine("ALTER DATABASE [{0}] SET TRUSTWORTHY ON", DatabaseName);
                writer.WriteLine("EXEC sp_configure 'clr enabled', 1");
                writer.WriteLine("RECONFIGURE");
                writer.WriteLine("GO\r\n");

                foreach (string assembly in assemblyList)
                {
                    try
                    {
                        // See if we can load file as a valid .NET assembly
                        Assembly.LoadFile(assembly);

                        writer.WriteLine("CREATE ASSEMBLY {0} AUTHORIZATION dbo FROM '{1}'", FilePath.GetFileNameWithoutExtension(assembly).Replace('.', '_'), assembly);
                        writer.WriteLine("WITH PERMISSION_SET = UNSAFE");
                        writer.WriteLine("GO\r\n");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Skipped {0}: {1}", assembly, ex.Message);
                    }
                }
            }
        }
    }
}
