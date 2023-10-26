namespace OkmcPrototype;

public readonly record struct Range<T>(T From, T To) where T : struct;
