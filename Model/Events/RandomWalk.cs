using OkmcPrototype.Model.Objects;

namespace OkmcPrototype.Model.Events;

public class RandomWalk : EventDescriptor
{
    private Random _random = new Random();
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
        var sia = (SelfInterstitialAtom)modelObject;

        var lambda = 0.005f;

        if (_random.NextSingle() > 0.5)
        {
            sia.Position.Z += lambda;
        }
        else
        {
            sia.Position.Z -= lambda;
        }
    }
}
