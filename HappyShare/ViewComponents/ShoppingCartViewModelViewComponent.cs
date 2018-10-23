using Microsoft.AspNetCore.Mvc;
using HappyShare.Data;
using HappyShare.Models.ShoppingCartViewModels;
using HappyShare.Models;

namespace HappyShare.ViewComponents
{
    public class ShoppingCartViewModelViewComponent : ViewComponent
    {

        private readonly ApplicationDbContext _context;
        public ShoppingCartViewModelViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            return View(ReturnCurrentCartViewModel());
        }

        public ShoppingCartViewModel ReturnCurrentCartViewModel()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
          
            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(_context),
            };

            return viewModel;
        }
    }
}

