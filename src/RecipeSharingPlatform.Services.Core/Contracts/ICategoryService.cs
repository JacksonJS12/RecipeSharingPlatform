using Microsoft.AspNetCore.Mvc.Rendering;

namespace RecipeSharingPlatform.Services.Core.Contracts;

public interface ICategoryService
{
    public Task<IEnumerable<SelectListItem>> GetCategoriesForSelectAsync();
}