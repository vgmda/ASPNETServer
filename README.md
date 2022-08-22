## ASPNETServer
> This repository consists of an small CRUD application that demonstrates the use of front-end JavaScript library React and the ASP.NET Core 6 Minimal Web APIs. I used this app to learn and explore how ASP.NET server and client communicate with each other. This app also includes setting up a database on the server with the relevant data for the APIs. 

## Topics Covered:

*   [x] Minimal APIs
*   [x] Setting up Entity Framework Core for the Database
*   [x] Creating a class for DB operations
*   [x] API Endpoints
*   [x] React JS & Bootstrap
*   [x] CORS Policy
*   [x] Swagger UI

## Minimal APIs

```c#
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/get-all-posts", async () => await PostRepository.GetPostsAsync())
    .WithTags("Posts Endpoints");
   
app.Run();
```
Using CreateBuilder() to initializes a new instance of the WebApplicationBuilder class with preconfigured defaults. The code above adds an endpoint to `/get-all-posts`. Async is used here to retrive the data from the DB. Code below represents the `GetPostsAsync()`
```c#
internal async static Task<List<Post>> GetPostsAsync()
{
    using (var db = new AppDBContext())
    {
        return await db.Posts.ToListAsync();
    }
}
```
