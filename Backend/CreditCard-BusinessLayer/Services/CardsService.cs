using CreditCard_DataAccessLayer.DTO;
using CreditCard_DataAccessLayer.Models;
using CreditCard_DataAccessLayer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditCard_BusinessLayer.Services
{
    public class CardsService
    {
        private IEncryptionService encryptionService;
        public CardsService(IEncryptionService encryptionService)
        {
            this.encryptionService = encryptionService;
        }

        public  async Task<bool> AddNew(DTO_AddNewCard newCard)
        {
            var encryptedCardInfo= new DTO_AddNewCard
            {
                CardholderID = newCard.CardholderID,
                CardNumber = encryptionService.Encrypt(newCard.CardNumber),
                CVV = encryptionService.Encrypt(newCard.CVV),
                CardType = newCard.CardType,
                Balance = newCard.Balance
            };
            return await CardsRepository.AddNewCard(encryptedCardInfo) > 0;
        }
       
        public async Task<md_Cards> GetCardByID(int CardID)
        {
            var card = await CardsRepository.GetCardInfoByID(CardID);
            card.DisplayNumber= DecryptCardInfo(card.DisplayNumber);
            return card;
        }
        public  async Task<List<md_Cards>> GetCardsInfo()
        {
            var cards = await CardsRepository.GetCardsInfo();
            cards.ForEach(c =>
            {
                c.DisplayNumber = DecryptCardInfo(c.DisplayNumber);
            });
            return cards;
        }
        public async Task<List<md_Cards>> GetMyCardsInfo(int CardholderID)
        {
            var cards = await CardsRepository.GetCardsInfo();
            cards.ForEach(c =>
            {
                c.DisplayNumber = DecryptCardInfo(c.DisplayNumber);
            });
            var myCard= cards.Where(c=>c.CardholderID==CardholderID).ToList();
            return myCard;
        }
      
        public  async Task<bool> UpdateCardBalance(int CardID, double Amount)
        {
            return await CardsRepository.UpdateCardBalanceAfterTransaction(CardID, Amount);
        }
        public  async Task<bool> UpdateCardStatus(DTO_UpdateCardStatus DTO)
        {
            return await CardsRepository.UpdateCardStatus(DTO);
        }
        public  async Task<bool>CheckBalance (int CardID, double WithdrawAmount)
        {
            return await CardsRepository.CheckCardBalance(CardID, WithdrawAmount);
        }
   
        private string DecryptCardInfo(string CardNumber)
        {
            if (!string.IsNullOrEmpty(CardNumber))
            {
                CardNumber = encryptionService.Decrypt(CardNumber);
                CardNumber = "**** **** **** " + CardNumber.Substring(CardNumber.Length - 4);
            }
            return CardNumber;
        }

    }
}
