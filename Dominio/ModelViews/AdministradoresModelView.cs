namespace MinimalApi.Dominio.ModelViews
{
    public class AdministradoresModelView
    {
        public int Id { get; set; }
        public string Email { get; set; } = default!;        
        public string Perfil { get; set; } = default!;
    }
}
