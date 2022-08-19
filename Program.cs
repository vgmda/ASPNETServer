using ASPNETServer.Data;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("CORSPolicy",
    builder =>
    {
        builder
        .AllowAnyMethod()
        .AllowAnyHeader()
        .WithOrigins("https://localhost:3000", "https://appname.azurestaticapps.net");
    });
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

app.UseHttpsRedirection();

app.UseCors("CORSPolicy");

app.MapGet("/get-all-posts", async () => await PostRepository.GetPostsAsync())
    .WithTags("Posts Endpoints");

app.MapGet("/get-post-by-id/{postId}", async (int postId) =>

{
    Post postToReturn = await PostRepository.GetPostByIdAsync(postId);

    if (postToReturn != null)
    {
        return Results.Ok(postToReturn);
    }
    else
    {
        return Results.BadRequest();
    }

}).WithTags("Posts Endpoints");

app.MapPost("/create-post", async (Post postToCreate) =>

{
    bool createSuccessful = await PostRepository.CreatePostAsync(postToCreate);

    if (createSuccessful)
    {
        return Results.Ok("Create successful");
    }
    else
    {
        return Results.BadRequest();
    }

}).WithTags("Posts Endpoints");

app.MapPut("/update-post", async (Post postToUpdate) =>

{
    bool updateSuccessful = await PostRepository.UpdatePostAsync(postToUpdate);

    if (updateSuccessful)
    {
        return Results.Ok("Update successful");
    }
    else
    {
        return Results.BadRequest();
    }

}).WithTags("Posts Endpoints");

app.MapDelete("/delete-post-by-id/{postId}", async (int postId) =>

{
    bool deleteSuccessful = await PostRepository.DeletePostAsync(postId);

    if (deleteSuccessful)
    {
        return Results.Ok("Delete successful");
    }
    else
    {
        return Results.BadRequest();
    }

}).WithTags("Posts Endpoints");


app.Run();