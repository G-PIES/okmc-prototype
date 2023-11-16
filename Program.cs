using System.Globalization;
using OkmcPrototype;
using OkmcPrototype.Model;
using OkmcPrototype.Model.Events;
using OkmcPrototype.Model.Objects;

var size = 0.1 * Units.MilliMeters;
var parameters = new ModelParameters
{
    AttemptFrequency = Math.Pow(10, 13),
    MigrationEnergy = 0.34,
    Temperature = 130,
    RandomWalkDistance = (float)(5 * Units.NanoMeters),
    DimensionsX = new Range<double>(-size / 2, size / 2),
    DimensionsY = new Range<double>(-size / 2, size / 2),
    DimensionsZ = new Range<double>(-size / 4, size / 4),
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
simulation.Run(model, TimeSpan.FromMinutes(20));

ModelObject[] GenerateObjects()
{
    var count = 500;
    var objects = ImmutableArray.CreateBuilder<ModelObject>(count);
    var random = new Random();
    for (int i = 0; i < count; i++)
    {
        var type = i > count / 2
            ? DefectType.Interstitial
            : DefectType.Vacancy;
        var position = new Vector3(
            (float)random.NextDouble(parameters.DimensionsX),
            (float)random.NextDouble(parameters.DimensionsY),
            (float)random.NextDouble(parameters.DimensionsZ));
        objects.Add(new Defect(type, position));
    }

    return objects.ToArray();
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
