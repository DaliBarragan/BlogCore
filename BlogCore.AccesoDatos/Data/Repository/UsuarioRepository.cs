using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Data;
using BlogCore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogCore.AccesoDatos.Data.Repository
{
    public class UsuarioRepository : Repository<IdentityUser>, IUsuarioRepository
    {
        private readonly ApplicationDbContext _db;

        public UsuarioRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void BloquearUsuario(string IdUsuario)
        {
            var usuarioDesdeDB = _db.identityUsers.FirstOrDefault(u => u.Id == IdUsuario);
            usuarioDesdeDB.LockoutEnd = DateTime.Now.AddYears(1000); // Bloquea el usuario por un largo periodo
            _db.SaveChanges();
        }
        
        public void DesbloquearUsuario(string IdUsuario)
        {
            var usuarioDesdeDB = _db.identityUsers.FirstOrDefault(u => u.Id == IdUsuario);
            usuarioDesdeDB.LockoutEnd = DateTime.Now; // Bloquea el usuario por un largo periodo
            _db.SaveChanges();
        }
    }
}