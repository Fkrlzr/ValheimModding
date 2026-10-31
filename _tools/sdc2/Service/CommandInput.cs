using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Service;

public class CommandInput : TextReceiver
{
	[CompilerGenerated]
	private Action<string> _003ConTextReceived_003EP;

	[CompilerGenerated]
	private Action _003ConCancelled_003EP;

	public string Topic;

	public CommandInput(Action<string> onTextReceived, Action onCancelled)
	{
		_003ConTextReceived_003EP = onTextReceived;
		_003ConCancelled_003EP = onCancelled;
		Topic = "text";
		base._002Ector();
	}

	public void Ask(string topic)
	{
		Topic = topic;
		Show();
	}

	public void Show()
	{
		if (Object.op_Implicit((Object)(object)TextInput.instance))
		{
			TextInput.instance.RequestText((TextReceiver)(object)this, Topic, 1000);
		}
	}

	public void ShowIfHidden()
	{
		if (Object.op_Implicit((Object)(object)TextInput.instance) && !TextInput.IsVisible())
		{
			Show();
		}
	}

	public string GetText()
	{
		return "";
	}

	public void SetText(string text)
	{
		_003ConTextReceived_003EP(text);
	}

	public void Hide()
	{
		_003ConCancelled_003EP();
	}
}
