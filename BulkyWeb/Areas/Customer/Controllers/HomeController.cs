using System.Diagnostics;
using System.Security.Claims;
using BulkyWeb.Models;
using BulkyWeb.Repository;
using BulkyWeb.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
			_unitOfWork = unitOfWork;
		}

        public IActionResult Index()
        {
            IEnumerable<Products> productList = _unitOfWork.Products.GetAll(includeProperties: "Category");
            return View(productList);
        }
		public IActionResult Details(int productId)
		{
			Products product = _unitOfWork.Products.Get(u=>u.Id == productId, includeProperties:"Category");
			return View(product);
		}



		//public IActionResult Details(int productId)
		//{
		//	ShoppingCart cart = new()
		//	{
		//		Product = _unitOfWork.Product.Get(u => u.Id == productId, includeProperties: "Category,ProductImages"),
		//		Count = 1,
		//		ProductId = productId
		//	};
		//	return View(cart);
		//}

		//[HttpPost]
		//[Authorize]
		//public IActionResult Details(ShoppingCart shoppingCart)
		//{
		//	var claimsIdentity = (ClaimsIdentity)User.Identity;
		//	var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
		//	shoppingCart.ApplicationUserId = userId;

		//	ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.ApplicationUserId == userId &&
		//	u.ProductId == shoppingCart.ProductId);

		//	if (cartFromDb != null)
		//	{
		//		//shopping cart exists
		//		cartFromDb.Count += shoppingCart.Count;
		//		_unitOfWork.ShoppingCart.Update(cartFromDb);
		//		_unitOfWork.Save();
		//	}
		//	else
		//	{
		//		//add cart record
		//		_unitOfWork.ShoppingCart.Add(shoppingCart);
		//		_unitOfWork.Save();
		//		HttpContext.Session.SetInt32(SD.SessionCart,
		//		_unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId).Count());
		//	}
		//	TempData["success"] = "Cart updated successfully";




		//	return RedirectToAction(nameof(Index));
		//}




		public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
