namespace ExpenseTrackerApi;

public class CategoryDTO
{
    public int  Id { get; set; }
    public string? Name { get; set; }

    public CategoryDTO() { }

    public CategoryDTO(Category category)
    {
        Id = category.Id;
        Name = category.Name;
    }
}