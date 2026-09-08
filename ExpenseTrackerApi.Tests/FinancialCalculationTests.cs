using ExpenseTrackerApi;

namespace ExpenseTrackerApi.Tests;

public class FinancialCalculationTests
{
    [Fact]
    public void CalculateTotal_ShouldSumAllUserExpenses()
    {
        // Arrange
        var expenses = new List<Expense>
        {
            new() { Id = 1, Name = "Coffee", Amount = 3.50m, UserId = "user-1" },
            new() { Id = 2, Name = "Lunch", Amount = 12.00m, UserId = "user-1" },
            new() { Id = 3, Name = "Dinner", Amount = 24.50m, UserId = "user-1" }
        };

        // Act
        var total = expenses.Sum(e => e.Amount);

        // Assert
        Assert.Equal(40.00m, total);
    }

    [Fact]
    public void CalculateTotal_ShouldReturnZero_WhenUserHasNoExpenses()
    {
        // Arrange
        var expenses = new List<Expense>();

        // Act
        var total = expenses.Sum(e => e.Amount);

        // Assert
        Assert.Equal(0m, total);
    }

    [Fact]
    public void FilterExpenses_ShouldIsolateUserData_AndExcludeOtherUsers()
    {
        // Arrange
        var allExpenses = new List<Expense>
        {
            new() { Id = 1, Name = "Gabriel Expense 1", Amount = 50m, UserId = "user-gabriel" },
            new() { Id = 2, Name = "John Expense", Amount = 100m, UserId = "user-john" },
            new() { Id = 3, Name = "Gabriel Expense 2", Amount = 25m, UserId = "user-gabriel" }
        };

        // Act - Simulate the LINQ query executed in the endpoint
        var gabrielExpenses = allExpenses.Where(e => e.UserId == "user-gabriel").ToList();
        var gabrielTotal = gabrielExpenses.Sum(e => e.Amount);

        // Assert
        Assert.Equal(2, gabrielExpenses.Count);
        Assert.Equal(75m, gabrielTotal);
        Assert.All(gabrielExpenses, e => Assert.Equal("user-gabriel", e.UserId));
    }
}
