namespace ApisDotnetCore6.Models;

public class ExceptionHandleModel
{
    public int StatusCode { set; get; }

    public string? Message { set; get; }

    public ExceptionHandleModel(int statusCode, string message)
    {
        this.StatusCode = statusCode;
        this.Message = message;
    }
}