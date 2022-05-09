namespace ngxJSReportServer.Model
{
    public class TableModel
    {
        public string OriginalName { get; set; }
        public string Name { get; set; }
        public List<FieldModel> Fields { get; set; }
        public string? TableType { get; set; }

    }
}
