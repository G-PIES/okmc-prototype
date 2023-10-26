namespace OkmcPrototype.Model;

public abstract class EventDescriptor
{
    public abstract double Rate { get; }

    public virtual void Initialize(Model model)
    {
    }

    public abstract void Execute(Model model, ModelObject modelObject);
}
