using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Core;

internal sealed class MergingParser : IParser
{
	private sealed class ParsingEventCollection : IEnumerable<LinkedListNode<ParsingEvent>>, IEnumerable
	{
		[CompilerGenerated]
		private sealed class _003CEnumerate_003Ed__14 : IEnumerable<LinkedListNode<ParsingEvent>>, IEnumerable, IEnumerator<LinkedListNode<ParsingEvent>>, IDisposable, IEnumerator
		{
			private int _003C_003E1__state;

			private LinkedListNode<ParsingEvent> _003C_003E2__current;

			private int _003C_003El__initialThreadId;

			private LinkedListNode<ParsingEvent> node;

			public LinkedListNode<ParsingEvent> _003C_003E3__node;

			LinkedListNode<ParsingEvent> IEnumerator<LinkedListNode<ParsingEvent>>.Current
			{
				[DebuggerHidden]
				get
				{
					return _003C_003E2__current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return _003C_003E2__current;
				}
			}

			[DebuggerHidden]
			public _003CEnumerate_003Ed__14(int _003C_003E1__state)
			{
				this._003C_003E1__state = _003C_003E1__state;
				_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				_003C_003E1__state = -2;
			}

			private bool MoveNext()
			{
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					break;
				case 1:
					_003C_003E1__state = -1;
					node = node.Next;
					break;
				}
				if (node != null)
				{
					_003C_003E2__current = node;
					_003C_003E1__state = 1;
					return true;
				}
				return false;
			}

			bool IEnumerator.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				return this.MoveNext();
			}

			[DebuggerHidden]
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator<LinkedListNode<ParsingEvent>> IEnumerable<LinkedListNode<ParsingEvent>>.GetEnumerator()
			{
				_003CEnumerate_003Ed__14 _003CEnumerate_003Ed__;
				if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
				{
					_003C_003E1__state = 0;
					_003CEnumerate_003Ed__ = this;
				}
				else
				{
					_003CEnumerate_003Ed__ = new _003CEnumerate_003Ed__14(0);
				}
				_003CEnumerate_003Ed__.node = _003C_003E3__node;
				return _003CEnumerate_003Ed__;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<LinkedListNode<ParsingEvent>>)this).GetEnumerator();
			}
		}

		private readonly LinkedList<ParsingEvent> events;

		private readonly HashSet<LinkedListNode<ParsingEvent>> deleted;

		private readonly Dictionary<AnchorName, LinkedListNode<ParsingEvent>> references;

		public int Count => events.Count;

		public ParsingEventCollection()
		{
			events = new LinkedList<ParsingEvent>();
			deleted = new HashSet<LinkedListNode<ParsingEvent>>();
			references = new Dictionary<AnchorName, LinkedListNode<ParsingEvent>>();
		}

		public void AddAfter(LinkedListNode<ParsingEvent> node, IEnumerable<ParsingEvent> items, Action<ParsingEvent> onItemAdded)
		{
			foreach (ParsingEvent item in items)
			{
				node = events.AddAfter(node, item);
				onItemAdded(item);
			}
		}

		public void Add(ParsingEvent item)
		{
			LinkedListNode<ParsingEvent> node = events.AddLast(item);
			AddReference(item, node);
		}

		public void MarkDeleted(LinkedListNode<ParsingEvent> node)
		{
			deleted.Add(node);
		}

		public bool IsDeleted(LinkedListNode<ParsingEvent> node)
		{
			return deleted.Contains(node);
		}

		public void CleanMarked()
		{
			foreach (LinkedListNode<ParsingEvent> item in deleted)
			{
				events.Remove(item);
			}
		}

		public IEnumerable<LinkedListNode<ParsingEvent>> FromAnchor(AnchorName anchor)
		{
			LinkedListNode<ParsingEvent> next = references[anchor].Next;
			return Enumerate(next);
		}

		public IEnumerator<LinkedListNode<ParsingEvent>> GetEnumerator()
		{
			return Enumerate(events.First).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		[IteratorStateMachine(typeof(_003CEnumerate_003Ed__14))]
		private static IEnumerable<LinkedListNode<ParsingEvent>> Enumerate(LinkedListNode<ParsingEvent>? node)
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003CEnumerate_003Ed__14(-2)
			{
				_003C_003E3__node = node
			};
		}

		private void AddReference(ParsingEvent item, LinkedListNode<ParsingEvent> node)
		{
			if (item is MappingStart { Anchor: { IsEmpty: false } anchor })
			{
				references[anchor] = node;
			}
		}
	}

	private sealed class ParsingEventCloner : IParsingEventVisitor
	{
		private ParsingEvent? clonedEvent;

		public ParsingEvent Clone(ParsingEvent e)
		{
			e.Accept(this);
			if (clonedEvent == null)
			{
				throw new InvalidOperationException($"Could not clone event of type '{e.Type}'");
			}
			return clonedEvent;
		}

		void IParsingEventVisitor.Visit(AnchorAlias e)
		{
			clonedEvent = new AnchorAlias(e.Value, e.Start, e.End);
		}

		void IParsingEventVisitor.Visit(StreamStart e)
		{
			throw new NotSupportedException();
		}

		void IParsingEventVisitor.Visit(StreamEnd e)
		{
			throw new NotSupportedException();
		}

		void IParsingEventVisitor.Visit(DocumentStart e)
		{
			throw new NotSupportedException();
		}

		void IParsingEventVisitor.Visit(DocumentEnd e)
		{
			throw new NotSupportedException();
		}

		void IParsingEventVisitor.Visit(Scalar e)
		{
			clonedEvent = new Scalar(AnchorName.Empty, e.Tag, e.Value, e.Style, e.IsPlainImplicit, e.IsQuotedImplicit, e.Start, e.End);
		}

		void IParsingEventVisitor.Visit(SequenceStart e)
		{
			clonedEvent = new SequenceStart(AnchorName.Empty, e.Tag, e.IsImplicit, e.Style, e.Start, e.End);
		}

		void IParsingEventVisitor.Visit(SequenceEnd e)
		{
			Mark start = e.Start;
			Mark end = e.End;
			clonedEvent = new SequenceEnd(in start, in end);
		}

		void IParsingEventVisitor.Visit(MappingStart e)
		{
			clonedEvent = new MappingStart(AnchorName.Empty, e.Tag, e.IsImplicit, e.Style, e.Start, e.End);
		}

		void IParsingEventVisitor.Visit(MappingEnd e)
		{
			Mark start = e.Start;
			Mark end = e.End;
			clonedEvent = new MappingEnd(in start, in end);
		}

		void IParsingEventVisitor.Visit(Comment e)
		{
			throw new NotSupportedException();
		}
	}

	private readonly ParsingEventCollection events;

	private readonly IParser innerParser;

	private IEnumerator<LinkedListNode<ParsingEvent>> iterator;

	private bool merged;

	private readonly int maxParsingEvents;

	public ParsingEvent? Current => iterator.Current?.Value;

	public MergingParser(IParser innerParser)
		: this(innerParser, 100000)
	{
	}

	public MergingParser(IParser innerParser, int maxParsingEvents = 100000)
	{
		if (maxParsingEvents <= 0)
		{
			throw new ArgumentOutOfRangeException("maxParsingEvents", "Max parsing events must be a positive integer.");
		}
		events = new ParsingEventCollection();
		merged = false;
		iterator = events.GetEnumerator();
		this.innerParser = innerParser;
		this.maxParsingEvents = maxParsingEvents;
	}

	public bool MoveNext()
	{
		if (!merged)
		{
			Merge();
			events.CleanMarked();
			iterator = events.GetEnumerator();
			merged = true;
		}
		return iterator.MoveNext();
	}

	private void Merge()
	{
		while (innerParser.MoveNext())
		{
			ParsingEvent current = innerParser.Current;
			events.Add(current);
			EnsureMaxParsingEventsNotExceeded(current);
		}
		foreach (LinkedListNode<ParsingEvent> @event in events)
		{
			if (IsMergeToken(@event))
			{
				events.MarkDeleted(@event);
				if (!HandleMerge(@event.Next))
				{
					Mark start = @event.Value.Start;
					Mark end = @event.Value.End;
					throw new SemanticErrorException(in start, in end, "Unrecognized merge key pattern");
				}
			}
		}
	}

	private bool HandleMerge(LinkedListNode<ParsingEvent>? node)
	{
		if (node == null)
		{
			return false;
		}
		if (node.Value is AnchorAlias anchorAlias)
		{
			return HandleAnchorAlias(node, node, anchorAlias);
		}
		if (node.Value is SequenceStart)
		{
			return HandleSequence(node);
		}
		return false;
	}

	private bool HandleMergeSequence(LinkedListNode<ParsingEvent> sequenceStart, LinkedListNode<ParsingEvent>? node)
	{
		if (node == null)
		{
			return false;
		}
		if (node.Value is AnchorAlias anchorAlias)
		{
			return HandleAnchorAlias(sequenceStart, node, anchorAlias);
		}
		if (node.Value is SequenceStart)
		{
			return HandleSequence(node);
		}
		return false;
	}

	private static bool IsMergeToken(LinkedListNode<ParsingEvent> node)
	{
		if (node.Value is Scalar scalar)
		{
			return scalar.Value == "<<";
		}
		return false;
	}

	private bool HandleAnchorAlias(LinkedListNode<ParsingEvent> node, LinkedListNode<ParsingEvent> anchorNode, AnchorAlias anchorAlias)
	{
		IEnumerable<ParsingEvent> mappingEvents = GetMappingEvents(anchorAlias.Value);
		events.AddAfter(node, mappingEvents, EnsureMaxParsingEventsNotExceeded);
		events.MarkDeleted(anchorNode);
		return true;
	}

	private bool HandleSequence(LinkedListNode<ParsingEvent> node)
	{
		events.MarkDeleted(node);
		LinkedListNode<ParsingEvent> linkedListNode = node;
		while (linkedListNode != null)
		{
			if (linkedListNode.Value is SequenceEnd)
			{
				events.MarkDeleted(linkedListNode);
				return true;
			}
			LinkedListNode<ParsingEvent> next = linkedListNode.Next;
			HandleMergeSequence(node, next);
			linkedListNode = next;
		}
		return true;
	}

	private IEnumerable<ParsingEvent> GetMappingEvents(AnchorName anchor)
	{
		ParsingEventCloner @object = new ParsingEventCloner();
		int nesting = 0;
		return (from e in events.FromAnchor(anchor)
			where !events.IsDeleted(e)
			select e.Value).TakeWhile((ParsingEvent e) => (nesting += e.NestingIncrease) >= 0).Select(@object.Clone);
	}

	private void EnsureMaxParsingEventsNotExceeded(ParsingEvent parsingEvent)
	{
		if (events.Count > maxParsingEvents)
		{
			Mark start = parsingEvent.Start;
			Mark end = parsingEvent.End;
			throw new YamlException(in start, in end, $"Too many parsing events. The configured limit of {maxParsingEvents} was exceeded to prevent excessive memory usage.");
		}
	}
}
