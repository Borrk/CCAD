using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HappyShare.Models;
using HappyShare.Data;

namespace QualitySouvenir.Controllers
{
    public class ShoppingCartController : Controller
    {
        private ApplicationDbContext _context;

        public ShoppingCartController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            // Return the view
            return View(cart);
        }

        // GET: /Store/AddToCart/5
        public ActionResult AddToCart(int id)
        {
            // Retrieve the album from the database
            var item = _context.SharedItems
                .Single(p => p.ID == id);
            // Add it to the shopping cart
            var cart = ShoppingCart.GetCart(this.HttpContext);
            cart.AddToCart(item, _context);
            // Go back to the main store page for more shopping
            return Redirect(Request.Headers["Referer"].ToString());
        }

        public ActionResult RemoveFromCart(int id)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            int itemCount = cart.RemoveFromCart(id, _context);
            return Redirect(Request.Headers["Referer"].ToString());
        }

        public ActionResult ClearCart()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            cart.EmptyCart(_context);
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}