using Events.Domain.Entities;
using System.Linq.Expressions;

namespace Events.Application.Builders;

public class EventsFilterSpecificationBuilder
{
    private readonly List<Expression<Func<Event, bool>>> Filters = new();

    public EventsFilterSpecificationBuilder FilterByUserId(Guid userId)
    {
        Filters.Add(x => x.UserId == userId);
        return this;
    }

    public EventsFilterSpecificationBuilder FilterByPublic(bool isPublic)
    {
        Filters.Add(x => x.IsPublic == isPublic);
        return this;
    }

    public EventsFilterSpecificationBuilder FilterByName(string name)
    {
        Filters.Add(x => x.Name.ToLower().Contains(name.ToLower()));
        return this;
    }

    public EventsFilterSpecificationBuilder FilterByMinAndMaxAge(int minAge, int maxAge)
    {
        Filters.Add(x => x.MinAllowedAge == minAge && x.MaxAllowedAge == maxAge);
        return this;
    }

    public EventsFilterSpecificationBuilder FilterByActive(bool isActive)
    {
        Filters.Add(x => x.IsActive == isActive);
        return this;
    }

    public List<Expression<Func<Event, bool>>> Build()
    {
        return Filters;
    }
}
