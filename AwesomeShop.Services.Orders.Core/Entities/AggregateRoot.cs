using AwesomeShop.Services.Orders.Core.Events;

namespace AwesomeShop.Services.Orders.Core.Entities;

public class AggregateRoot : IEntityBase {
    private readonly ICollection<IDomainEvent> _events = [];

    public Guid Id { get; protected set; }

    public IEnumerable<IDomainEvent> Events => this._events;

    protected void AddEvent(IDomainEvent @event) => this._events.Add(@event);
}