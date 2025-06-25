using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace RecipeSharingPlatform.Data.Models;

public class UserRecipe
{
    [Key]
    [Required]
    public string UserId { get; set; } = string.Empty;

    [ForeignKey(nameof(UserId))]
    public IdentityUser User { get; set; } = null!;

    [Required]
    public int RecipeId { get; set; }

    [ForeignKey(nameof(RecipeId))]
    public Recipe Recipe { get; set; } = null!;
}