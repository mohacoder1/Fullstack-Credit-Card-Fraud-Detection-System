using CreditCard_DataAccessLayer.Database;
using CreditCard_DataAccessLayer.DTO.Dashboard;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

public class DashboardRepository
{
    public static async Task<DTO_DashboardData> GetDashboardChartsAsync()
    {
        var data = new DTO_DashboardData();

        using (SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString))
        {
            SqlCommand cmd = new SqlCommand("SP_Dashboard_GetChartsData", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            await conn.OpenAsync();

            using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
            {
                // 1. قراءة الجدول الأول: Fraud Trend
                while (await reader.ReadAsync())
                {
                    data.FraudTrend.Add(new DTO_FraudTrendItem
                    {
                        Month = reader.GetInt32("Month"),
                        FraudCount = reader.GetInt32("FraudCount"),
                        LegitCount = reader.GetInt32("LegitCount")
                    });
                }

                // 2. الانتقال للجدول الثاني: Fraud Distribution
                if (await reader.NextResultAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        data.FraudDistribution.Add(new DTO_DistributionItem
                        {
                            Label = reader.GetString("Status"),
                            Value = reader.GetInt32("Count")
                        });
                    }
                }

                // 3. الانتقال للجدول الثالث: Transactions by Type
                if (await reader.NextResultAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        data.TransactionType.Add(new DTO_DistributionItem
                        {
                            Label = reader.GetString("Type"),
                            Value = reader.GetInt32("Count")
                        });
                    }
                }

                // 4. الانتقال للجدول الرابع: Risk Distribution
                if (await reader.NextResultAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        data.RiskDistribution.Add(new DTO_DistributionItem
                        {
                            Label = reader.GetString("Severity"),
                            Value = reader.GetInt32("Count")
                        });
                    }
                }

                // 5. الانتقال للجدول الخامس: Latest Alerts
                if (await reader.NextResultAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        data.LatestAlerts.Add(new DTO_AlertItem
                        {
                            Id = reader.GetInt32("Id"),
                            Title = reader.GetString("Title"),
                            Severity = reader.GetString("Severity"),
                            Timestamp = reader.GetDateTime("Timestamp")
                        });
                    }
                }
            }
        }
        return data;
    }
}