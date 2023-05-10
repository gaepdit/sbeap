using Sbeap.Domain.Entities.EntityBase;

namespace Sbeap.Domain.Entities.Agencies;

public class Agency : SimpleNamedEntity
{
    public Agency(Guid id, string name) : base(id, name) { }
}
