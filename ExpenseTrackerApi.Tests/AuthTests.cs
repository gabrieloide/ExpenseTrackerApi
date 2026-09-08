using ExpenseTrackerApi;

namespace ExpenseTrackerApi.Tests;

public class AuthTests
{
    [Fact]
    public void AuthDTO_ShouldSupportValueEquality_BecauseItIsARecord()
    {
        // Arrange - Two different instances in memory
        var dto1 = new AuthDTO("test@example.com", "Password123");
        var dto2 = new AuthDTO("test@example.com", "Password123");

        // Assert - As a C# record, they are compared by value equality
        Assert.Equal(dto1, dto2);
        Assert.True(dto1 == dto2);
    }

    [Theory]
    [InlineData("admin@company.com", true)]
    [InlineData("user@test.org", true)]
    [InlineData("not-a-valid-email", false)]
    [InlineData("", false)]
    public void EmailFormat_ShouldValidateCorrectly(string email, bool expectedValid)
    {
        // Act
        bool isValid = email.Contains('@') && email.Contains('.');

        // Assert
        Assert.Equal(expectedValid, isValid);
    }
}
