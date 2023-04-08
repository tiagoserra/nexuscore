using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Common.Entities;

public abstract class Entity
{
    public long Id { get; }
    public DateTime CreatedOn { get; private set; }
    public string CreatedBy { get; private set; }
    public DateTime? ModifiedOn { get; private set; }
    public string ModifiedBy { get; private set; }

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
}