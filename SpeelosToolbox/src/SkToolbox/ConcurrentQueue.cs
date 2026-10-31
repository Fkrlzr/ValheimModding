using System.Collections.Generic;

namespace SkToolbox;

internal class ConcurrentQueue<T>
{
	private readonly Queue<T> queue = new Queue<T>();

	private readonly object queueLock = new object();

	public void Enqueue(T item)
	{
		lock (queueLock)
		{
			queue.Enqueue(item);
		}
	}

	public bool TryDequeue(out T result)
	{
		lock (queueLock)
		{
			if (queue.Count == 0)
			{
				result = default(T);
				return false;
			}
			result = queue.Dequeue();
			return true;
		}
	}
}
