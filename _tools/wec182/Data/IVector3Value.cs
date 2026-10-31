using UnityEngine;

namespace Data;

public interface IVector3Value
{
	Vector3? Get(Functions f);

	bool? Match(Functions f, Vector3 value);
}
