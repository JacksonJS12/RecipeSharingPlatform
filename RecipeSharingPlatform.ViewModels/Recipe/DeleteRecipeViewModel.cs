using System.ComponentModel.DataAnnotations;

namespace RecipeSharingPlatform.ViewModels.Recipe;

public class DeleteRecipeViewModel
{
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;

    public string Author { get; set; } = string.Empty;

    public string AuthorId { get; set; } = string.Empty;

    public string? ImageUrl { get; set; }

    public string Category { get; set; } = string.Empty;

    public DateTime CreatedOn { get; set; }

    public string Instructions { get; set; } = string.Empty;
}