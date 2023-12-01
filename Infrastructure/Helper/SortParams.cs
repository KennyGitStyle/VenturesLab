namespace Infrastructure.Helper;

public class SortParams
{
    public DateOnly? Date { get; init; }
    public string SortBy { get; init; } = "CurrentDate";
    public bool Ascending { get; set; } = true;
}
