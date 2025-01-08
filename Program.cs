using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinimalApi.Dominio.DTO;
using MinimalApi.Dominio.Entidades;
using MinimalApi.Dominio.ModelViews;
using MinimalApi.Dominio.Servicos;
using MinimalApi.Dominio.Servicos.Interfaces;
using MinimalApi.Infraestrutura.Db;

var builder = WebApplication.CreateBuilder(args);

#region Injecao de Dependencia
builder.Services.AddScoped<IAdministradorService, AdministradorService>();
builder.Services.AddScoped<IVeiculoService, VeiculoService>();
#endregion

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Banco de Dados
builder.Services.AddDbContext<DbContexto>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
#endregion

var app = builder.Build();

#region Home
app.MapGet("/", () => Results.Json(new Home())).WithTags("Home");
#endregion

#region Administradores
ErrosDeValidacaoAdministrador ValidaAdmDTO(AdministradorDTO administradorDTO)
{
    var validacao = new ErrosDeValidacaoAdministrador
    {
        Mensagens = new List<string>()
    };

    if (string.IsNullOrEmpty(administradorDTO.Email))
        validacao.Mensagens.Add("O email não pode ser vazio");

    if (string.IsNullOrEmpty(administradorDTO.Senha))
        validacao.Mensagens.Add("A senha não pode ficar em branco");

    if (string.IsNullOrEmpty(administradorDTO.Perfil))
        validacao.Mensagens.Add("O email não pode ser vazio");

    return validacao;
}

app.MapPost("/administradores/login", ([FromBody] LoginDTO loginDTO, IAdministradorService administradorService) =>
{
    if (administradorService.Login(loginDTO) != null)
        return Results.Ok("Login com sucesso");
    else
        return Results.Unauthorized();

}).WithTags("Administrador");

app.MapPost("/administradores", ([FromBody] AdministradorDTO administradorDTO, IAdministradorService administradorService) =>
{
    var validacao = ValidaAdmDTO(administradorDTO);

    if (validacao.Mensagens.Count > 0)
        return Results.BadRequest(validacao);

    var newAdm = new Administrador
    {
        Email = administradorDTO.Email,
        Senha = administradorDTO.Senha,
        Perfil = administradorDTO.Perfil,
    };

    administradorService.Incluir(newAdm);

    return Results.Ok(newAdm);

}).WithTags("Administrador");

app.MapGet("/administradores", ([FromQuery] int pagina, int quantidade, IAdministradorService administradorService) =>
{
    var admsViews = new List<AdministradoresModelView>();
    var listaAdministradores = administradorService.Todos(pagina, quantidade);

    foreach (var adm in listaAdministradores)
    {
        admsViews.Add(new AdministradoresModelView
        {
            Id = adm.Id, 
            Email = adm.Email,
            Perfil = adm.Perfil
        });
    }

    return Results.Ok(admsViews);

}).WithTags("Administrador");

app.MapGet("/administradores/{id}", ([FromRoute] int id, IAdministradorService administradorService) =>
{
    var adm = administradorService.BuscaPorId(id);

    if (adm == null)
        return Results.NotFound();

    return Results.Ok(adm);

}).WithTags("Administrador");

app.MapPut("/administradores/{id}", ([FromRoute] int id, AdministradorDTO administradorDTO, IAdministradorService administradorService) =>
{
    var adm = administradorService.BuscaPorId(id);

    if (adm == null)
        return Results.NotFound();

    var validacao = ValidaAdmDTO(administradorDTO);

    if (validacao.Mensagens.Count > 0)
        return Results.BadRequest(validacao);

    adm.Email = administradorDTO.Email;
    adm.Senha = administradorDTO.Senha;
    adm.Perfil = administradorDTO.Perfil;

    administradorService.Atualizar(adm);

    return Results.Ok(adm);

}).WithTags("Administrador");

app.MapDelete("/administradores/{id}", ([FromRoute] int id, IAdministradorService administradorService) =>
{
    var adm = administradorService.BuscaPorId(id);

    if (adm == null)
        return Results.NotFound();

    administradorService.Apagar(adm);

    return Results.NoContent();

}).WithTags("Administrador");
#endregion

#region Veiculos
ErrosDeValidacaoVeiculo ValidaVeiculoDTO(VeiculoDTO veiculoDTO)
{
    var validacao = new ErrosDeValidacaoVeiculo
    {
        Mensagens = new List<string>()
    };

    if (string.IsNullOrEmpty(veiculoDTO.Nome))
        validacao.Mensagens.Add("O nome não pode ser vazio");

    if (string.IsNullOrEmpty(veiculoDTO.Marca))
        validacao.Mensagens.Add("A marca não pode ficar em branco");

    if (veiculoDTO.Ano < 1950)
        validacao.Mensagens.Add("Veiculo muito antigo, aceito somente anos superiores a 1950");

    return validacao;
}

app.MapPost("/veiculos", ([FromBody] VeiculoDTO veiculoDTO, IVeiculoService veiculoService) => 
{
    var validacao = ValidaVeiculoDTO(veiculoDTO); 

    if (validacao.Mensagens.Count > 0)
        return Results.BadRequest(validacao);

    var newVeiculo = new Veiculo
    {
        Nome = veiculoDTO.Nome,
        Marca = veiculoDTO.Marca,
        Ano = veiculoDTO.Ano
    };

    veiculoService.Incluir(newVeiculo);

    return Results.Created($"/veiculo/{newVeiculo.Id}", newVeiculo);

}).WithTags("Veiculo");

app.MapGet("/veiculos", ([FromQuery] int pagina, int quantidade, IVeiculoService veiculoService) => 
{
    var listaVeiculos = veiculoService.Todos(pagina, quantidade);

    return Results.Ok(listaVeiculos);

}).WithTags("Veiculo");

app.MapGet("/veiculos/{id}", ([FromRoute] int id, IVeiculoService veiculoService) =>
{
    var veiculo = veiculoService.BuscaPorId(id);

    if (veiculo == null) 
        return Results.NotFound();

    return Results.Ok(veiculo);

}).WithTags("Veiculo");

app.MapPut("/veiculos/{id}", ([FromRoute] int id,  VeiculoDTO veiculoDTO, IVeiculoService veiculoService) =>
{
    var veiculo = veiculoService.BuscaPorId(id);

    if (veiculo == null)
        return Results.NotFound();

    var validacao = ValidaVeiculoDTO(veiculoDTO);

    if (validacao.Mensagens.Count > 0)
        return Results.BadRequest(validacao);

    veiculo.Nome = veiculoDTO.Nome;
    veiculo.Marca = veiculoDTO.Marca;
    veiculo.Ano = veiculoDTO.Ano;    

    veiculoService.Atualizar(veiculo);

    return Results.Ok(veiculo);

}).WithTags("Veiculo");

app.MapDelete("/veiculos/{id}", ([FromRoute] int id, IVeiculoService veiculoService) =>
{
    var veiculo = veiculoService.BuscaPorId(id);

    if (veiculo == null)
        return Results.NotFound();

    veiculoService.Apagar(veiculo);

    return Results.NoContent();

}).WithTags("Veiculo");

#endregion

app.UseSwagger();
app.UseSwaggerUI();

app.Run();