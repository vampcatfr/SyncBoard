using Microsoft.EntityFrameworkCore;
using SyncBoard.Application.Boards.CreateBoard;
using SyncBoard.Application.Boards.GetBoardById;
using SyncBoard.Application.Boards.GetBoards;
using SyncBoard.Application.Common.Persistence;
using SyncBoard.Infrastructure.Persistence;
using SyncBoard.Infrastructure.Persistence.Repositories;
using SyncBoard.Application.Boards.RenameBoard;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString =
        builder.Configuration.GetConnectionString("DefaultConnection");

    options.UseNpgsql(connectionString);
});

builder.Services.AddScoped<IBoardRepository, BoardRepository>();
builder.Services.AddScoped<CreateBoardHandler>();
builder.Services.AddScoped<GetBoardByIdHandler>();
builder.Services.AddScoped<GetBoardsHandler>();
builder.Services.AddScoped<RenameBoardHandler>();

builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();