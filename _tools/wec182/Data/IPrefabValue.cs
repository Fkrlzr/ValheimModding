namespace Data;

public interface IPrefabValue
{
	int? Get(Functions f);

	bool? Match(Functions f, int value);
}
