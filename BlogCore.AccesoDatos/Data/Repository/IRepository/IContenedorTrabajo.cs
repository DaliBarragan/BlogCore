using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.AccesoDatos.Data.Repository.IRepository
{
    public interface IContenedorTrabajo : IDisposable
    {
        //Aqui se agregan los diferentes repositorios
        ICategoriaRepository Categoria { get; }
        IArticuloRepository Articulo { get; }

        ISlidersRepository Sliders { get; }

        //Agrega otros repositorios según sea necesario
        void Save();
    }
}
