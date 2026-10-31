namespace Data;

public interface IZdoIdValue
{
	ZDOID? Get(Functions f);

	bool? Match(Functions f, ZDOID value);
}
