namespace Application.Common.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class AuthorizeAttribute : Attribute
{
    public string Role { get; set; } = string.Empty;

    public string Policy { get; set; } = string.Empty;

    public AuthorizeAttribute() { }
}
