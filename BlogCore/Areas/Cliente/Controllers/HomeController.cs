using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Models;
using BlogCore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BlogCore.Areas.Cliente.Controllers
{
    [Area("Cliente")]
    public class HomeController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;

        public HomeController(IContenedorTrabajo contenedorTrabajo)
        {
            _contenedorTrabajo = contenedorTrabajo;
        }


        //Primera version de Index sin paginacion
        // [HttpGet]
        // public IActionResult Index()
        // {
        //     HomeVM homeVM = new HomeVM()
        //     {
        //         ListaSliders = _contenedorTrabajo.Sliders.GetAll(),
        //         ListaArticulos = _contenedorTrabajo.Articulo.GetAll(includeProperties: "Categoria")
        //     };
        //     //Se envia la variable ViewBag.IsHome para que el slider se muestre unicamente en esta vista
        //     ViewBag.IsHome = true; // To indicate that this is the home page
        //     return View(homeVM);
        // }

        //Segunda version de Index con paginacion
        [HttpGet]
        public IActionResult Index(int page = 1, int pageSize = 3)
        {
            var articulos = _contenedorTrabajo.Articulo.AsQueryable();

            //Paginar resultados
            var paginatedEntries = articulos.Skip((page - 1) * pageSize).Take(pageSize);


            HomeVM homeVM = new HomeVM()
            {
                ListaSliders = _contenedorTrabajo.Sliders.GetAll(),
                ListaArticulos = paginatedEntries.ToList(),
                PageIndex = page,
                TotalPages = (int)Math.Ceiling(articulos.Count() / (double)pageSize)
            };
            //Se envia la variable ViewBag.IsHome para que el slider se muestre unicamente en esta vista
            ViewBag.IsHome = true; // To indicate that this is the home page
            return View(homeVM);
        }

        //Buscador
        [HttpGet]
        public IActionResult ResultadoBusqueda(string searchString, int page = 1, int pageSize = 3)
        {
            var articulos = _contenedorTrabajo.Articulo.AsQueryable();
            //Buscador de 
            if (!string.IsNullOrEmpty(searchString))
            {
                articulos = articulos.Where(e => e.Nombre.Contains(searchString));
            }

            //Paginar resultados
            var paginatedEntries = articulos.Skip((page - 1) * pageSize).Take(pageSize);

            //Modelo de la vista
            var model = new ListaPaginada<Articulo>(paginatedEntries.ToList(), articulos.Count(), page, pageSize, searchString);
            return View(model);
        }

        [HttpGet]
        public IActionResult Detalle(int id)
        {
            var articuloDesdeDB = _contenedorTrabajo.Articulo.Get(id);
            return View(articuloDesdeDB);
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
