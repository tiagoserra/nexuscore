namespace Application.Common.Interfaces;

public interface IExecutionContext
{
    bool HasRole(string role);

    bool HasPolicy(string policy);
}