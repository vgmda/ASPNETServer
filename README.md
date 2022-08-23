## ASPNETServer
> This repository consists of an small CRUD application that demonstrates the use of front-end JavaScript library React and the ASP.NET Core 6 Minimal Web APIs. I used this app to learn and explore how ASP.NET server and client communicate with each other. This app also includes setting up a database on the server with the relevant data for the APIs. 

## Topics Covered:

*   [x] Minimal APIs
*   [x] Swagger UI
*   [x] CORS Policy
*   [x] Setting up Entity Framework Core for the Database
*   [x] Creating a class for DB operations
*   [x] API Endpoints
*   [x] React JS & Bootstrap



## Minimal APIs

```c#
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/get-all-posts", async () => await PostRepository.GetPostsAsync())
    .WithTags("Posts Endpoints");
   
app.Run();
```
Using `CreateBuilder()` to initializes a new instance of the `WebApplicationBuilder` class with preconfigured defaults. The code above adds an endpoint to `/get-all-posts`. Async is used here to retrive the data from the DB. Code below represents the `GetPostsAsync()`
```c#
internal async static Task<List<Post>> GetPostsAsync()
{
    using (var db = new AppDBContext())
    {
        return await db.Posts.ToListAsync();
    }
}
```

## Swagger UI

Swagger UI allows us to visualize and interact with the API's resources without having implementation logic in place. It allows both computers and humans to understand the capabilities of a REST API without direct access to the source code. Swagger UI offers a web-based UI that provides information about the service, using the generated OpenAPI specification. 


<img width="1405" alt="Screen Shot 2022-08-23 at 22 50 38 p m" src="https://user-images.githubusercontent.com/104713435/186272906-d2bec2bd-fbf6-483f-8679-c09235c0bb83.png">

```c#
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(swaggerGenOptions =>
{
    swaggerGenOptions.SwaggerDoc("v1", new OpenApiInfo { Title = "ASP.NET React", Version = "v1" });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(swaggerUIOptions =>
{
    swaggerUIOptions.DocumentTitle = "ASP.NET React";
    swaggerUIOptions.SwaggerEndpoint("/swagger/v1/swagger.json", "Web API with POST model.");
    swaggerUIOptions.RoutePrefix = String.Empty;

});
```
