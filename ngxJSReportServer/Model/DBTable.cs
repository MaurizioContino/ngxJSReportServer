namespace ngxJSReportServer.Model
{
    public class DBTable
    {
        public string OriginalName { get; set; }
        public string Name { get; set; }
        public List<DBField> Fields { get; set; }
        public string TableType { get; set; }

    }
}
