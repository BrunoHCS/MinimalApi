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

        public void Incluir(Administrador administrador)
        {
            _dbContexto.Administradores.Add(administrador);
            _dbContexto.SaveChanges();
        }

        public List<Administrador> Todos(int pagina, int quantidade)
        {
            var listaAdministradores = _dbContexto.Administradores.Skip((pagina - 1) * quantidade).Take(quantidade).ToList();

            return listaAdministradores.Select(adm => new Administrador
            {
                Id = adm.Id,
                Email = adm.Email,
                Senha = adm.Senha,
                Perfil = adm.Perfil,
            }).ToList();
        }

        public Administrador? BuscaPorId(int id)
        {
            return _dbContexto.Administradores.Find(id);
        }

        public void Atualizar(Administrador adm)
        {
            _dbContexto.Administradores.Update(adm);
            _dbContexto.SaveChanges();
        }

        public void Apagar(Administrador adm)
        {
            _dbContexto.Administradores.Remove(adm);
            _dbContexto.SaveChanges();
        }
    }
}
