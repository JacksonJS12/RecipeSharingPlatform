using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RecipeSharingPlatform.ViewModels.Recipe;

public class EditRecipeViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Recipe title is required.")]
    [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
    public string Title { get; set; } 

    [Required(ErrorMessage = "Instructions are required.")]
    [StringLength(2000, ErrorMessage = "Instructions cannot exceed 2000 characters.")]
    public string Instructions { get; set; }

    [Url(ErrorMessage = "Please enter a valid URL.")]
    public string? ImageUrl { get; set; }

    [Required(ErrorMessage = "Category is required.")]
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "Created date is required.")]
    public DateTime CreatedOn { get; set; }

    public IEnumerable<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
}