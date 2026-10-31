using UnityEngine;

namespace Data;

public interface IQuaternionValue
{
	Quaternion? Get(Functions f);

	bool? Match(Functions f, Quaternion value);
}
