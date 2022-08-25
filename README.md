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

The following code integrates the Swagger UI into the App by adding the Swagger generator to the services collection. 
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

## CORS (Cross-Origin Resource Sharing) Policy

Cross-origin resource sharing (CORS) is a browser mechanism which enables controlled access to resources located outside of a given domain. It extends and adds flexibility to the same-origin policy (SOP). However, it also provides potential for cross-domain attacks, if a website's CORS policy is poorly configured and implemented. CORS is not a protection against cross-origin attacks such as cross-site request forgery (CSRF).

```c#
builder.Services.AddCors(options =>
{
    options.AddPolicy("CORSPolicy",
    builder =>
    {
        builder
        .AllowAnyMethod()
        .AllowAnyHeader()
        .WithOrigins("http://localhost:3000", "https://appname.azurestaticapps.net");
    });
});

app.UseCors("CORSPolicy");
```
## Setting up Entity Framework Core for the Database

Starting off with a `Post.cs` class to declare our properties for a Post.

`Post.cs`
```c#
internal sealed class Post
{
    // Data Annotations
    [Key]
    public int PostId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(100000)]
    public string Content { get; set; } = string.Empty;

}

```
The class is the DBContext class and used to query from a database and group together changes that will then be written back to the DB. It consists of two methods, `OnConfiguring()` and `OnModelCreating()`. 

`OnConfiguring()`: Configure the context to connect to SQLite database.
`OnModelCreating()`: This method is called only once when the first instance of a derived context is created. The idea is to override this method and populate with the pre-existing data.

`AppDBContext.cs`
```c#
internal sealed class AppDBContext : DbContext
{
    public DbSet<Post> Posts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder) =>
        dbContextOptionsBuilder.UseSqlite("Data Source=./Data/AppDB.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        Post[] postsToSeed = new Post[6];

        for (int i = 1; i <= 6; i++)
        {
            postsToSeed[i - 1] = new Post
            {
                PostId = i,
                Title = $"Post {i}",
                Content = $"This is post {i} and it has some content."
            };

        }
        modelBuilder.Entity<Post>().HasData(postsToSeed);
    }

}
```

## Creating a class for DB operations

Using `PostRepository.cs` class to query the DB with appropriate CRUD operations. The class is of type static and methods are defined as asynchronous tasks. 

This includes 5 methods:
- `GetPostsAsync()`:
```c#
internal async static Task<List<Post>> GetPostsAsync()
{
    using (var db = new AppDBContext())
    {
        return await db.Posts.ToListAsync();
    }
}
```
- `GetPostByIdAsync()`:
```c#
internal async static Task<Post> GetPostByIdAsync(int postId)
{
    using (var db = new AppDBContext())
    {
        return await db.Posts.FirstOrDefaultAsync(post => post.PostId == postId);
    }
}
```
- `CreatePostAsync()`:
```c#
internal async static Task<bool> CreatePostAsync(Post postToCreate)
{
    using (var db = new AppDBContext())
    {
        try
        {
            await db.Posts.AddAsync(postToCreate);

            return await db.SaveChangesAsync() >= 1;
        }
        catch (Exception e)
        {

            return false;
        }
    }
}
```
- `UpdatePostAsync()`:
```c#
internal async static Task<bool> UpdatePostAsync(Post postToUpdate)
{
    using (var db = new AppDBContext())
    {
        try
        {
            db.Posts.Update(postToUpdate);

            return await db.SaveChangesAsync() >= 1;
        }
        catch (Exception e)
        {

            return false;
        }
    }
}
```
- `DeletePostAsync()`:
```c#
internal async static Task<bool> DeletePostAsync(int postId)
{
    using (var db = new AppDBContext())
    {
        try
        {
            Post postToDelete = await GetPostByIdAsync(postId);

            db.Remove(postToDelete);

            return await db.SaveChangesAsync() >= 1;
        }
        catch (Exception e)
        {

            return false;
        }
    }
}
```
## API Endpoints

For this phase, I created a React project within the Visual Studio Code which acts as the client side. The `index.js` lets us render the context of `App.js`. All functions are declared within the `Apps.js` such as:
* _getPosts()_ - Fetches the 'GET' response from the API Endpoint
* _deletePost()_ - Fetches and executes the 'DELETE' method using the post Id
* _renderPostsTable()_ - This function holds the front end for the table where data is populated
* _onPostCreated()_ - Part of the main functionality, to create a new post
* _onPostUpdated()_ - Part of the additional functionality, adds ability to update the post
* _onPostDeleted()_ - Part of the additional functionality, adds ability to delete the post

Additional components are within the `Components` folder:
* `PostCreateForm.js` - Linked with _onPostCreated()_
```javascript
{showingCreateNewPostForm && <PostCreateForm onPostCreated={onPostCreated} />}
```
* `PostUpdateForm.js` - Linked with _onPostUpdated()_
```javascript
{postCurrentlyBeingUpdated !== null && <PostUpdateForm post={postCurrentlyBeingUpdated} onPostUpdated={onPostUpdated} />}
```
## React JS & Bootstrap
![Screen Shot 2022-08-25 at 22 21 53 p m](https://user-images.githubusercontent.com/104713435/186771338-d85f4444-6d0a-4894-9048-d27ac0ffba7c.png)

![Screen Shot 2022-08-25 at 22 22 27 p m](https://user-images.githubusercontent.com/104713435/186771404-100d8fa8-e69a-4549-9b22-9eae0f1084c9.png)

![Screen Shot 2022-08-25 at 22 23 06 p m](https://user-images.githubusercontent.com/104713435/186771494-2314d677-be1f-4fcd-b03b-a88f9f8ddda6.png)

