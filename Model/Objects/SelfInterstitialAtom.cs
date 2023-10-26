namespace OkmcPrototype.Model.Objects;

public class SelfInterstitialAtom : ModelObject
{
    private Vector3 _position;

    public SelfInterstitialAtom(Vector3 position)
    {
        Position = position;
    }
    public ref Vector3 Position => ref _position;
}
