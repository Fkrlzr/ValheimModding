using Data;

namespace WorldEditCommands;

public class RemoveData : UndoData
{
	public RemoveData(ZDO zdo)
		: base(new FakeZDO(zdo))
	{
	}

	public override void Undo()
	{
		UndoHelper.Place(Current);
	}

	public override void Redo()
	{
		UndoHelper.Destroy(Current);
	}
}
