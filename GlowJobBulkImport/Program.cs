using System;
using System.IO;
using ExcelDataReader;
using GlowJobBulkImport.DBInsert;
using Microsoft.Extensions.Configuration;

namespace GlowJobBulkImport
{
    class MainClass
    {
        public static void Main(String[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                                         .AddJsonFile("appsettings.json", true, true)
                                         .Build();
            Console.WriteLine(config.GetConnectionString("DefaultConnection"));
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = File.Open(@"/Users/satz/Downloads/CategoryandContent.xlsx", FileMode.Open, FileAccess.Read))
            {
                var excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                var result = excelReader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = _ => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = true,
                    }
                });
                switch (args[0])
                {
                    case Arugments.Category:
                        Console.WriteLine("Processing {0}", Arugments.Category);
                        var category = new CategoryInsert();
                        category.BulkToMariadb(result.Tables[0].Rows);
                        break;

                    default:
                        throw new Exception("No Arguments found!");

                }

                excelReader.Close();
            }
        }

                  }
    public static  class Arugments
    {
        public const String Category ="category";
        public const String Aptidue = "apptitude";
        public const String Reciepe = "recipe";
    }


}