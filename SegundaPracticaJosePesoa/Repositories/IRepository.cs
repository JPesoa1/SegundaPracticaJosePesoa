using SegundaPracticaJosePesoa.Models;

namespace SegundaPracticaJosePesoa.Repositories
{
    public interface IRepository
    {
        void Insertar(int id,string nombre , string imagen , string descripcion);
        List<Comic> GetComics();
    }
}
