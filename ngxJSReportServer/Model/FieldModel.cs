using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ngxJSReportServer.Model
{
    public class FieldModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string FieldId { get; set; }
        public string Parent { get; set; }
        public string OriginalName { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }
        public string FieldType { get; set; }
        public string Size { get; set; }
        public string GroupType { get; set; }

        public TableModel TableModel { get; set; }
        public List<JoinModel> JoinsF1 { get; set; }
        public List<JoinModel> JoinsF2 { get; set; }
        public List<QueryModel> Queries { get; set; }
        public List<QueryModel> QuerySelection { get; set; }

    }
}
