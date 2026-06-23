using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditCard_DataAccessLayer.Models
{
    public sealed class md_Report
    {

        //FUN_Report_GetFraudDetectionReport

       public int TotalTransactions { get; set; }
       public int FraudDetected { get; set; }
       public int Legitimate { get; set; }
       public double FraudRate { get; set; }
       public int HighRiskAlerts { get; set; }
       public int PendingReviews { get; set; }
       public int TodaysTransactions { get; set; }
       public int CrossBorderSuspicious { get; set; }

        public md_Report(int TotalTransactions, int FraudDetected, int Legitimate, double FraudRate, int HighRiskAlerts
           , int PendingReviews, int TodaysTransactions, int CrossBorderSuspicious)
        {
            this.TotalTransactions = TotalTransactions;
            this.CrossBorderSuspicious = CrossBorderSuspicious;
            this.PendingReviews= PendingReviews;
            this.FraudDetected = FraudDetected;
            this.FraudRate = FraudRate;
            this.HighRiskAlerts = HighRiskAlerts;
            this.Legitimate = PendingReviews;
            this.TodaysTransactions = TodaysTransactions;

        }
    }
}
