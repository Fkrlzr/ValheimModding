// Helpers that stand in for compiler-generated calls the decompiler wrote out as explicit operator calls.
internal static class Compat
{
	public static bool Truthy(UnityEngine.Object o)
	{
		return (bool)o;
	}
}
