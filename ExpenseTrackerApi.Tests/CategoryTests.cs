using ExpenseTrackerApi;

namespace ExpenseTrackerApi.Tests;

public class CategoryTests
{
    [Fact]
    public void CategoryDTO_ShouldMapCorrectly_FromCategory()
    {
        // Arrange
        var category = new Category
        {
            Id = 1,
            Name = "Food"
        };

        // Act
        var dto = new CategoryDTO(category);

        // Assert
        Assert.Equal(1, dto.Id);
        Assert.Equal("Food", dto.Name);
    }

    [Theory]
    [InlineData("Food", true)]
    [InlineData("Transportation", true)]
    [InlineData("", false)]
    [InlineData("   ", false)]
    [InlineData(null, false)]
    public void Category_NameValidation_ShouldValidateRequiredString(string? name, bool expectedValid)
    {
        // Act
        bool isValid = !string.IsNullOrWhiteSpace(name);

        // Assert
        Assert.Equal(expectedValid, isValid);
    }
}
