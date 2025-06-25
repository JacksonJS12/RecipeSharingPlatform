using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using RecipeSharingPlatform.GCommon;
using static RecipeSharingPlatform.GCommon.ValidationConstants.Recipe;

namespace RecipeSharingPlatform.Data.Models;

public class Recipe
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
    [ForeignKey(nameof(AuthorId))]
    public IdentityUser Author { get; set; } = null!;

    [Required]
    [Display(Name = "Created On")]
    [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
    public DateTime CreatedOn { get; set; }

    [Required]
    [Display(Name = "Category")]
    public int CategoryId { get; set; }

    [Required]
    [ForeignKey(nameof(CategoryId))]
    public Category Category { get; set; } = null!;

    public bool IsDeleted { get; set; } = false;
    public int SavedCount { get; set; } = 0;

    public virtual ICollection<UserRecipe> UsersRecipes { get; set; } = new HashSet<UserRecipe>();

}