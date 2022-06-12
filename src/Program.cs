using Microsoft.EntityFrameworkCore;
using minimal_api.Context;
using minimal_api.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

//Injetando a dependência de dbcontext e sinalizando que vou utilizar InMemoryDabase
builder.Services.AddDbContext<Contexto>(options => options.UseInMemoryDatabase("Cidades"));

//Adicionando Swagger para documentação da API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if(app.Environment.IsDevelopment())
{
    //Ativando o swagger
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/cidades", async (Contexto dbContext) => await dbContext.Cidades.ToListAsync());

app.MapGet("/cidades/{id}", async (int id, Contexto dbContext) => 
    await dbContext.Cidades.FirstOrDefaultAsync(cidade => cidade.Id == id));

app.MapPost("/cidades", async (Cidade cidade, Contexto dbContext) => 
{
    if (cidade != null)
    {
        dbContext.Cidades.Add(cidade);
        await dbContext.SaveChangesAsync();
    }

    return cidade;
});  

app.MapPut("/cidades/{id}", async (int id, Cidade cidade, Contexto dbContext) => 
{
    var cidadeAtual = await dbContext.Cidades.FirstOrDefaultAsync(c => c.Id == id);
    if (cidadeAtual != null && cidadeAtual.Id == cidade.Id)
    {
        dbContext.Entry(cidade).State = EntityState.Modified;
        await dbContext.SaveChangesAsync();   

        return cidade;             
    }    

    return null;
}); 

app.MapDelete("/cidades/{id}", async (int id, Contexto dbContext) => 
{
    var cidade = await dbContext.Cidades.FirstOrDefaultAsync(c => c.Id == id);
    if (cidade != null)
    {
        dbContext.Cidades.Remove(cidade);
        await dbContext.SaveChangesAsync();
    }

    return;
});

app.Run();
