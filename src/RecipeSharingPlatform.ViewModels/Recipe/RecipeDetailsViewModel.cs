using System.ComponentModel.DataAnnotations;

namespace RecipeSharingPlatform.ViewModels.Recipe;

public class RecipeDetailsViewModel
{
    public int Id { get; set; }
        
    public string Title { get; set; } = string.Empty;
        
    public string Instructions { get; set; } = string.Empty;
        
    public string? ImageUrl { get; set; }
        
    public string Author { get; set; } = string.Empty;
        
    public string Category { get; set; } = string.Empty;
        
    [Display(Name = "Created On")]
    public DateTime CreatedOn { get; set; }

    public bool IsAuthor { get; set; }
        
    public bool IsSaved { get; set; }
}