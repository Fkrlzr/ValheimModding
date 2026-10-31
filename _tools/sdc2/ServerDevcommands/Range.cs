namespace ServerDevcommands;

public class Range<T>
{
	public T Min;

	public T Max;

	public bool Uniform;

	public Range(T value)
	{
		Min = value;
		Max = value;
	}

	public Range(T min, T max)
	{
		Min = min;
		Max = max;
	}
}
