namespace OkmcPrototype.Model.Objects;

public enum DefectType
{
    Interstitial = 1,
    Vacancy
}

public class Defect : ModelObject
{
    private Vector3 _position;

    public Defect(DefectType type, Vector3 position)
    {
        Type = type;
        Position = position;
        Size = 1;
    }

    public DefectType Type { get; }
    public int Size { get; set; }
    public ref Vector3 Position => ref _position;
}
