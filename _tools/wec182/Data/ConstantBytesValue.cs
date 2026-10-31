using System.Linq;

namespace Data;

public class ConstantBytesValue : IBytesValue
{
	private readonly byte[]? Value;

	public ConstantBytesValue(byte[]? value)
	{
		Value = value;
		base._002Ector();
	}

	public byte[]? Get(Functions f)
	{
		return Value;
	}

	public bool? Match(Functions f, byte[]? value)
	{
		if (Value == null && value == null)
		{
			return true;
		}
		if (Value == null || value == null)
		{
			return false;
		}
		return Value.SequenceEqual(value);
	}
}
