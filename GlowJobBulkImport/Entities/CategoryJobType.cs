using System;
namespace GlowJobBulkImport.Entities
{
    public class CategoryJobType
    {
       public int category_id { get; set; }
       public int job_type_id { get; set; }
       public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}
