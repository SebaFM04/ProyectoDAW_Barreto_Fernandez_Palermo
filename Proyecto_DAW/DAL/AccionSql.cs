// DAL/AccionSql.cs
using System.Collections.Generic;

namespace DAL
{
    public class AccionSql
    {
        public string Query { get; set; }
        public Dictionary<string, object> Parametros { get; set; }
    }
}