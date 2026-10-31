namespace Data;

public interface IStringValue
{
	string? Get(Functions f);

	string? GetWhole(Functions f);

	bool? Match(Functions f, string value);
}
