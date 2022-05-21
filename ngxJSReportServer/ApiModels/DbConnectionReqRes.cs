namespace ngxJSReportServer.ApiModels
{
    public class DbConnectionReqRes
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ConnectionString { get; set; }
    }
}
