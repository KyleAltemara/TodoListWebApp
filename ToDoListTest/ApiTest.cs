using Microsoft.AspNetCore.Http.HttpResults;
using TodoListApi.Models;
using TodoListApi.Data;
using Microsoft.EntityFrameworkCore;

namespace ToDoListTest
{
    [TestClass]
    public class ApiTest
    {
        private TodoDb CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<TodoDb>()
                .UseInMemoryDatabase(databaseName: "TodoListTest")
                .Options;

            return new TodoDb(options);
        }

        [TestMethod]
        public async Task GetAllTodos_ReturnsOkOfTodosResult()
        {
            // Arrange
            var db = CreateDbContext();

            // Act
            var result = await TodoListApi.Program.GetAllTodos(db);

            // Assert: Check for the correct returned type
            Assert.IsInstanceOfType(result, typeof(Ok<TodoItemDTO[]>));
        }

        [TestMethod]
        public async Task GetCompleteTodos_ReturnsOkOfCompleteTodosResult()
        {
            // Arrange
            var db = CreateDbContext();
            db.Todos.Add(new TodoItem { Name = "Test Todo", IsComplete = true });
            await db.SaveChangesAsync();

            // Act
            var result = await TodoListApi.Program.GetCompleteTodos(db);

            // Assert: Check for the correct returned type
            Assert.IsInstanceOfType(result, typeof(Ok<List<TodoItemDTO>>));
        }

        [TestMethod]
        public async Task GetTodo_ReturnsOkOfTodoResult()
        {
            // Arrange
            var db = CreateDbContext();
            var todo = new TodoItem { Name = "Test Todo", IsComplete = false };
            db.Todos.Add(todo);
            await db.SaveChangesAsync();

            // Act
            var result = await TodoListApi.Program.GetTodo(todo.Id, db);

            // Assert: Check for the correct returned type
            Assert.IsInstanceOfType(result, typeof(Ok<TodoItemDTO>));
        }

        [TestMethod]
        public async Task GetTodo_ReturnsNotFoundResult()
        {
            // Arrange
            var db = CreateDbContext();

            // Act
            var result = await TodoListApi.Program.GetTodo(999, db); // Non-existent ID

            // Assert: Check for the correct returned type
            Assert.IsInstanceOfType(result, typeof(NotFound));
        }

        [TestMethod]
        public async Task CreateTodo_ReturnsCreatedResult()
        {
            // Arrange
            var db = CreateDbContext();
            var todoItemDTO = new TodoItemDTO { Name = "New Todo", IsComplete = false };

            // Act
            var result = await TodoListApi.Program.CreateTodo(todoItemDTO, db);

            // Assert: Check for the correct returned type
            Assert.IsInstanceOfType(result, typeof(Created<TodoItemDTO>));
        }

        [TestMethod]
        public async Task CreateTodo_ReturnsBadRequestForInvalidModel()
        {
            // Arrange
            var db = CreateDbContext();
            var todoItemDTO = new TodoItemDTO { Name = null, IsComplete = false }; // Invalid model (Name is null)

            // Act
            var result = await TodoListApi.Program.CreateTodo(todoItemDTO, db);

            // Assert: Check for the correct returned type
            Assert.IsInstanceOfType(result, typeof(BadRequest<string>));
        }

        [TestMethod]
        public async Task UpdateTodo_ReturnsNoContentResult()
        {
            // Arrange
            var db = CreateDbContext();
            var todo = new TodoItem { Name = "Test Todo", IsComplete = false };
            db.Todos.Add(todo);
            await db.SaveChangesAsync();
            var todoItemDTO = new TodoItemDTO { Id = todo.Id, Name = "Updated Todo", IsComplete = true };

            // Act
            var result = await TodoListApi.Program.UpdateTodo(todo.Id, todoItemDTO, db);

            // Assert: Check for the correct returned type
            Assert.IsInstanceOfType(result, typeof(NoContent));
        }

        [TestMethod]
        public async Task UpdateTodo_ReturnsNotFoundResult()
        {
            // Arrange
            var db = CreateDbContext();
            var todoItemDTO = new TodoItemDTO { Id = 999, Name = "Non-existent Todo", IsComplete = true }; // Non-existent ID

            // Act
            var result = await TodoListApi.Program.UpdateTodo(999, todoItemDTO, db); // Non-existent ID

            // Assert: Check for the correct returned type
            Assert.IsInstanceOfType(result, typeof(NotFound));
        }

        [TestMethod]
        public async Task UpdateTodo_ReturnsBadRequestForInvalidModel()
        {
            // Arrange
            var db = CreateDbContext();
            var todo = new TodoItem { Name = "Test Todo", IsComplete = false };
            db.Todos.Add(todo);
            await db.SaveChangesAsync();
            var todoItemDTO = new TodoItemDTO { Id = todo.Id, Name = null, IsComplete = true }; // Invalid model (Name is null)

            // Act
            var result = await TodoListApi.Program.UpdateTodo(todo.Id, todoItemDTO, db);

            // Assert: Check for the correct returned type
            Assert.IsInstanceOfType(result, typeof(BadRequest<string>));
        }

        [TestMethod]
        public async Task DeleteTodo_ReturnsNoContentResult()
        {
            // Arrange
            var db = CreateDbContext();
            var todo = new TodoItem { Name = "Test Todo", IsComplete = false };
            db.Todos.Add(todo);
            await db.SaveChangesAsync();

            // Act
            var result = await TodoListApi.Program.DeleteTodo(todo.Id, db);

            // Assert: Check for the correct returned type
            Assert.IsInstanceOfType(result, typeof(NoContent));
        }

        [TestMethod]
        public async Task DeleteTodo_ReturnsNotFoundResult()
        {
            // Arrange
            var db = CreateDbContext();

            // Act
            var result = await TodoListApi.Program.DeleteTodo(999, db); // Non-existent ID

            // Assert: Check for the correct returned type
            Assert.IsInstanceOfType(result, typeof(NotFound));
        }
    }
}