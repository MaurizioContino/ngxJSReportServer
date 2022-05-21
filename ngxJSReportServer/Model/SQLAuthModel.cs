using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ngxJSReportServer.Model
{
    public class SQLAuthModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SQLAuthModelId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
       
        public string ConnectionString { get; set; }

    }
}
