using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerApi;

public static class CategoryEndpoints
{
    public static void MapCategoryEndpoints(this IEndpointRouteBuilder app)
    {
        var category = app.MapGroup("/categories");
        category.MapPost("/", PostCategory);
        category.MapGet("/", GetAllCategories);
    }

    public static async Task<IResult> GetAllCategories(ExpenseTrackerDb db)
    {
        return TypedResults.Ok(await db.Categories.AsNoTracking().Select(x => new CategoryDTO(x)).ToListAsync()); 
    }

    public static async Task<IResult> PostCategory(ExpenseTrackerDb db, CategoryDTO categoryDTO)
    {
        var category = new Category
        {
            Name = categoryDTO.Name
        };
        db.Categories.Add(category);
        await db.SaveChangesAsync();
        categoryDTO = new(category);
        return TypedResults.Created($"/categories/{category.Id}", categoryDTO);
    }
}
