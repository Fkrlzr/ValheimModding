namespace YamlDotNet.Core.Tokens;

public abstract class Token
{
	public Mark Start { get; }

	public Mark End { get; }

	protected Token(in Mark start, in Mark end)
	{
		Start = start;
		End = end;
	}
}
