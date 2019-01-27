using System;
namespace  GlowJobBulkImport.Entities
{
    public class Category
    {
        public int id { get; set; }
        public string name { get; set; }
        public int? parent_id { get; set; }
        public string description { get; set; }
        public string keywords { get; set; }
        public string slug { get; set; }
        public int order { get; set; }
        public Boolean is_enabled { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }


    }
}
