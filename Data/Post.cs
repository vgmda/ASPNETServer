using System;
using System.ComponentModel.DataAnnotations;

namespace ASPNETServer.Data;


// Internal - only available in this project/solution
// Sealed - It can't be inherited from

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

