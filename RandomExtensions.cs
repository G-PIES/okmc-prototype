namespace OkmcPrototype;

public static class RandomExtensions
{
    public static double NextDouble(this Random random, Range<double> range)
    {
        return range.From + random.NextDouble() * (range.To - range.From);
    }

    public static float NextSingle(this Random random, Range<float> range)
    {
        return range.From + random.NextSingle() * (range.To - range.From);
    }
}
