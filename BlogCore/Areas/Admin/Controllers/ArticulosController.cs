using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Models;
using BlogCore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BlogCore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ArticulosController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ArticulosController(IContenedorTrabajo contenedorTrabajo,
            IWebHostEnvironment hostingEnvironment)
        {
            _contenedorTrabajo = contenedorTrabajo;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            ArticuloVM articuloVM = new ArticuloVM()
            {
                Articulo = new BlogCore.Models.Articulo(),
                ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias()
                
            };
            return View(articuloVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ArticuloVM articuloVM)
        {
            
            
            string rutaPrincipal = _hostingEnvironment.WebRootPath;
            var archivos = HttpContext.Request.Form.Files;
            if(articuloVM.Articulo.Id == 0 && archivos.Count() > 0)
            {
                // Nueva creacion
                string nombreArchivo = Guid.NewGuid().ToString();
                var subidas = Path.Combine(rutaPrincipal, @"imagenes\articulos");
                var extension = Path.GetExtension(archivos[0].FileName);

                using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                {
                    archivos[0].CopyTo(fileStreams);
                }
                articuloVM.Articulo.URLImagen = @"\imagenes\articulos\" + nombreArchivo + extension;
                articuloVM.Articulo.FechaCreacion = DateTime.Now.ToString();
                _contenedorTrabajo.Articulo.Add(articuloVM.Articulo);
                _contenedorTrabajo.Save();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("Imagen", "Debe subir una imagen para el articulo.");
            }
        
            articuloVM.ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias();
            return View(articuloVM);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            ArticuloVM articuloVM = new ArticuloVM()
            {
                Articulo = new Articulo(),
                ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias()
            };
            if (id != null)
            {
                articuloVM.Articulo = _contenedorTrabajo.Articulo.Get(id.GetValueOrDefault());
                
            }
            else
            {
                return NotFound();
            }
            return View(articuloVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ArticuloVM articuloVM)
        {
    
            string rutaPrincipal = _hostingEnvironment.WebRootPath;
            var archivos = HttpContext.Request.Form.Files;

            var articuloDesdeDb = _contenedorTrabajo.Articulo.Get(articuloVM.Articulo.Id);

            if (archivos.Count() > 0)
            {
                // Nueva imagen para Articulo Existente
                string nombreArchivo = Guid.NewGuid().ToString();
                var subidas = Path.Combine(rutaPrincipal, @"imagenes\articulos");
                var extension = Path.GetExtension(archivos[0].FileName);
                var nuevaExtension = Path.GetExtension(archivos[0].FileName);

                var rutaImagen = Path.Combine(rutaPrincipal, articuloDesdeDb.URLImagen.TrimStart('\\'));

                if (System.IO.File.Exists(rutaImagen))
                {
                    System.IO.File.Delete(rutaImagen);
                }

                //Se sube el nuevo archivo
                using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                {
                    archivos[0].CopyTo(fileStreams);
                }
                articuloVM.Articulo.URLImagen = @"\imagenes\articulos\" + nombreArchivo + extension;
                articuloVM.Articulo.FechaCreacion = DateTime.Now.ToString();
                _contenedorTrabajo.Articulo.Update(articuloVM.Articulo);
                _contenedorTrabajo.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                
                articuloVM.Articulo.URLImagen = articuloDesdeDb.URLImagen;
            }

            _contenedorTrabajo.Articulo.Update(articuloVM.Articulo);
            _contenedorTrabajo.Save();
            return RedirectToAction(nameof(Index));
           
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
