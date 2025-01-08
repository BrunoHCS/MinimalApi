using Microsoft.EntityFrameworkCore;
using MinimalApi.Dominio.Entidades;
using MinimalApi.Dominio.Servicos.Interfaces;
using MinimalApi.Infraestrutura.Db;

namespace MinimalApi.Dominio.Servicos
{
    public class VeiculoService : IVeiculoService
    {
        private readonly DbContexto _dbContexto;
        public VeiculoService(DbContexto dbContexto)
        {
            _dbContexto = dbContexto;
        }

        public void Apagar(Veiculo veiculo)
        {
            _dbContexto.Veiculos.Remove(veiculo);
            _dbContexto.SaveChanges();
        }

        public void Atualizar(Veiculo veiculo)
        {
            _dbContexto.Veiculos.Update(veiculo);
            _dbContexto.SaveChanges();
        }

        public Veiculo? BuscaPorId(int id)
        {
            return _dbContexto.Veiculos.Find(id);
        }

        public void Incluir(Veiculo veiculo)
        {
            _dbContexto.Veiculos.Add(veiculo);
            _dbContexto.SaveChanges();
        }

        public List<Veiculo> Todos(int pagina = 1, int quantidade = 10)
        {
            var listaVeiculos = _dbContexto.Veiculos.Skip((pagina - 1) * quantidade).Take(quantidade).ToList();

            return listaVeiculos.Select(veiculo => new Veiculo
            {
                Id = veiculo.Id,
                Nome = veiculo.Nome,
                Marca = veiculo.Marca,
                Ano = veiculo.Ano  
            }).ToList();
        }
    }
}
