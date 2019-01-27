using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

namespace GlowJobBulkImport.DBInsert
{
    public class CategoryInsert
    {
     

        public void BulkToMariadb(DataRowCollection dataRow)
        {
            var category = new List<string>();
            var categoryJobType = new List<string>();
            StringBuilder sCommand = new StringBuilder("INSERT INTO categories (`id`, `name`, `parent_id`, `description`,`keywords`,`slug`,`order`,`is_enabled`,`created_at`,`updated_at`) VALUES ");
            StringBuilder sCommandJobType = new StringBuilder("INSERT INTO categories_job_types (`category_id`, `job_type_id`, `created_at`,`updated_at`) VALUES ");
            using (MySqlConnection mConnection = new MySqlConnection("Server=localhost;Database=homestead;Uid=homestead;Password=homestead;Port=3306;"))
            {

              
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                DeleteRecordsResetId(mConnection);
                foreach (DataRow dr in dataRow)
                {
                    var id = (String.IsNullOrEmpty(Convert.ToString(dr[2]))) ? "NULL" : dr[2];

                    if (!id.Equals("NULL"))
                    {

                        string[] jobType = Regex.Split(dr[6].ToString(), @",");
                        foreach (var item in jobType)
                        {
                            categoryJobType.Add(string.Format("({0},{1},'{2}','{3}')",
                                    Convert.ToUInt32(dr[2]),
                                    item,
                                  (DateTime.Now).ToString("yyyy-MM-dd H:mm:ss"),
                                 (DateTime.Now).ToString("yyyy-MM-dd H:mm:ss")
                              ));
                        }
                    }
                    category.Add(string.Format("({0},'{1}',{2},'{3}','{4}','{5}',{6},{7},'{8}','{9}')",
                                Convert.ToInt32(dr[0]),
                                MySqlHelper.EscapeString(dr[1].ToString()),
                                id,
                                 MySqlHelper.EscapeString(dr[3].ToString()),
                                  MySqlHelper.EscapeString(dr[4].ToString()),
                                   MySqlHelper.EscapeString(dr[5].ToString().GenerateSlug()+"-"+dr[0].ToString()),
                                    Convert.ToInt32(dr[0]),
                                    1,
                                    (DateTime.Now).ToString("yyyy-MM-dd H:mm:ss"),
                                   (DateTime.Now).ToString("yyyy-MM-dd H:mm:ss")
                                ));
                                
                }
                sCommand.Append(string.Join(",", category));
                sCommand.Append(";");
                sCommandJobType.Append(string.Join(",", categoryJobType));
                sCommandJobType.Append(";");
                mConnection.Open();
                using (MySqlCommand myCmd = new MySqlCommand(sCommand.ToString(), mConnection))
                {
                    myCmd.CommandType = System.Data.CommandType.Text;
                    myCmd.ExecuteNonQuery();
                    stopwatch.Stop();
                    Console.WriteLine("Category inserts took " + stopwatch.ElapsedMilliseconds + "ms");
                }
                using (MySqlCommand myCmd = new MySqlCommand(sCommandJobType.ToString(), mConnection))
                {
                    myCmd.CommandType = System.Data.CommandType.Text;
                    myCmd.ExecuteNonQuery();
                    stopwatch.Stop();
                    Console.WriteLine("Job Type inserts took " + stopwatch.ElapsedMilliseconds + "ms");
                }
                mConnection.Close();
            }
        }


        public void DeleteRecordsResetId(MySqlConnection mConnection)
        {
            mConnection.Open();
            string cat = "DELETE FROM categories;";
            string job = "DELETE FROM categories_job_types";
            MySqlCommand cmdCat = new MySqlCommand(cat, mConnection);
            MySqlCommand cmdJob = new MySqlCommand(job, mConnection);
            cmdJob.ExecuteNonQuery();
            cmdCat.ExecuteNonQuery();

           
            string idReset = "ALTER TABLE categories_job_types AUTO_INCREMENT = 1";
            MySqlCommand cmdIdReset = new MySqlCommand(idReset, mConnection);
         
            cmdIdReset.ExecuteNonQuery();
           

            mConnection.Close();
        }
    }
}
