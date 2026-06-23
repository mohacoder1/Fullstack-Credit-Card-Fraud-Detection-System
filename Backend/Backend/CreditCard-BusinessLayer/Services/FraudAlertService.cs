using CreditCard_DataAccessLayer.DTO;
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
    public  class FraudAlertService
    {

       

        public static List<md_FraudAlert> GetAllFraudAlerts() =>
              FraudAlertRepository.GetAllFraudAlerts(); 
        
        public static List<md_FraudAlert> GetAlertsByCardholderID(int CardholderID) =>
              FraudAlertRepository.GetAlertsByCardholderID(CardholderID);
        public static DTO_DetailedAlert GetDetailedAlert(int ID , IEncryptionService encryptionService)
        {
            var alert = FraudAlertRepository.GetDetailedAlert(ID);
            if (alert == null)
            {
                throw new Exception($"No alert found with ID {ID}");
            }
            else
            {
               
                alert.CardNumber = encryptionService.Decrypt(alert.CardNumber);
                alert.CardNumber = "**** **** **** " + alert.CardNumber.Substring(alert.CardNumber.Length - 4); // تحويل الوقت إلى التوقيت المحلي
             
                return alert;
            }
        }
                   

        public static bool setAlertAcknowledge(int AlertID, int viewerID) =>
              FraudAlertRepository.setAlertAcknowledge(AlertID, viewerID);
        public static bool ResolveFraudAlert(DTO_ResolveAlert alert) =>
                     FraudAlertRepository.ResolveFraudAlert(alert);



    }
}
