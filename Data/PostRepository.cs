using System;
using Microsoft.EntityFrameworkCore;

namespace ASPNETServer.Data;

internal static class PostRepository
{
    internal async static Task<List<Post>> GetPostsAsync()
    {
        using (var db = new AppDBContext())
        {
            return await db.Posts.ToListAsync();
        }
    }


    internal async static Task<Post> GetPostByIdAsync(int postId)
    {
        using (var db = new AppDBContext())
        {
            return await db.Posts.FirstOrDefaultAsync(post => post.PostId == postId);
        }
    }

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
}

