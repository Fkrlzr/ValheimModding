using Data;

namespace WorldEditCommands;

public class EditData : UndoData
{
	public FakeZDO Previous;

	public ZDO Original;

	public EditData(ZDO zdo)
		: base(new FakeZDO(zdo))
	{
		Original = zdo;
		Previous = Current;
	}

	public void Update()
	{
		Current = new FakeZDO(Original);
	}

	public override void Undo()
	{
		UndoHelper.Destroy(Current);
		UndoHelper.Place(Previous);
	}

	public override void Redo()
	{
		UndoHelper.Destroy(Previous);
		UndoHelper.Place(Current);
	}
}
