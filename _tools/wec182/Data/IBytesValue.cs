namespace Data;

public interface IBytesValue
{
	byte[]? Get(Functions f);

	bool? Match(Functions f, byte[]? value);
}
