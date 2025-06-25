using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static RecipeSharingPlatform.GCommon.ValidationConstants.Recipe;

namespace RecipeSharingPlatform.ViewModels.Recipe;

public class RecipesInListViewModel
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(RecipeTitleMaxLength, MinimumLength = RecipeTitleMinLength)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(RecipeInstructionsMaxLength, MinimumLength = RecipeInstructionsMinLength)]
    public string Instructions { get; set; } = string.Empty;

    public string? ImageUrl { get; set; }

    [Required]
    public string AuthorId { get; set; } = string.Empty;

    [Required]
    public string AuthorName { get; set; } = null!;

    [Required]
    [Display(Name = "Created On")]
    [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
    public DateTime CreatedOn { get; set; }

    [Required]
    [Display(Name = "Category")]
    public int CategoryId { get; set; }

    [Required]
    public string CategoryName { get; set; } = null!;

    public bool IsDeleted { get; set; } = false;
    public bool IsAuthor { get; set; }
    public bool IsSaved { get; set; }
    public int SavedCount { get; set; }
}