using System;
using System.IO;
using ExcelDataReader;
using GlowJobBulkImport.DBInsert;

namespace GlowJobBulkImport
{
    class MainClass
    {
        public static void Main(String[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("No Aruguments Provided");
                Environment.Exit(1);
            }
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