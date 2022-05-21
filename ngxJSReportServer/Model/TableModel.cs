using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ngxJSReportServer.Model
{
    public class TableModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TableId { get; set; }
        public string OriginalName { get; set; }
        public string Name { get; set; }
        public List<FieldModel> Fields { get; set; }
        public string? TableType { get; set; }

    }
}
