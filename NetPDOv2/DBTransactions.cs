using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetPDOv2 {
    public class dbTransactions {
        public bool isExist(string sql, string[] q) {
            NetPDOv2.PDO db = new NetPDOv2.PDO();
            try {
                db._connect();
                db.prepare(sql);
                if (!q.Equals(null)) { db.bindValues(q); }
                db.execute(true);
                JArray result = db.fetchAllAsObj();
                if (!result.Equals(null) && result.Count > 0) {
                    return true;
                }
                return false;
            } catch {
                return false;
            }
        }
        public queryResult selectFromQuery(string sql, string[] q, bool isObj = false) {
            NetPDOv2.PDO db = new NetPDOv2.PDO();
            queryResult res = new queryResult();
            try {
                db._connect();
                db.prepare(sql);
                if (!q.Equals(null)) { db.bindValues(q); }
                db.execute(true);
                if (isObj) {
                    res.resultAsObject = db.fetchAllAsObj();
                    res.resultAsString = null;
                } else {
                    res.resultAsString = db.fetchAllAsStr();
                    res.resultAsObject = null;
                }
            } catch {
                if (isObj) {
                    res.resultAsObject = null;
                    res.resultAsString = null;
                } else {
                    res.resultAsString = null;
                    res.resultAsObject = null;
                }
            }
            return res;
        }
        public class queryResult {
            public JArray resultAsObject { get; set; }
            public string resultAsString { get; set; }
        }
    }
}
