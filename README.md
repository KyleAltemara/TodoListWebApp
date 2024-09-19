# TodoListWebApp
This is a web application that allows users to manage a to-do list. The application uses an in-memory database to store and retrieve to-do items. Users can view, create, edit, and delete to-do entries without being redirected to a new page, providing a seamless single-page application (SPA) experience.
<https://www.thecsharpacademy.com/project/26/todo-list>

## Features

- Contains models for to-do items and a context to manage the to-do data.
- Uses Entity Framework Core to interact with the in-memory database and create the necessary schema.
- Implements a minimal API to connect the front-end and the database.
- Uses the Fetch API from the front-end to call the minimal API in the backend.
- Displays a list of to-do items with options to add, edit, and delete entries.
- Provides a user-friendly interface for managing to-do entries.
- Presents confirmation messages for delete operations and success messages for updates.

## Getting Started

To run the application, follow these steps:

1. Clone the repository to your local machine.
2. Open the solution in Visual Studio.
3. Build the solution to restore NuGet packages and compile the code.
4. Run the `TodoListApi` project to start the web application.

## Dependencies

- Microsoft.EntityFrameworkCore.InMemory: The application uses this package to manage the in-memory database context and entity relationships.
- Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore: The application uses this package for error handling related to the database.
- Swashbuckle.AspNetCore: The application uses this package to generate Swagger documentation for the API.

## Usage

1. The application will display a list of to-do items with options to add, edit, and delete entries.
2. Use the input box to add new to-do items.
3. Click on the edit button to modify existing to-do items. A message will indicate that the record hasn't been updated until the user submits the new to-do. A success message will be shown upon successful update.
4. Click on the delete button to remove to-do items from the list. A confirmation message will be presented before deletion.

## License

This project is licensed under the MIT License.

## Resources Used

- [The C# Academy](https://www.thecsharpacademy.com/)
- [Microsoft Docs: Minimal APIs](https://learn.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-8.0&tabs=visual-studio)
- [Microsoft Docs: Calling a Web API in JavaScript](https://learn.microsoft.com/en-us/aspnet/core/tutorials/web-api-javascript?view=aspnetcore-8.0)
- GitHub Copilot to generate code snippets.
