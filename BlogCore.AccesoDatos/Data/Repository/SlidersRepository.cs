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
    public class SlidersRepository : Repository<Sliders>, ISlidersRepository
    {
        
        private readonly ApplicationDbContext _db;

        public SlidersRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public Sliders Get(int? id)
        {
            throw new NotImplementedException();
        }

        public void Update(Sliders sliders)
        {
            var objFromDb = _db.Sliders.FirstOrDefault(s => s.Id == sliders.Id);
            if (objFromDb != null)
            {
                objFromDb.Nombre = sliders.Nombre;
                objFromDb.Estado = sliders.Estado;
                objFromDb.URLImagen = sliders.URLImagen;
            }
            _db.SaveChanges();
        }
    }
}