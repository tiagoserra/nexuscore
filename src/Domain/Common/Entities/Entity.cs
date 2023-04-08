using Domain.Common.Events;

namespace Domain.Common.Entities;

public abstract class Entity
{
    public long Id { get; }
    public DateTime CreatedOn { get; private set; }
    public string CreatedBy { get; private set; }
    public DateTime? ModifiedOn { get; private set; }
    public string ModifiedBy { get; private set; }
    private readonly List<DomainEvent> _domainEvents = new();
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void SetCreation(string createdBy)
    {
        CreatedOn = DateTime.Now;
        CreatedBy = createdBy;
    }

    public void SetModification(string modifiedBy)
    {
        ModifiedBy = modifiedBy;
        ModifiedOn = DateTime.Now;
    }

    public void AddDomainEvent(DomainEvent domainEvent)
        => _domainEvents.Add(domainEvent);

    public void RemoveDomainEvent(DomainEvent domainEvent)
        => _domainEvents.Remove(domainEvent);

    public void ClearDomainEvents()
        => _domainEvents.Clear();
}