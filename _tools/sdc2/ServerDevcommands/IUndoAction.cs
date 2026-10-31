namespace ServerDevcommands;

public interface IUndoAction
{
	string Undo();

	string Redo();
}
