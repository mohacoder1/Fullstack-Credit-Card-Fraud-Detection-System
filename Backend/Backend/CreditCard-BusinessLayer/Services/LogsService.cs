using CreditCard_DataAccessLayer.Models;
using CreditCard_DataAccessLayer.Models.Transactions;
using CreditCard_DataAccessLayer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditCard_BusinessLayer.Services
{
    public static class LogsService
    {
        public static List<md_Logs> GetAllLogs()
        {
            return LogsRepository.GetAllLogs();
        }
        

    }
}
