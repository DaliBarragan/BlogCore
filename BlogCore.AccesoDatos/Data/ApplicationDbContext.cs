﻿using BlogCore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlogCore.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        //Poner modelos que se vayan creando
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Articulo> Articulo { get; set; }
        public DbSet<Sliders> Sliders { get; set; }
        public DbSet<IdentityUser> identityUsers { get; set; }
    }
}
