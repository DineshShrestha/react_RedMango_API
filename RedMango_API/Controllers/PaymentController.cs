﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RedMango_API.Data;
using RedMango_API.Models;
using Stripe;
using System.Net;

namespace RedMango_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private ApiResponse _response;
        private readonly   IConfiguration _configuration;
        private readonly ApplicationDbContext _db;
        public PaymentController(IConfiguration configuration, ApplicationDbContext db)
        { 
            _configuration = configuration;
            _db = db;
            _response= new();
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> MakePayment(string userId)
        { 
            ShoppingCart shoppingCart = _db.ShoppingCarts
                .Include(u=>u.CartItems)
                .ThenInclude(u=>u.MenuItem).FirstOrDefault(u=>u.UserId == userId);
            if (shoppingCart == null || shoppingCart.CartItems == null || shoppingCart.CartItems.Count()==0) {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                return BadRequest(_response);
            }

            #region Create Payment Intent
            // Set your secret key. Remember to switch to your live secret key in production.
            // See your keys here: https://dashboard.stripe.com/apikeys
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];
            double carttotal = shoppingCart.CartItems.Sum(u => u.Quantity * u.MenuItem.Price);

            PaymentIntentCreateOptions options = new()
            {
                Amount = (int)(carttotal*100),
                Currency = "nok",
                PaymentMethodTypes = new List<string> { "card" },
                StatementDescriptor = "Custom descriptor",
            };
            PaymentIntentService service = new();
            PaymentIntent response = service.Create(options);
            shoppingCart.StripePaymentIntentId = response.Id;
            shoppingCart.ClientSecret = response.ClientSecret;
            #endregion
            _response.Result=shoppingCart;
            _response.StatusCode=HttpStatusCode.OK;
            return Ok(_response);
        }
    }
}
