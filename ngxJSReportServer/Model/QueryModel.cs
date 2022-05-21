using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations.Schema;
namespace ngxJSReportServer.Model
{
    public class QueryModel
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QueryId { get; set; }

        [NotMapped]
        public Dictionary<string,TableModel> Tables { get; set; }

        [NotMapped]
        public Dictionary<string, object>? Parameters { get; set; }
        [NotMapped]
        public int Page { get; set; }
        [NotMapped]
        public int PageSize { get; set; }

        public FieldModel[] Fields { get; set; }
        public FieldModel[] SelectedFields { get; set; }
        public JoinModel[]? Joins { get; set; }
        
        public string SessionId { get; set; }
        
    }
}
