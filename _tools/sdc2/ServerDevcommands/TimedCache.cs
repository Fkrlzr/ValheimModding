using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ServerDevcommands;

public class TimedCache
{
	[CompilerGenerated]
	private Func<List<string>?> _003Cbuild_003EP;

	private const float IdleSeconds = 10f;

	private const float MaxAgeSeconds = 30f;

	private List<string> value;

	private float lastRead;

	private float lastBuild;

	public TimedCache(Func<List<string>?> build, List<string>? initial = null)
	{
		_003Cbuild_003EP = build;
		value = initial ?? new List<string>();
		lastRead = float.NegativeInfinity;
		lastBuild = float.NegativeInfinity;
		base._002Ector();
	}

	public List<string> Get()
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		if (realtimeSinceStartup - lastRead > 10f || realtimeSinceStartup - lastBuild > 30f)
		{
			List<string> list = _003Cbuild_003EP();
			if (list == null)
			{
				return value;
			}
			value = list;
			lastBuild = realtimeSinceStartup;
		}
		lastRead = realtimeSinceStartup;
		return value;
	}
}
