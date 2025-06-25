namespace RecipeSharingPlatform.GCommon
{
    public static class ValidationConstants
    {
        public static class Recipe
        {
            public const int RecipeTitleMinLength = 3;
            public const int RecipeTitleMaxLength = 80;
        
            public const int RecipeInstructionsMinLength = 10;
            public const int RecipeInstructionsMaxLength = 250;
        }

        public static class Category
        {
            public const int CategoryNameMinLength = 3;
            public const int CategoryNameMaxLength = 20;
        }
    }
}
