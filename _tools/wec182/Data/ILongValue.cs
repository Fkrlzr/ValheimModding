namespace Data;

public interface ILongValue
{
	long? Get(Functions f);

	bool? Match(Functions f, long value);
}
