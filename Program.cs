using System.Globalization;
using OkmcPrototype;
using OkmcPrototype.Model;
using OkmcPrototype.Model.Events;
using OkmcPrototype.Model.Objects;

var parameters = new ModelParameters
{
    AttemptFrequency = Math.Pow(10, 13),
    MigrationEnergy = 0.34,
    Temperature = 130,
    DimensionsX = new Range<double>(0, 36 * Constants.BohrRadius),
    DimensionsY = new Range<double>(0, 36 * Constants.BohrRadius),
    DimensionsZ = new Range<double>(0, 216 * Constants.BohrRadius),
};

var model = new Model
{
    Parameters = parameters,
    Objects = GenerateObjects(),
    Events = ImmutableArray.Create<EventDescriptor>(
        new RandomWalk()
    ),
};

var simulation = new Simulation
{
    Parameters = new SimulationParameters
    {
        Omega = 1
    }
};
simulation.Run(model, TimeSpan.FromSeconds(30));

PrintByBuckets(model.Objects.Cast<SelfInterstitialAtom>(), 100, sia => sia.Position.Z);

ImmutableArray<ModelObject> GenerateObjects()
{
    var objects = ImmutableArray.CreateBuilder<ModelObject>();
    var random = new Random();
    for (int i = 0; i < 1000; i++)
    {
        var position = new Vector3(
            (float)random.NextDouble(parameters.DimensionsX),
            (float)random.NextDouble(parameters.DimensionsX),
            (float)(parameters.DimensionsZ.To / 2));
        objects.Add(new SelfInterstitialAtom(position));
    }

    return objects.ToImmutableArray();
}

void PrintByBuckets<TEntity>(
    IEnumerable<TEntity> entities,
    int buckets,
    Func<TEntity, double> func)
{
    var min = entities.Min(func);
    var max = entities.Max(func);

    var delta = (max - min) / (buckets - 1);

    var numbers = new int[buckets];
    foreach (var entity in entities)
    {
        var bucket = (int)((func(entity) - min) / delta);
        numbers[bucket]++;
    }

    for (int i = 0; i < buckets; i++)
    {
        var number = numbers[i];
        if (number == 0)
        {
            continue;
        }
        var value = min + delta * i;
        Console.Write(value.ToString(CultureInfo.InvariantCulture));
        Console.Write("\t");
        Console.Write(number.ToString(CultureInfo.InvariantCulture));
        Console.WriteLine();
    }
}
