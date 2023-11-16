using OkmcPrototype.Model;
using OkmcPrototype.Model.Objects;

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
                    if (modelObject == null)
                    {
                        continue;
                    }

                    var randomDouble = random.NextDouble();
                    var numberOfTimes = CalculateNumberOfTimes(probabilities, randomDouble);
                    for (int i = 0; i < numberOfTimes; i++)
                    {
                        eventDescriptor.Execute(model, modelObject);
                    }
                }
            }

            ProcessOutOfBox(model);
            ProcessInteractions(model);

            currentTime += timeIncrement;
        }
    }

    private void ProcessOutOfBox(Model.Model model)
    {
        for (var i = 0; i < model.Objects.Length; i++)
        {
            var defect = model.Objects[i] as Defect;
            if (defect == null)
            {
                continue;
            }

            if (!IsInModelDimensions(defect.Position))
            {
                model.Objects[i] = null;
            }
        }

        bool IsInModelDimensions(Vector3 position)
        {
            return position.X > model.Parameters.DimensionsX.From &&
                position.X < model.Parameters.DimensionsX.To &&
                position.Y > model.Parameters.DimensionsY.From &&
                position.Y < model.Parameters.DimensionsY.To &&
                position.Z > model.Parameters.DimensionsZ.From &&
                position.Z < model.Parameters.DimensionsZ.To;
        }
    }

    private static void ProcessInteractions(Model.Model model)
    {
        for (var i = 0; i < model.Objects.Length; i++)
        {
            var first = model.Objects[i] as Defect;
            if (first == null)
            {
                continue;
            }

            for (var j = i + 1; j < model.Objects.Length; j++)
            {
                var second = model.Objects[j] as Defect;
                if (second == null)
                {
                    continue;
                }

                if (CloseEnough(first, second))
                {
                    model.Objects[j] = null;
                    if (first.Type == second.Type)
                    {
                        first.Size++;
                    }
                    else
                    {
                        first.Size--;
                        if (first.Size == 0)
                        {
                            model.Objects[i] = null;
                        }
                    }
                }
            }
        }

        bool CloseEnough(Defect first, Defect second)
        {
            var distance = Vector3.Distance(first.Position, second.Position);
            return distance < model.Parameters.RandomWalkDistance;
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
