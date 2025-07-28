using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Data;
using BlogCore.Models;

namespace BlogCore.AccesoDatos.Data.Repository
{
    public class ArticuloRepository : Repository<Articulo>, IArticuloRepository
    {

        private readonly ApplicationDbContext _db;

        public ArticuloRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Articulo articulo)
        {
            var objFromDb = _db.Articulo.FirstOrDefault(s => s.Id == articulo.Id);
            if (objFromDb != null)
            {
                objFromDb.Nombre = articulo.Nombre;
                objFromDb.Descripcion = articulo.Descripcion;
                objFromDb.URLImagen = articulo.URLImagen;
                objFromDb.CategoriaId = articulo.CategoriaId;
            }
            _db.SaveChanges();
        }

        public IQueryable<Articulo> AsQueryable()
        {
            return _db.Set<Articulo>().AsQueryable();
        }
    }
}