namespace Data;

public interface IHashValue
{
	int? Get(Functions f);

	bool? Match(Functions f, int value);
}
