namespace Data;

public interface IFloatValue
{
	float? Get(Functions f);

	bool TryGet(Functions f, out float value);

	bool? Match(Functions f, float value);
}
