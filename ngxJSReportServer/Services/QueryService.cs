using ngxJSReportServer.Model;

namespace ngxJSReportServer.Services
{
    public class QueryService
    {

        public static string GetQuery(QueryModel q)
        {
            var select = CalculateSelect(q);
            var from = CalculateFrom(q);
            var groupby = CalculateGroup(q);
            var where = "";
            var sort = CalculateOrder(q);

            return select + " \n" + from + " \n" + groupby + " \n" + where + " \n" + sort;
        }


        private static string CalculateSelect(QueryModel q)
        {
            var tmp = "SELECT ";
            if (q.SelectedFields.Length > 0)
            {
                tmp += string.Join(",",
                    q.SelectedFields.Select(f =>
                         "\n\t" + (f.GroupType == "" || f.GroupType == "Group" ? f.Parent + "." + f.OriginalName : f.GroupType + '(' + f.Parent + "." + f.OriginalName + ')') + " as [" + f.Name + "]")
                );
            }
            else
            {
                tmp += "*";
            }
            return tmp;
        }


        private static object CalculateFrom(QueryModel q)
        {
            var tables = q.Tables.Values.ToList();
            if (q.Joins.Length > 0)
            {
                var tmp = "";
                for (var i = 0; i < q.Tables.Count; i++)
                {
                    for (var j = 0; j < q.Tables.Count; j++)
                    {
                        if (i != j)
                        {
                            if (tmp == "") tmp = '\t' + tables[i].OriginalName + " " + tables[i].Name;
                            var cjoins = q.Joins.Where(x => x.f1.Parent == tables[i].Name && x.f2.Parent == tables[j].Name).ToList();
                            if (cjoins.Count > 0)
                            {

                                tmp += " \n" + getJoinName(cjoins[0].JoinType) + " \n\t" + tables[j].OriginalName + " " + tables[j].Name +
                                  " ON ";
                                
                                tmp += string.Join(" AND",
                                    cjoins.Select(j =>
                                        j.f1.FieldId.Replace("_", ".") + " = " + j.f2.FieldId.Replace("_", ".")
                                    )
                                );

                            }
                        }
                    }
                }
                return "FROM " + tmp;
            } 
            else
            {
                if (tables.Count > 0)
                {
                    return "FROM " + tables[0].OriginalName + " as " + tables[0].Name;
                }
                else
                {
                    return "";
                }
            }
        }

        public static string CalculateGroup(QueryModel q)
        {

            var filtered = q.SelectedFields.Where(f => f.GroupType == "Group").ToList();
            if (filtered.Count > 0)
            {
                return "Group by \n" +
                    string.Join("\t,",
                        filtered.Select(f => '\t' + f.OriginalName)
                    );
            }
            return "";
        }
        public static string getJoinName(int joinType)
        {
            
                switch (joinType)
                {
                    case 0:
                        return "INNER JOIN";
                    case 1:
                        return "LEFT JOIN";
                    case 2:
                        return "RIGHT JOIN";
                    case 3:
                        return "CROSS JOIN";
                    default:
                        return "INNER JOIN";
                }
            }

        private static string CalculateOrder(QueryModel q)
        {
            var tmp = "ORDER BY ";
            if (q.SelectedFields.Length > 0)
            {
                tmp += string.Join(",",
                    q.SelectedFields.Select(f => "[" + f.Name + "]"));
            }
            else
            {
                return "";
            }
            return tmp;
        }
    }
        



}
