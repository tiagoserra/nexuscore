namespace Application.Common.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class AuditAttribute : Attribute
{
    public bool IsAuditable { get; set; }

    public AuditAttribute(bool isAuditable)
        => IsAuditable = isAuditable;
}
