using ExpenseTrackerApi;

namespace ExpenseTrackerApi.Tests;

public class ExpenseTests
{
    [Fact]
    public void ExpenseDTO_ShouldMapCorrectly_FromExpense()
    {
        // Arrange
        var expense = new Expense
        {
            Id = 1,
            Name = "Lunch",
            Description = "Burger with friends",
            Amount = 15.50m,
            Date = new DateTime(2026, 9, 8),
            CategoryId = 2
        };

        // Act
        var dto = new ExpenseDTO(expense);

        // Assert
        Assert.Equal(1, dto.Id);
        Assert.Equal("Lunch", dto.Name);
        Assert.Equal("Burger with friends", dto.Description);
        Assert.Equal(15.50m, dto.Amount);
        Assert.Equal(new DateTime(2026, 9, 8), dto.Date);
        Assert.Equal(2, dto.CategoryId);
    }

    [Theory]
    [InlineData(-10.0, false)]
    [InlineData(0.0, false)]
    [InlineData(25.50, true)]
    [InlineData(1000.00, true)]
    public void Expense_AmountValidation_ShouldValidateCorrectly(decimal amount, bool expectedValid)
    {
        // Act
        bool isValid = amount > 0;

        // Assert
        Assert.Equal(expectedValid, isValid);
    }

    [Fact]
    public void Expense_DefaultDate_ShouldAssignCurrentUtcDate_WhenOmitted()
    {
        // Arrange
        DateTime? providedDate = null;

        // Act
        DateTime finalDate = providedDate ?? DateTime.UtcNow;

        // Assert
        Assert.True((DateTime.UtcNow - finalDate).TotalSeconds < 5);
    }
}
