using RecipeSharingPlatform.Data.Models;
using RecipeSharingPlatform.ViewModels.Recipe;

namespace RecipeSharingPlatform.Services.Core.Contracts
{
    public interface IRecipeService
    {
        IEnumerable<T> GetAll<T>();
        Task<bool> CreateAsync(CreateRecipeViewModel viewModel, string userId);
        Task<Recipe?> GetByIdAsync(int id);
        Task<bool> IsRecipeSavedByUserAsync(int recipeId, string userId);
        Task<bool> SaveRecipeAsync(int recipeId, string userId);
        Task<List<FavoriteRecipeViewModel>> GetUserFavoriteRecipesAsync(string userId);
        Task<bool> RemoveFromFavoritesAsync(int recipeId, string userId);
        Task<EditRecipeViewModel?> GetRecipeForEditAsync(int id, string userId);
        Task<bool> UpdateRecipeAsync(EditRecipeViewModel viewModel, string userId);
        Task<DeleteRecipeViewModel?> GetRecipeForDeleteAsync(int id, string userId);
        Task<bool> DeleteRecipeAsync(int id, string userId);
    }
}