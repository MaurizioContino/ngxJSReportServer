using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ngxJSReportServer.Model
{
    public class JoinModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int JoinId { get; set; }

        public FieldModel f1 { get; set; }
        public FieldModel f2 { get; set; }
        public int JoinType { get; set; }

        public List<QueryModel> Queries { get; set; }
    }
}
