using Microsoft.EntityFrameworkCore;
using SyncBoard.Api.ErrorHandling;
using SyncBoard.Application.Boards.CreateBoard;
using SyncBoard.Application.Boards.DeleteBoard;
using SyncBoard.Application.Boards.GetBoardById;
using SyncBoard.Application.Boards.GetBoards;
using SyncBoard.Application.Boards.RenameBoard;
using SyncBoard.Application.Cards.CreateCard;
using SyncBoard.Application.Cards.DeleteCard;
using SyncBoard.Application.Cards.GetCardsByColumnId;
using SyncBoard.Application.Cards.MoveCard;
using SyncBoard.Application.Cards.RenameCard;
using SyncBoard.Application.Columns.CreateColumn;
using SyncBoard.Application.Columns.DeleteColumn;
using SyncBoard.Application.Columns.GetColumnsByBoardId;
using SyncBoard.Application.Columns.MoveColumn;
using SyncBoard.Application.Columns.RenameColumn;
using SyncBoard.Application.Common.Persistence;
using SyncBoard.Infrastructure.Persistence;
using SyncBoard.Infrastructure.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString =
        builder.Configuration.GetConnectionString("DefaultConnection");

    options.UseNpgsql(connectionString);
});

// Boards
builder.Services.AddScoped<IBoardRepository, BoardRepository>();
builder.Services.AddScoped<CreateBoardHandler>();
builder.Services.AddScoped<GetBoardByIdHandler>();
builder.Services.AddScoped<GetBoardsHandler>();
builder.Services.AddScoped<RenameBoardHandler>();
builder.Services.AddScoped<DeleteBoardHandler>();

// Columns
builder.Services.AddScoped<IColumnRepository, ColumnRepository>();
builder.Services.AddScoped<CreateColumnHandler>();
builder.Services.AddScoped<GetColumnsByBoardIdHandler>();
builder.Services.AddScoped<RenameColumnHandler>();
builder.Services.AddScoped<MoveColumnHandler>();
builder.Services.AddScoped<DeleteColumnHandler>();

// Cards
builder.Services.AddScoped<ICardRepository, CardRepository>();
builder.Services.AddScoped<CreateCardHandler>();
builder.Services.AddScoped<GetCardsByColumnIdHandler>();
builder.Services.AddScoped<RenameCardHandler>();
builder.Services.AddScoped<MoveCardHandler>();
builder.Services.AddScoped<DeleteCardHandler>();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ApiExceptionFilter>();
});

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddOpenApi();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();