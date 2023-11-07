namespace OkmcPrototype.Model;

public class ModelParameters
{
    /// <summary>
    /// v_0
    /// </summary>
    public required double AttemptFrequency { get; init; }

    /// <summary>
    /// E_m
    /// </summary>
    public required double MigrationEnergy { get; init; }

    /// <summary>
    /// T
    /// </summary>
    public required double Temperature { get; init; }

    public required float RandomWalkDistance { get; init; }

    public required Range<double> DimensionsX { get; init; }
    public required Range<double> DimensionsY { get; init; }
    public required Range<double> DimensionsZ { get; init; }
}
