using BlogCore.AccesoDatos.Data.Inicializador;
using BlogCore.Data;
using BlogCore.Models;
using BlogCore.Utilidades;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.AccesoDatos.Data.Initializer
{
    public class Initializer : IInitializerDB
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public Initializer(ApplicationDbContext db, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public void Inicializar()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception)
            {

            }

            if (_db.Roles.Any(ro => ro.Name == CNT.Administrador)) return;

            //Creacion de roles

            _roleManager.CreateAsync(new IdentityRole(CNT.Administrador)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(CNT.Registrado)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(CNT.Cliente)).GetAwaiter().GetResult();

            _userManager.CreateAsync(new IdentityUser
            {
                UserName = "admin@mail.com",
                Email = "admin@mail.com",
                EmailConfirmed = true,
                //La contraseña va despues de la llave que cierra y la coma
            }, "Admin12345*").GetAwaiter().GetResult();

            IdentityUser usuario = _db.identityUsers.
            Where(us => us.Email == "admin@mail.com").FirstOrDefault();
            _userManager.AddToRoleAsync(usuario, CNT.Administrador).GetAwaiter().GetResult();

        }
    }
}