using Data;

namespace WorldEditCommands;

public abstract class UndoData
{
	public FakeZDO Current;

	protected UndoData(FakeZDO zdo)
	{
		Current = zdo;
		base._002Ector();
	}

	public abstract void Undo();

	public abstract void Redo();
}
