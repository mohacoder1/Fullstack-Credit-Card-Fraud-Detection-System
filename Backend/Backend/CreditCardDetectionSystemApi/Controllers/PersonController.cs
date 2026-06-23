using CreditCard_BusinessLayer.Services;
using CreditCard_DataAccessLayer.DTO;
using CreditCard_DataAccessLayer.Models;
using CreditCard_DataAccessLayer.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using UtilLayer;

namespace CreditCardDetectionSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
       
        public PersonController(AuthService authService)
        {
            //_authService = authService;
        }

     
    }
}

