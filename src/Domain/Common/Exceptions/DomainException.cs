namespace Domain.Common.Exceptions;

public class DomainException : Exception
{
    public DomainException(string messageError)
        :base(messageError)
    {
    }
    
    public static void When(bool condition, string field, string messageError)
    {
        if (condition) throw new DomainException($"{field} {messageError}");
    }

    public static void When(bool condition, string messageError)
    {
        if (condition) throw new DomainException(messageError);
    }
}