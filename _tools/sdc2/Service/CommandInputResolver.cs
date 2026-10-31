using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Service;

public static class CommandInputResolver
{
	private sealed class InputRequest
	{
		public readonly List<InputMatch> Matches;

		public readonly Action<string> OnResolved;

		public readonly CommandInput Input;

		public int Index;

		public string Command;

		public InputRequest(string command, List<InputMatch> matches, Action<string> onResolved)
		{
			Matches = matches;
			OnResolved = onResolved;
			Input = new CommandInput(HandleInput, CancelActiveRequest);
			Command = command;
			base._002Ector();
		}
	}

	private readonly struct InputMatch
	{
		public readonly string Token;

		public readonly string Topic;

		public InputMatch(string token, string topic)
		{
			Token = token;
			Topic = topic;
		}
	}

	private static readonly Regex InputPattern = new Regex("<input(?:_([^>]+))?>", RegexOptions.IgnoreCase | RegexOptions.Compiled);

	private static readonly Queue<InputRequest> PendingRequests = new Queue<InputRequest>();

	private static InputRequest? ActiveRequest;

	public static void CheckActiveRequest()
	{
		if (ActiveRequest != null)
		{
			ActiveRequest.Input.ShowIfHidden();
		}
	}

	public static void Clear()
	{
		PendingRequests.Clear();
		ActiveRequest = null;
	}

	public static bool TryResolve(string command, Action<string> onResolved)
	{
		List<InputMatch> list = FindMatches(command);
		if (list.Count == 0)
		{
			return false;
		}
		PendingRequests.Enqueue(new InputRequest(command, list, onResolved));
		StartNext();
		return true;
	}

	private static void StartNext()
	{
		if (ActiveRequest == null && PendingRequests.Count != 0)
		{
			ActiveRequest = PendingRequests.Dequeue();
			ContinueRequest();
		}
	}

	private static void ContinueRequest()
	{
		if (ActiveRequest != null)
		{
			InputMatch inputMatch = ActiveRequest.Matches[ActiveRequest.Index];
			ActiveRequest.Input.Ask(inputMatch.Topic);
		}
	}

	private static void HandleInput(string value)
	{
		if (ActiveRequest != null)
		{
			InputRequest activeRequest = ActiveRequest;
			InputMatch inputMatch = activeRequest.Matches[activeRequest.Index];
			activeRequest.Command = ReplaceFirst(activeRequest.Command, inputMatch.Token, value);
			activeRequest.Index++;
			if (activeRequest.Index >= activeRequest.Matches.Count)
			{
				CancelActiveRequest();
				activeRequest.OnResolved(activeRequest.Command);
			}
			else
			{
				ContinueRequest();
			}
		}
	}

	private static void CancelActiveRequest()
	{
		ActiveRequest = null;
		StartNext();
	}

	private static List<InputMatch> FindMatches(string command)
	{
		List<InputMatch> list = new List<InputMatch>();
		foreach (Match item in InputPattern.Matches(command))
		{
			if (item.Success)
			{
				string text = ((item.Groups[1].Success && !string.IsNullOrWhiteSpace(item.Groups[1].Value)) ? item.Groups[1].Value : "text");
				list.Add(new InputMatch(item.Value, text.Replace("_", " ")));
			}
		}
		return list;
	}

	private static string ReplaceFirst(string source, string token, string replacement)
	{
		int num = source.IndexOf(token, StringComparison.Ordinal);
		if (num < 0)
		{
			return source;
		}
		return source.Substring(0, num) + replacement + source.Substring(num + token.Length);
	}
}
