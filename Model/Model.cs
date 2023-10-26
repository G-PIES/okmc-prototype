namespace OkmcPrototype.Model;

public class Model
{
    public required ModelParameters Parameters { get; init; }
    public required ImmutableArray<ModelObject> Objects { get; init; }
    public required ImmutableArray<EventDescriptor> Events { get; init; }

    public void Initialize()
    {
        foreach (var e in Events)
        {
            e.Initialize(this);
        }
    }
}
