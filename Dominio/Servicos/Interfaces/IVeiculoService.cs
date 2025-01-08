using MinimalApi.Dominio.Entidades;

namespace MinimalApi.Dominio.Servicos.Interfaces
{
    public interface IVeiculoService
    {
        List<Veiculo> Todos(int pagina, int quantidade);
        Veiculo? BuscaPorId(int id);
        void Incluir(Veiculo veiculo);
        void Atualizar(Veiculo veiculo);
        void Apagar(Veiculo veiculo);
    }
}
