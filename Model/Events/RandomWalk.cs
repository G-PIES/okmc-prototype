using OkmcPrototype.Model.Objects;

namespace OkmcPrototype.Model.Events;

public class RandomWalk : EventDescriptor
{
    private const float TwoPi = (float)(2 * Math.PI);
    private readonly Random _random = new();
    private double _rate;
    public override double Rate => _rate;

    public override void Initialize(Model model)
    {
        base.Initialize(model);
        var parameters = model.Parameters;

        var v0 = parameters.AttemptFrequency;
        var Em = parameters.MigrationEnergy;
        var kB = Constants.BoltzmannConstant;
        var T = parameters.Temperature;

        _rate = v0 * Math.Exp(-Em / (kB * T));
    }

    public override void Execute(Model model, ModelObject modelObject)
    {
        var sia = (Defect)modelObject;

        if (sia.Size >= 5)
        {
            return;
        }

        var lambda = model.Parameters.RandomWalkDistance;

        sia.Position += GenerateRandomVector(lambda);
    }

    private Vector3 GenerateRandomVector(float length)
    {
        var alpha = _random.NextSingle(new Range<float>(0, TwoPi));
        var beta = _random.NextSingle(new Range<float>(0, TwoPi));
        var cosAlpha = (float)Math.Cos(alpha);
        var cosBeta = (float)Math.Cos(beta);
        var sinAlpha = (float)Math.Sin(alpha);
        var sinBeta = (float)Math.Sin(beta);

        return new Vector3(
            length * cosAlpha * cosBeta,
            length * sinAlpha * cosBeta,
            length * sinBeta
        );
    }
}
