namespace OkmcPrototype;

public static class MathExtensions
{
    private static readonly Dictionary<int, int> Factorials = new() { { 0, 1 } };

    public static int Factorial(int n)
    {
        if (!Factorials.TryGetValue(n, out var result))
        {
            result = n * Factorial(n - 1);
            Factorials.Add(n, result);
        }

        return result;
    }
}
