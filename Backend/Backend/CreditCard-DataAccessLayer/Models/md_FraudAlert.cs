using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace CreditCard_DataAccessLayer.Models
{
    public  class md_FraudAlert
    {
        public int AlertID { get; set; }
        public string AlertLevel { get; set; }
        public string AlertStatus { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Currency { get; set; }
        public int TX_Number { get; set; }
        public DateTime TX_Time { get; set; }
        public double Amount { get; set; }
        public double RiskScore { get; set; }




        public md_FraudAlert(
            int AlertID, int TX_Number,
            string Title, string Currency,
            string SubTitle, string AlertLevel,
            string AlertStatus, DateTime TX_Time,
            double Amount, double RiskScore
            )
        {
            this.AlertID = AlertID;
            this.TX_Number = TX_Number;
            this.AlertLevel = AlertLevel;
            this.AlertStatus = AlertStatus;
            this.TX_Time = TX_Time;

            this.Amount = Amount;
            this.Title = Title;
            this.SubTitle = SubTitle;
            this.Currency = Currency;
            this.RiskScore = RiskScore;
        }
    }
}
