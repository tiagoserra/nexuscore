using Application.Common.Interfaces;

namespace Infrastructure.Manager.Common;

public class ExecutionContext : IExecutionContext
{
    public bool HasPolicy(string policy)
        => true;

    public bool HasRole(string role)
        => true;

    public Guid ExecutionId
        => new Guid(); 
}