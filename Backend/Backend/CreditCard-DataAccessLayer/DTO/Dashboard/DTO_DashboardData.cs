using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditCard_DataAccessLayer.DTO.Dashboard
{
    public class DTO_DashboardData
    {
        public List<DTO_FraudTrendItem> FraudTrend { get; set; } = new();
        public List<DTO_DistributionItem> FraudDistribution { get; set; } = new();
        public List<DTO_DistributionItem> TransactionType { get; set; } = new();
        public List<DTO_DistributionItem> RiskDistribution { get; set; } = new();
        public List<DTO_AlertItem> LatestAlerts { get; set; } = new();
    }
}
