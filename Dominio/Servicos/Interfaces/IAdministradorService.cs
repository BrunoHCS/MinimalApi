using MinimalApi.Dominio.DTO;
using MinimalApi.Dominio.Entidades;

namespace MinimalApi.Dominio.Servicos.Interfaces
{
    public interface IAdministradorService
    {
        Administrador? Login(LoginDTO loginDTO);
        void Incluir(Administrador administrador);
        List<Administrador> Todos(int pagina, int quantidade);
        Administrador? BuscaPorId(int id);
        void Atualizar(Administrador adm);
        void Apagar(Administrador adm);
    }
}
