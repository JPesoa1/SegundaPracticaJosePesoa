using Microsoft.AspNetCore.Mvc;
using SegundaPracticaJosePesoa.Models;
using SegundaPracticaJosePesoa.Repositories;

using System.Diagnostics;

namespace SegundaPracticaJosePesoa.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private IRepository repo;

      



        public HomeController(ILogger<HomeController> logger, IRepository repo)
        {
            _logger = logger;
            this.repo = repo;
        }

        public IActionResult Index()
        {
            List<Comic> comics = this.repo.GetComics();

            return View(comics);
        }


        public IActionResult Create()
        {
            return View();
        
        }


        [HttpPost]
        public IActionResult Create(int idcomic,string nombre , string imagen, string descripcion)
        {

            this.repo.Insertar(idcomic,nombre,imagen,descripcion);

            return RedirectToAction("Index");

        }









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