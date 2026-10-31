using Data;

namespace WorldEditCommands;

public class SpawnData : UndoData
{
	public SpawnData(ZDO zdo)
		: base(new FakeZDO(zdo))
	{
	}

	public override void Undo()
	{
		UndoHelper.Destroy(Current);
	}

	public override void Redo()
	{
		UndoHelper.Place(Current);
	}
}
