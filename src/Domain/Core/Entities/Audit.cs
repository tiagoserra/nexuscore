namespace Domain.Core.Entities;

public class Audit
{
    public long Id { get; set; }
    public DateTime CreatedOn { get; }
    public Guid ExecutionContextId { get; }
    public string CommandName { get; private set; }
    public string Request { get; private set; }
    public string Response { get; private set; }
    public TimeSpan ExecutionTime { get; private set; }

    protected Audit()
    {
        
    }

    public Audit(Guid executionContextId , string request, string response, TimeSpan executionTime, string commandName)
    {
        CreatedOn =  DateTime.Now;
        ExecutionContextId = executionContextId;
        Request = request;
        Response = response;
        ExecutionTime = executionTime;
        CommandName = commandName;
    }
}