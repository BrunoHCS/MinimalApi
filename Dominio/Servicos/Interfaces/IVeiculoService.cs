using MinimalApi.Dominio.Entidades;

namespace MinimalApi.Dominio.Servicos.Interfaces
{
    public interface IVeiculoService
    {
        List<Veiculo> Todos(int pagina = 1, int quantidade = 10);
        Veiculo? BuscaPorId(int id);
        void Incluir(Veiculo veiculo);
        void Atualizar(Veiculo veiculo);
        void Apagar(Veiculo veiculo);
    }
}
