namespace Common;

public class ValueRange<T>
{
	public T Min;

	public T Max;

	public bool Uniform = true;

	public ValueRange(T value)
	{
		Min = value;
		Max = value;
	}

	public ValueRange(T min, T max)
	{
		Min = min;
		Max = max;
	}
}
