using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RecipeSharingPlatform.Data.Models;
using RecipeSharingPlatform.Services.Core;
using RecipeSharingPlatform.Services.Core.Contracts;
using RecipeSharingPlatform.ViewModels.Recipe;

namespace RecipeSharingPlatform.Web.Controllers
{
    public class RecipeController : BaseController
    {
        private readonly IRecipeService _recipeService;
        private readonly ICategoryService _categoryService;

        public RecipeController(IRecipeService recipeService, ICategoryService categoryService)
        {
            this._recipeService = recipeService;
            this._categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string userId = GetUserId();

            var allRecipes = this._recipeService.GetAll<Recipe>();

            var viewModels = new List<RecipesInListViewModel>();

            foreach (var recipe in allRecipes)
            {
                var viewModel = new RecipesInListViewModel
                {
                    Id = recipe.Id,
                    Title = recipe.Title,
                    Instructions = recipe.Instructions,
                    ImageUrl = recipe.ImageUrl,
                    AuthorId = recipe.AuthorId,
                    AuthorName = recipe.Author?.UserName ?? "Unknown Author",
                    CreatedOn = recipe.CreatedOn,
                    CategoryId = recipe.CategoryId,
                    CategoryName = recipe.Category?.Name ?? "Unknown Category",
                    IsDeleted = recipe.IsDeleted,
                    IsAuthor = recipe.AuthorId == userId,
                    IsSaved = await this._recipeService.IsRecipeSavedByUserAsync(recipe.Id, userId),
                    SavedCount = recipe.SavedCount,
                };

                viewModels.Add(viewModel);
            }

            return View(viewModels);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Create()
        {
            var viewModel = new CreateRecipeViewModel();
            await PopulateCategoriesAsync(viewModel);
            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRecipeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateCategoriesAsync(model);
                return View(model);
            }

            string userId = GetUserId();

            try
            {
                bool isCreated = await this._recipeService.CreateAsync(model, userId);

                if (isCreated)
                {
                    TempData["SuccessMessage"] = "Recipe created successfully!";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An unexpected error occurred while creating the recipe.");
            }

            TempData["ErrorMessage"] = "An error occurred while creating the recipe. Please try again.";
            await PopulateCategoriesAsync(model);
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                string userId = GetUserId();

                var recipe = await this._recipeService.GetByIdAsync(id);

                if (recipe == null)
                {
                    TempData["ErrorMessage"] = "Recipe not found.";
                    return RedirectToAction(nameof(Index));
                }

                var viewModel = new RecipeDetailsViewModel
                {
                    Id = recipe.Id,
                    Title = recipe.Title,
                    Instructions = recipe.Instructions,
                    ImageUrl = recipe.ImageUrl,
                    Author = recipe.Author?.UserName ?? "Unknown Author",
                    Category = recipe.Category?.Name ?? "Unknown Category",
                    CreatedOn = recipe.CreatedOn,
                    IsAuthor = recipe.AuthorId == userId,
                    IsSaved = await this._recipeService.IsRecipeSavedByUserAsync(recipe.Id, userId)
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading the recipe details.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(int id)
        {
            try
            {
                string userId = GetUserId();

                if (string.IsNullOrEmpty(userId))
                {
                    TempData["ErrorMessage"] = "You must be logged in to save recipes.";
                    return RedirectToAction("Details", new { id = id });
                }

                bool isSaved = await this._recipeService.SaveRecipeAsync(id, userId);

                if (isSaved)
                {
                    TempData["SuccessMessage"] = "Recipe saved to favorites successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to save recipe. It may already be in your favorites.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while saving the recipe.";
            }

            return RedirectToAction("Favorites");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Favorites()
        {
            try
            {
                string userId = GetUserId();

                if (string.IsNullOrEmpty(userId))
                {
                    TempData["ErrorMessage"] = "You must be logged in to view your favorites.";
                    return RedirectToAction("Login", "Account");
                }

                var favoriteRecipes = await this._recipeService.GetUserFavoriteRecipesAsync(userId);

                return View(favoriteRecipes);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading your favorite recipes.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(int id)
        {
            try
            {
                string userId = GetUserId();

                if (string.IsNullOrEmpty(userId))
                {
                    TempData["ErrorMessage"] = "You must be logged in to remove favorites.";
                    return RedirectToAction("Login", "Account");
                }

                bool isRemoved = await this._recipeService.RemoveFromFavoritesAsync(id, userId);

                if (isRemoved)
                {
                    TempData["SuccessMessage"] = "Recipe removed from favorites successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to remove recipe from favorites.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while removing the recipe from favorites.";
            }

            return RedirectToAction(nameof(Favorites));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            
                string userId = GetUserId();

                if (string.IsNullOrEmpty(userId))
                {
                    TempData["ErrorMessage"] = "You must be logged in to edit recipes.";
                    return Redirect("Identity/Account/Login");
                }

                var viewModel = await this._recipeService.GetRecipeForEditAsync(id, userId);
                if (viewModel == null)
                {
                    return RedirectToAction("Index", "Recipe");
                }
                await PopulateCategoriesForEditAsync(viewModel);
                return View(viewModel);
         
        }
        

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditRecipeViewModel model)
        {
            if (model == null)
            {
                return Redirect("Identity/Account/Login");
            }
            if (!ModelState.IsValid)
            {
                await PopulateCategoriesForEditAsync(model);
                return View(model);
            }

            try
            {
                string userId = GetUserId();

                if (string.IsNullOrEmpty(userId))
                {
                    TempData["ErrorMessage"] = "You must be logged in to edit recipes.";
                    return RedirectToAction("Login", "Account");
                }

                bool isUpdated = await this._recipeService.UpdateRecipeAsync(model, userId);

                if (isUpdated)
                {
                    TempData["SuccessMessage"] = "Recipe updated successfully!";
                    return RedirectToAction("Details", new { id = model.Id });
                }
                else
                {
                    TempData["ErrorMessage"] =
                        "Failed to update recipe. You may not have permission to edit this recipe.";
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An unexpected error occurred while updating the recipe.");
            }

            await PopulateCategoriesForEditAsync(model);
            return View(model);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                string userId = GetUserId();

                if (string.IsNullOrEmpty(userId))
                {
                    TempData["ErrorMessage"] = "You must be logged in to delete recipes.";
                    return RedirectToAction("Login", "Account");
                }

                var viewModel = await this._recipeService.GetRecipeForDeleteAsync(id, userId);

                if (viewModel == null)
                {
                    TempData["ErrorMessage"] = "Recipe not found or you don't have permission to delete it.";
                    return RedirectToAction(nameof(Index));
                }

                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading the recipe for deletion.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(DeleteRecipeViewModel model)
        {
            try
            {
                string userId = GetUserId();

                if (string.IsNullOrEmpty(userId))
                {
                    TempData["ErrorMessage"] = "You must be logged in to delete recipes.";
                    return RedirectToAction("Login", "Account");
                }

                if (model.AuthorId != userId)
                {
                    TempData["ErrorMessage"] = "You don't have permission to delete this recipe.";
                    return RedirectToAction(nameof(Index));
                }

                bool isDeleted = await this._recipeService.DeleteRecipeAsync(model.Id, userId);

                if (isDeleted)
                {
                    TempData["SuccessMessage"] = $"Recipe '{model.Title}' has been deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to delete the recipe. Please try again.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the recipe.";
            }

            return RedirectToAction(nameof(Index));
        }
        
        private async Task PopulateCategoriesAsync(CreateRecipeViewModel model)
        {
            try
            {
                model.Categories = await this._categoryService.GetCategoriesForSelectAsync();
            }
            catch (Exception)
            {
                model.Categories = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                ModelState.AddModelError("", "Unable to load categories. Please try again.");
            }
        }
        private async Task PopulateCategoriesForEditAsync(EditRecipeViewModel model)
        {
            try
            {
                var categories = await this._categoryService.GetCategoriesForSelectAsync();
                model.Categories = categories ?? new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            }
            catch (Exception)
            {
                model.Categories = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                ModelState.AddModelError("", "Unable to load categories. Please try again.");
            }
        }
    }
}