namespace Data;

public interface IIntValue
{
	int? Get(Functions f);

	bool? Match(Functions f, int value);
}
