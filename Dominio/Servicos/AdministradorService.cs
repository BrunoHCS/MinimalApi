using MinimalApi.Dominio.DTO;
using MinimalApi.Dominio.Entidades;
using MinimalApi.Dominio.Servicos.Interfaces;
using MinimalApi.Infraestrutura.Db;

namespace MinimalApi.Dominio.Servicos
{
    public class AdministradorService : IAdministradorService
    {
        private readonly DbContexto _dbContexto;
        public AdministradorService(DbContexto dbContexto)
        {
             _dbContexto = dbContexto;
        }

        public Administrador? Login(LoginDTO loginDTO)
        {
            return _dbContexto.Administradores.Where(a => a.Email == loginDTO.Email && a.Senha == loginDTO.Senha).FirstOrDefault();           
        }
    }
}
