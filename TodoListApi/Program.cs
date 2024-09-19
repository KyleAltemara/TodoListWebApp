
using Microsoft.EntityFrameworkCore;
using TodoListApi.Data;
using TodoListApi.Models;

namespace TodoListApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
        //builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        //builder.Services.AddEndpointsApiExplorer();
        //builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            //app.UseSwagger();
            //app.UseSwaggerUI();
            app.UseDeveloperExceptionPage();
        }

        app.UseDefaultFiles();
        app.UseStaticFiles();

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        // Map the API endpoints
        app.MapGet("/api/todoitems", GetAllTodos);
        app.MapGet("/api/todoitems/complete", GetCompleteTodos);
        app.MapGet("/api/todoitems/{id}", GetTodo);
        app.MapPost("/api/todoitems", CreateTodo);
        app.MapPut("/api/todoitems/{id}", UpdateTodo);
        app.MapDelete("/api/todoitems/{id}", DeleteTodo);

        app.Run();
    }

    public static async Task<IResult> GetAllTodos(TodoDb db)
    {
        return TypedResults.Ok(await db.Todos.Select(x => new TodoItemDTO(x)).ToArrayAsync());
    }

    public static async Task<IResult> GetCompleteTodos(TodoDb db)
    {
        return TypedResults.Ok(await db.Todos.Where(t => t.IsComplete).Select(x => new TodoItemDTO(x)).ToListAsync());
    }

    public static async Task<IResult> GetTodo(int id, TodoDb db)
    {
        return await db.Todos.FindAsync(id)
            is TodoItem todo
                ? TypedResults.Ok(new TodoItemDTO(todo))
                : TypedResults.NotFound();
    }

    public static async Task<IResult> CreateTodo(TodoItemDTO todoItemDTO, TodoDb db)
    {
        if (string.IsNullOrEmpty(todoItemDTO.Name))
        {
            return TypedResults.BadRequest("Name is required.");
        }

        var todoItem = new TodoItem
        {
            IsComplete = todoItemDTO.IsComplete,
            Name = todoItemDTO.Name
        };

        db.Todos.Add(todoItem);
        await db.SaveChangesAsync();

        todoItemDTO = new TodoItemDTO(todoItem);

        return TypedResults.Created($"/todoitems/{todoItem.Id}", todoItemDTO);
    }

    public static async Task<IResult> UpdateTodo(int id, TodoItemDTO todoItemDTO, TodoDb db)
    {
        if (string.IsNullOrEmpty(todoItemDTO.Name))
        {
            return TypedResults.BadRequest("Name is required.");
        }

        var todo = await db.Todos.FindAsync(id);

        if (todo is null) return TypedResults.NotFound();

        todo.Name = todoItemDTO.Name;
        todo.IsComplete = todoItemDTO.IsComplete;

        await db.SaveChangesAsync();

        return TypedResults.NoContent();
    }

    public static async Task<IResult> DeleteTodo(int id, TodoDb db)
    {
        if (await db.Todos.FindAsync(id) is TodoItem todo)
        {
            db.Todos.Remove(todo);
            await db.SaveChangesAsync();
            return TypedResults.NoContent();
        }

        return TypedResults.NotFound();
    }
}
