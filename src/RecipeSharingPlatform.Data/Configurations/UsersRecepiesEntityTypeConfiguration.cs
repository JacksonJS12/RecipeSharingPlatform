using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeSharingPlatform.Data.Models;

namespace RecipeSharingPlatform.Data.Configurations;

public class UsersRecepiesEntityTypeConfiguration : IEntityTypeConfiguration<UserRecipe>
{
    public void Configure(EntityTypeBuilder<UserRecipe> builder)
    {
        builder.HasKey(ur => new { ur.UserId, ur.RecipeId });
        
        builder
            .HasOne(ur => ur.User)
            .WithMany()
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade); 
        
        builder
            .HasOne(ur => ur.Recipe)
            .WithMany(r => r.UsersRecipes)
            .HasForeignKey(ur => ur.RecipeId)
            .OnDelete(DeleteBehavior.NoAction); 
        
        builder.ToTable("UserRecipes");
        
        builder.Property(ur => ur.UserId)
            .IsRequired()
            .HasMaxLength(450); 
            
        builder.Property(ur => ur.RecipeId)
            .IsRequired();
    }
}