using AspNetMVC_Inlamningsuppgift_1.Models;
using Microsoft.AspNetCore.Mvc;

namespace AspNetMVC_Inlamningsuppgift_1.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(MeetingModel model)
        {
            return View();
        }



        public IActionResult Service()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(MeetingModel model)
        {
            return View();
        }


        public IActionResult Error()
        {
            return View();
        }
    }
}
