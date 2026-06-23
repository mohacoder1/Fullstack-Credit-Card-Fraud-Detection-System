using CreditCard_DataAccessLayer.Models;
using CreditCard_DataAccessLayer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditCard_BusinessLayer.Services
{
    public class ReportServices
    {
        public static md_Report GetSummeryReport()
        {
            return ReportsRepository.GetSummeryReport()!;
        }
        public static md_Report GetSummeryReportByID(int ID)
        {
            return ReportsRepository.GetSummeryReportByID(ID)!;
        }
    }
}
