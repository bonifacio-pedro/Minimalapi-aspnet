using Livraria.Context;
using Microsoft.EntityFrameworkCore;
using Livraria.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

string? con = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<LivrariaApiContext>(o => o.UseMySql(con,ServerVersion.AutoDetect(con)));

var app = builder.Build();

app.MapGet("/", () => "Hallo!");

app.MapGet("/livros", async (LivrariaApiContext db) => await db.Livros
                                                            .Take(10)
                                                            .ToListAsync());

app.MapGet("/livros/{genero}", async(string genero,LivrariaApiContext db) => 
    await db.Livros.Where(l => l.genero == genero).ToListAsync()
);

app.MapGet("/livros/{id:int}",async (int id,LivrariaApiContext db) =>
    await db.Livros.FindAsync(id) is Livro livro
    ? Results.Ok(livro)
    : Results.NotFound("Nenhum livro encontrado com esse id")
 );

app.MapPost("/livros", async (Livro livro,LivrariaApiContext db) => {
    db.Livros.Add(livro);
    await db.SaveChangesAsync();
    return Results.Ok(livro);
});

app.MapPut("/livros/{id:int}", async (int id,Livro livro, LivrariaApiContext db) => {
    if (livro is null) return Results.BadRequest("Envie um corpo de requisição válido");
    if(await db.Livros.FindAsync(id) is Livro livroFind){
        livroFind.titulo = livro.titulo;
        livroFind.genero = livro.genero;
        livroFind.autor = livro.autor;
        await db.SaveChangesAsync();
        return Results.NoContent();
    }
    else return Results.NotFound("Nenhum livro encontrado com esse id");
});

app.MapDelete("/livros/{id:int}", async (int id, LivrariaApiContext db) =>{ 
    if(await db.Livros.FindAsync(id) is Livro livro){
        db.Livros.Remove(livro);
        await db.SaveChangesAsync();
        return Results.Ok(livro);
    }
    else return Results.NotFound("Nenhum livro encontrado com esse id");
});

app.Run();
