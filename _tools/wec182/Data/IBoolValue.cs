namespace Data;

public interface IBoolValue
{
	int? GetInt(Functions f);

	bool? GetBool(Functions f);

	bool? Match(Functions f, bool value);
}
