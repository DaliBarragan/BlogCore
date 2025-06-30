using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlogCore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ArticulosController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;

        public ArticulosController(IContenedorTrabajo contenedorTrabajo)
        {
            _contenedorTrabajo = contenedorTrabajo;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                _contenedorTrabajo.Categoria.Add(categoria);
                _contenedorTrabajo.Save();
                TempData["success"] = "Categoria creada correctamente.";
                return RedirectToAction("Index");
            }
            return View(categoria);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Categoria categoria = new Categoria();
            categoria = _contenedorTrabajo.Categoria.Get(id);
            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                _contenedorTrabajo.Categoria.Update(categoria);
                _contenedorTrabajo.Save();
                TempData["success"] = "Categoria editada correctamente.";
                return RedirectToAction("Index");
            }
            return View(categoria);
        }

        #region API Calls
        [HttpGet]
        public IActionResult GetAll()
        {
            var articulos = _contenedorTrabajo.Articulo.GetAll(includeProperties: "Categoria");
            return Json(new { data = articulos });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var categoria = _contenedorTrabajo.Categoria.Get(id);
            if (categoria == null)
            {
                return Json(new { success = false, message = "Error al eliminar la categoria." });
            }
            _contenedorTrabajo.Categoria.Remove(categoria);
            _contenedorTrabajo.Save();
            return Json(new { success = true, message = "Categoria eliminada correctamente." });
        }

        #endregion
    }
}
