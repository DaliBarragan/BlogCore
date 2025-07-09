using BlogCore.AccesoDatos.Repository.IRepository;
using BlogCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.AccesoDatos.Data.Repository.IRepository
{
    public interface ISlidersRepository : IRepository<Sliders>
    {
        void Update(Sliders sliders);
    }
}
