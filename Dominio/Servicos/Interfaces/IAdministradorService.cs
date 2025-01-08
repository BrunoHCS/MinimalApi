using MinimalApi.Dominio.DTO;
using MinimalApi.Dominio.Entidades;

namespace MinimalApi.Dominio.Servicos.Interfaces
{
    public interface IAdministradorService
    {
        Administrador? Login(LoginDTO loginDTO);
    }
}
