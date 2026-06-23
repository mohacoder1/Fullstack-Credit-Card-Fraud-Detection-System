using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditCard_DataAccessLayer.DTO.Transactions
{
	public class DTO_LatestTransaction
	{
		public int TransactionID { get; set; }
		public int cardID { get; set; }
        public string Merchant { get; set; }
		public string Amount { get; set; }
		public DateTime Time { get; set; } = DateTime.Now;
		public string Status { get; set; }
		public string Message { get; set; }
	}
}
