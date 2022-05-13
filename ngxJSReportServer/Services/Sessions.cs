using ngxJSReportServer.Model;

namespace ngxJSReportServer.Services
{
    public class Session
    {

        public string SessionId { get; set; }
        public SQLAuthModel Auth { get; set; }
    }

    public static class SessionManager
    {

        private static Dictionary<string, Session> store = new Dictionary<string, Session>();
        
        public static string createSession(SQLAuthModel auth)
        {
            string id = Guid.NewGuid().ToString();
            store.Add(id, new Session { SessionId = id, Auth = auth });
            return id;
        }
        public static SQLAuthModel getAuth(string sessionId)
        {
            return store[sessionId].Auth;
        }


    }
    
}
