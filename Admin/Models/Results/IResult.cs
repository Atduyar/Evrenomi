namespace Admin.Models.Results
{
    public interface IResult
    {
        bool Success { get; }
        string Message { get; }
    }
}