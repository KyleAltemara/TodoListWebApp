using Microsoft.EntityFrameworkCore;
using TodoListApi.Models;

namespace TodoListApi.Data;

class TodoDb(DbContextOptions<TodoDb> options) : DbContext(options)
{
    public DbSet<Todo> Todos => Set<Todo>();
}
