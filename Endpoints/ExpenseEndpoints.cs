using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerApi;

public static class ExpenseEndpoints
{
    public static void MapExpenseEndpoints(this IEndpointRouteBuilder app)
    {
        var expenses = app.MapGroup("/expenses").RequireAuthorization();
        expenses.MapGet("/", GetAllExpenses);
        expenses.MapPost("/", PostExpense);
        expenses.MapGet("/{id}", GetExpense);
        expenses.MapPut("/{id}", UpdateExpense);
        expenses.MapPatch("/{id}", PatchExpense);
        expenses.MapDelete("/{id}", DeleteExpense);
        expenses.MapGet("/total", GetTotalExpenses);
    }

    private static async Task<IResult> GetAllExpenses(ExpenseTrackerDb db,  ClaimsPrincipal user)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

        return TypedResults.Ok(await db.Expenses
            .AsNoTracking()
            .Where(e => e.UserId == userId)
            .Select(x => new ExpenseDTO(x))
            .ToListAsync());
    }

    private static async Task<IResult> PostExpense(ExpenseTrackerDb db, ExpenseDTO expenseDTO, ClaimsPrincipal user)
    {
        var expense = new Expense()
        {
            Name = expenseDTO.Name,
            Amount = expenseDTO.Amount,
            Description = expenseDTO.Description,
            CategoryId = expenseDTO.CategoryId,
            Date = expenseDTO.Date == default ? DateTime.UtcNow : expenseDTO.Date
        };
        db.Expenses.Add(expense);
        
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        expense.UserId = userId;
        
        await db.SaveChangesAsync();
        expenseDTO.Id = expense.Id;
        return TypedResults.Created($"/expenses/{expense.Id}", expenseDTO);
    }

    private static async Task<IResult> GetTotalExpenses(ExpenseTrackerDb db, ClaimsPrincipal user)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var total = await db.Expenses.Where(e => e.UserId == userId)
            .SumAsync(e => e.Amount);
        
        return TypedResults.Ok(new { Total = total });
    }

    public static async Task<IResult> DeleteExpense(int id, ExpenseTrackerDb db)
    {
        if (await db.Expenses.FindAsync(id) is Expense expense)
        {
            db.Expenses.Remove(expense);
            await db.SaveChangesAsync();
            return TypedResults.NoContent();
        }
        return TypedResults.NotFound();
    }

    private static async Task<IResult> PatchExpense(ExpenseTrackerDb db, ExpenseDTO expenseDTO, int id)
    {
        var expense = await db.Expenses.FindAsync(id);
        
        if (expense is null) return TypedResults.NotFound();

        if (expense.Name is not null) expense.Name = expenseDTO.Name;
        if (expense.Description is not null) expense.Description = expenseDTO.Description;

        await db.SaveChangesAsync();
        return Results.Ok();
    }

    private static async Task<IResult> UpdateExpense(int id, ExpenseDTO expenseDTO, ExpenseTrackerDb db)
    {
        var expense = await db.Expenses.FindAsync(id);
        if (expense is null) return TypedResults.NotFound();
        if (expense.Name is not null) expense.Name = expenseDTO.Name;
        if (expense.Description is not null) expense.Description = expenseDTO.Description;
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    private static async Task<IResult> GetExpense(int id, ExpenseTrackerDb db)
    {
        var expense = await db.Expenses.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        return expense is not null 
            ? TypedResults.Ok(expense)
            : TypedResults.NotFound();
    }
}
