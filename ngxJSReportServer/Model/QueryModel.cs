namespace ngxJSReportServer.Model
{
    public class QueryModel
    {
        public Dictionary<string,TableModel> Tables { get; set; }
        public FieldModel[] Fields { get; set; }
        public FieldModel[] SelectedFields { get; set; }
        public JoinModel[]? Joins { get; set; }
        public Dictionary<string, object>? Parameters { get; set; }
        public string SessionId { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
