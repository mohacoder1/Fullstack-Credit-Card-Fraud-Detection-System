using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditCard_DataAccessLayer.Models
{
    public class md_Logs
    {


        public int LogID { get; set; }
        public int ViewerID { get; set; }
        public string RecordID { get; set; }
        public string SeverityLevel { get; set; }
        public string EventType { get; set; } = string.Empty;
        public string TableName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime logDate { get; set; }
        public md_Logs( int LogID, int ViewerID, string RecordID, string SeverityLevel, string EventType, string TableName, string Description, DateTime logDate)
        {
            this.LogID = LogID;
            this.ViewerID = ViewerID;
            this.RecordID = RecordID;
            this.SeverityLevel = SeverityLevel;
            this.EventType = EventType;
            this.TableName = TableName;
            this.Description = Description;
            this.logDate = logDate;
        }


    }
}
