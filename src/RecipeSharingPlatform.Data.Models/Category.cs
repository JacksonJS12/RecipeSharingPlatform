using System.ComponentModel.DataAnnotations;
using RecipeSharingPlatform.GCommon;
using static RecipeSharingPlatform.GCommon.ValidationConstants.Category;

namespace RecipeSharingPlatform.Data.Models;

public class Category
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(CategoryNameMaxLength, MinimumLength = CategoryNameMinLength)]
    public string Name { get; set; } = string.Empty;

    public virtual ICollection<Recipe> Recipes { get; set; } = new HashSet<Recipe>();
}