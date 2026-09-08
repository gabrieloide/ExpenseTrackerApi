namespace ExpenseTrackerApi;

public class ExpenseDTO
{
    public  int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public int ? CategoryId { get; set; }
    
    public ExpenseDTO()
    {
    }

    public ExpenseDTO(Expense expense)
    {
        Id = expense.Id;
        Name = expense.Name;
        Description = expense.Description;
        Date = expense.Date;
        Amount = expense.Amount;
        CategoryId = expense.CategoryId;
        
    }   
}