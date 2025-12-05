using Xunit;

namespace Events.Api.E2ETests;

/// <summary>
/// Defines a test collection that shares a single EventsApiFactory instance.
/// All tests in this collection run sequentially and share the same Postgres container.
/// </summary>
[CollectionDefinition("EventsApi")]
public class EventsApiCollection : ICollectionFixture<EventsApiFactory>
{
}

