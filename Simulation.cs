using OkmcPrototype.Model;

namespace OkmcPrototype;

public class Simulation
{
    public required SimulationParameters Parameters { get; init; }

    public void Run(Model.Model model, TimeSpan simulationTime)
    {
        model.Initialize();

        var random = new Random();

        var currentTime = 0.0;

        while (currentTime < simulationTime.TotalSeconds)
        {
            var timeIncrement = CalculateTimeIncrement(model);

            foreach (var eventDescriptor in model.Events)
            {
                var probabilities = CalculateEventProbabilities(eventDescriptor, timeIncrement);

                foreach (var modelObject in model.Objects)
                {
                    var randomDouble = random.NextDouble();
                    var numberOfTimes = CalculateNumberOfTimes(probabilities, randomDouble);
                    for (int i = 0; i < numberOfTimes; i++)
                    {
                        eventDescriptor.Execute(model, modelObject);
                    }
                }
            }

            currentTime += timeIncrement;
        }
    }

    private double CalculateTimeIncrement(Model.Model model)
    {
        var maxRate = model.Events.Max(e => e.Rate);
        return Parameters.Omega / maxRate;
    }

    private double CalculateEventProbability(EventDescriptor eventDescriptor, int numberOfTimes, double timeIncrement)
    {
        var result = Math.Pow(eventDescriptor.Rate * timeIncrement, numberOfTimes);
        result /= MathExtensions.Factorial(numberOfTimes);
        result *= Math.Exp(-eventDescriptor.Rate * timeIncrement);
        return result;
    }

    private ImmutableArray<double> CalculateEventProbabilities(EventDescriptor eventDescriptor, double timeIncrement)
    {
        var probabilities = ImmutableArray.CreateBuilder<double>();
        var sum = 0.0;
        var numberOfTimes = 0;
        while (sum < 0.999999)
        {
            sum += CalculateEventProbability(eventDescriptor, numberOfTimes, timeIncrement);
            probabilities.Add(sum);
            numberOfTimes++;
        }

        return probabilities.ToImmutableArray();
    }

    private int CalculateNumberOfTimes(ImmutableArray<double> probabilities, double randomDouble)
    {
        var index = 0;
        for (; index < probabilities.Length; index++)
        {
            if (probabilities[index] > randomDouble)
            {
                return index;
            }
        }

        return index;
    }
}
