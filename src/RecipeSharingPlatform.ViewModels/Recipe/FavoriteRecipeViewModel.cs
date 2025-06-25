using System.ComponentModel.DataAnnotations;

namespace RecipeSharingPlatform.ViewModels.Recipe;

public class FavoriteRecipeViewModel
{
    public int Id { get; set; }
        
    public string Title { get; set; } = string.Empty;
        
    public string? ImageUrl { get; set; }
        
    public string Category { get; set; } = string.Empty;
        
    public string Author { get; set; } = string.Empty;
        
    [Display(Name = "Created On")]
    public DateTime CreatedOn { get; set; }
}