using Microsoft.AspNetCore.Mvc.Rendering;
using RecipeSharingPlatform.Data;
using RecipeSharingPlatform.Data.Models;
using RecipeSharingPlatform.Services.Core.Contracts;
using RecipeSharingPlatform.ViewModels.Recipe;
using Microsoft.EntityFrameworkCore;

namespace RecipeSharingPlatform.Services.Core
{
    public class RecipeService : IRecipeService
    {
        private readonly ApplicationDbContext _context;

        public RecipeService(ApplicationDbContext context)
        {
            this._context = context;
        }

        public IEnumerable<T> GetAll<T>()
        {
            var recipes = this._context.Recipes
                .AsNoTracking()
                .Include(r => r.Category)
                .Where(x =>  !x.IsDeleted)
                .OrderByDescending(x => x.Id)
                .ToList();
            
            return (IEnumerable<T>)recipes;
        }
        public async Task<bool> CreateAsync(CreateRecipeViewModel viewModel, string userId)
        {
            try
            {
                var recipe = new Recipe
                {
                    Title = viewModel.Title,
                    Instructions = viewModel.Instructions,
                    ImageUrl = viewModel.ImageUrl,
                    CategoryId = viewModel.CategoryId,
                    AuthorId = userId,
                    CreatedOn = viewModel.CreatedOn,
                    IsDeleted = false
                };

                await this._context.Recipes.AddAsync(recipe);
                await this._context.SaveChangesAsync();
        
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Recipe?> GetByIdAsync(int id)
        {
            try
            {
                return await this._context.Recipes
                    .AsNoTracking()
                    .Include(r => r.Category)
                    .Include(r => r.Author)
                    .Where(r => !r.IsDeleted && r.Id == id)
                    .FirstOrDefaultAsync();
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> IsRecipeSavedByUserAsync(int recipeId, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return false;

            try
            {
                return await this._context.UserRecipes
                    .AnyAsync(ur => ur.RecipeId == recipeId && ur.UserId == userId);
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> SaveRecipeAsync(int recipeId, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return false;

            try
            {
                bool alreadySaved = await this._context.UserRecipes
                    .AnyAsync(ur => ur.RecipeId == recipeId && ur.UserId == userId);

                if (alreadySaved)
                    return false;

                bool recipeExists = await this._context.Recipes
                    .AnyAsync(r => r.Id == recipeId && !r.IsDeleted);

                Recipe recipe = this._context.Recipes.Find(recipeId);
                recipe.SavedCount++;
                if (!recipeExists)
                    return false;

                var userRecipe = new UserRecipe
                {
                    UserId = userId,
                    RecipeId = recipeId,
                };

                await this._context.UserRecipes.AddAsync(userRecipe);
                await this._context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<List<FavoriteRecipeViewModel>> GetUserFavoriteRecipesAsync(string userId)
        {
            try
            {
                var favoriteRecipes = await this._context.UserRecipes
                    .AsNoTracking()
                    .Include(ur => ur.Recipe)
                    .ThenInclude(r => r.Category)
                    .Include(ur => ur.Recipe)
                    .ThenInclude(r => r.Author)
                    .Where(ur => ur.UserId == userId && !ur.Recipe.IsDeleted)
                    .Select(ur => new FavoriteRecipeViewModel
                    {
                        Id = ur.Recipe.Id,
                        Title = ur.Recipe.Title,
                        ImageUrl = ur.Recipe.ImageUrl,
                        Category = ur.Recipe.Category.Name,
                        Author = ur.Recipe.Author.UserName ?? "Unknown Author",
                        CreatedOn = ur.Recipe.CreatedOn
                    })
                    .ToListAsync();

                return favoriteRecipes;
            }
            catch
            {
                return new List<FavoriteRecipeViewModel>();
            }
        }

        public async Task<bool> RemoveFromFavoritesAsync(int recipeId, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return false;

            try
            {
                var userRecipe = await this._context.UserRecipes
                    .FirstOrDefaultAsync(ur => ur.RecipeId == recipeId && ur.UserId == userId);

                if (userRecipe == null)
                    return false;

                this._context.UserRecipes.Remove(userRecipe);
                Recipe recipe = this._context.Recipes.Find(recipeId);
                recipe.SavedCount--;
                await this._context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<EditRecipeViewModel?> GetRecipeForEditAsync(int id, string userId)
        {
            try
            {
                var recipe = await this._context.Recipes
                    .AsNoTracking()
                    .Include(r => r.Category)
                    .Include(r => r.Author)
                    .Where(r => !r.IsDeleted && r.Id == id && r.AuthorId == userId)
                    .FirstOrDefaultAsync();
                if (recipe == null)
                {
                    return null;
                }
                
                return new EditRecipeViewModel
                {
                    Id = recipe.Id,
                    Title = recipe.Title,
                    Instructions = recipe.Instructions,
                    ImageUrl = recipe.ImageUrl,
                    CategoryId = recipe.CategoryId,
                    CreatedOn = recipe.CreatedOn,
                };
                
            }
            catch (Exception e)
            {
                throw;
            }
               

                
            
        }

        public async Task<bool> UpdateRecipeAsync(EditRecipeViewModel viewModel, string userId)
        {
            try
            {
                var recipe = await this._context.Recipes
                    .Where(r => r.Id == viewModel.Id && r.AuthorId == userId && !r.IsDeleted)
                    .FirstOrDefaultAsync();

                if (recipe == null)
                    return false;

                recipe.Title = viewModel.Title;
                recipe.Instructions = viewModel.Instructions;
                recipe.ImageUrl = viewModel.ImageUrl;
                recipe.CategoryId = viewModel.CategoryId;
                recipe.CreatedOn = viewModel.CreatedOn;

                this._context.Recipes.Update(recipe);
                await this._context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }public async Task<DeleteRecipeViewModel?> GetRecipeForDeleteAsync(int id, string userId)
        {
            try
            {
                var recipe = await this._context.Recipes
                    .AsNoTracking()
                    .Include(r => r.Category)
                    .Include(r => r.Author)
                    .Where(r => !r.IsDeleted && r.Id == id && r.AuthorId == userId)
                    .FirstOrDefaultAsync();

                if (recipe == null)
                    return null;

                return new DeleteRecipeViewModel
                {
                    Id = recipe.Id,
                    Title = recipe.Title,
                    Author = recipe.Author?.UserName ?? "Unknown Author",
                    AuthorId = recipe.AuthorId,
                    ImageUrl = recipe.ImageUrl,
                    Category = recipe.Category?.Name ?? "Unknown Category",
                    CreatedOn = recipe.CreatedOn,
                    Instructions = recipe.Instructions
                };
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> DeleteRecipeAsync(int id, string userId)
        {
            try
            {
                var recipe = await this._context.Recipes
                    .Where(r => r.Id == id && r.AuthorId == userId && !r.IsDeleted)
                    .FirstOrDefaultAsync();

                if (recipe == null)
                    return false;

                recipe.IsDeleted = true;

                var userRecipes = await this._context.UserRecipes
                    .Where(ur => ur.RecipeId == id)
                    .ToListAsync();

                if (userRecipes.Any())
                {
                    this._context.UserRecipes.RemoveRange(userRecipes);
                }

                await this._context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}