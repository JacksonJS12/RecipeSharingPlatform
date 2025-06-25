using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RecipeSharingPlatform.Data;
using RecipeSharingPlatform.Services.Core.Contracts;

namespace RecipeSharingPlatform.Services.Core;

public class CategoryService : ICategoryService
{
    private readonly ApplicationDbContext _context;

    public CategoryService(ApplicationDbContext context)
    {
        this._context = context;
    }
    
    public async Task<IEnumerable<SelectListItem>> GetCategoriesForSelectAsync()
    {
        try
        {
            var categories = await this._context.Categories
                .AsNoTracking()
                .Where(c => !string.IsNullOrEmpty(c.Name)) 
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .OrderBy(c => c.Text)
                .ToListAsync();

            return categories ?? new List<SelectListItem>();
        }
        catch (Exception)
        {
            return new List<SelectListItem>();
        }
    }
}