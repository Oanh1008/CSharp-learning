namespace ApisDotnetCore6.Exception;

public class NotFoundResourceException : System.Exception
{
    public NotFoundResourceException()
    {
    }

    public NotFoundResourceException(string message) : base(message)
    {
    }
}