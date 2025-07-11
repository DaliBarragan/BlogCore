using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Models;
using BlogCore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BlogCore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SlidersController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public SlidersController(IContenedorTrabajo contenedorTrabajo,
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
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Sliders slider)
        {
            
            
            string rutaPrincipal = _hostingEnvironment.WebRootPath;
            var archivos = HttpContext.Request.Form.Files;
            if(slider.Id == 0 && archivos.Count() > 0)
            {
                // Nueva creacion
                string nombreArchivo = Guid.NewGuid().ToString();
                var subidas = Path.Combine(rutaPrincipal, @"imagenes\sliders");
                var extension = Path.GetExtension(archivos[0].FileName);

                using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                {
                    archivos[0].CopyTo(fileStreams);
                }
                slider.URLImagen = @"\imagenes\sliders\" + nombreArchivo + extension;
                _contenedorTrabajo.Sliders.Add(slider);
                _contenedorTrabajo.Save();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("Imagen", "Debe subir una imagen para el articulo.");
            }
        
            
            return View();
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Sliders slider = new Sliders();
            slider = _contenedorTrabajo.Sliders.Get(id);
            if (slider == null)
            {
                return NotFound();
            }
            return View(slider);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Sliders slider)
        {
    
            string rutaPrincipal = _hostingEnvironment.WebRootPath;
            var archivos = HttpContext.Request.Form.Files;

            var sliderDesdeDb = _contenedorTrabajo.Sliders.Get(slider.Id);

            if (archivos.Count() > 0)
            {
                // Nueva imagen para Slider Existente
                string nombreArchivo = Guid.NewGuid().ToString();
                var subidas = Path.Combine(rutaPrincipal, @"imagenes\sliders");
                var extension = Path.GetExtension(archivos[0].FileName);
                var nuevaExtension = Path.GetExtension(archivos[0].FileName);

                var rutaImagen = Path.Combine(rutaPrincipal, sliderDesdeDb.URLImagen.TrimStart('\\'));

                if (System.IO.File.Exists(rutaImagen))
                {
                    System.IO.File.Delete(rutaImagen);
                }

                //Se sube el nuevo archivo
                using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                {
                    archivos[0].CopyTo(fileStreams);
                }
                slider.URLImagen = @"\imagenes\sliders\" + nombreArchivo + extension;
                _contenedorTrabajo.Sliders.Update(slider);
                _contenedorTrabajo.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {

                slider.URLImagen = sliderDesdeDb.URLImagen;
            }

            _contenedorTrabajo.Sliders.Update(slider);
            _contenedorTrabajo.Save();
            return RedirectToAction(nameof(Index));
           
        }

        #region API Calls
        [HttpGet]
        public IActionResult GetAll()
        {
            var sliders = _contenedorTrabajo.Sliders.GetAll();
            return Json(new { data = sliders });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var slider = _contenedorTrabajo.Sliders.Get(id);
            string rutaPrincipal = _hostingEnvironment.WebRootPath;
            var rutaImagen = Path.Combine(rutaPrincipal, slider.URLImagen.TrimStart('\\'));
            if (System.IO.File.Exists(rutaImagen))
            {
                System.IO.File.Delete(rutaImagen);
            }
            if (slider == null)
            {
                return Json(new { success = false, message = "Error al eliminar el slider." });
            }
            _contenedorTrabajo.Sliders.Remove(slider);
            _contenedorTrabajo.Save();
            return Json(new { success = true, message = "Slider eliminado correctamente." });
        }

        #endregion
    }
}
