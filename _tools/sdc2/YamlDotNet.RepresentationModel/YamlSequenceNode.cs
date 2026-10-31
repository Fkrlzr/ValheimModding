using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Core.ObjectPool;
using YamlDotNet.Serialization;

namespace YamlDotNet.RepresentationModel;

[DebuggerDisplay("Count = {children.Count}")]
internal sealed class YamlSequenceNode : YamlNode, IEnumerable<YamlNode>, IEnumerable, IYamlConvertible
{
	[CompilerGenerated]
	private sealed class _003CSafeAllNodes_003Ed__19 : IEnumerable<YamlNode>, IEnumerable, IEnumerator<YamlNode>, IDisposable, IEnumerator
	{
		private int _003C_003E1__state;

		private YamlNode _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private RecursionLevel level;

		public RecursionLevel _003C_003E3__level;

		public YamlSequenceNode _003C_003E4__this;

		private List<YamlNode>.Enumerator _003C_003E7__wrap1;

		private IEnumerator<YamlNode> _003C_003E7__wrap2;

		YamlNode IEnumerator<YamlNode>.Current
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
		public _003CSafeAllNodes_003Ed__19(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if ((uint)(num - -4) <= 1u || num == 2)
			{
				try
				{
					if (num == -4 || num == 2)
					{
						try
						{
						}
						finally
						{
							_003C_003Em__Finally2();
						}
					}
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = default(List<YamlNode>.Enumerator);
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				YamlSequenceNode yamlSequenceNode = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
				{
					_003C_003E1__state = -1;
					RecursionLevel recursionLevel = level;
					Mark start = yamlSequenceNode.Start;
					Mark end = yamlSequenceNode.End;
					recursionLevel.Increment(in start, in end);
					_003C_003E2__current = yamlSequenceNode;
					_003C_003E1__state = 1;
					return true;
				}
				case 1:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = yamlSequenceNode.children.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_00f4;
				case 2:
					{
						_003C_003E1__state = -4;
						goto IL_00da;
					}
					IL_00f4:
					if (_003C_003E7__wrap1.MoveNext())
					{
						YamlNode current = _003C_003E7__wrap1.Current;
						_003C_003E7__wrap2 = current.SafeAllNodes(level).GetEnumerator();
						_003C_003E1__state = -4;
						goto IL_00da;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = default(List<YamlNode>.Enumerator);
					level.Decrement();
					return false;
					IL_00da:
					if (_003C_003E7__wrap2.MoveNext())
					{
						YamlNode current2 = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = null;
					goto IL_00f4;
				}
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap1).Dispose();
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -3;
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<YamlNode> IEnumerable<YamlNode>.GetEnumerator()
		{
			_003CSafeAllNodes_003Ed__19 _003CSafeAllNodes_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CSafeAllNodes_003Ed__ = this;
			}
			else
			{
				_003CSafeAllNodes_003Ed__ = new _003CSafeAllNodes_003Ed__19(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CSafeAllNodes_003Ed__.level = _003C_003E3__level;
			return _003CSafeAllNodes_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<YamlNode>)this).GetEnumerator();
		}
	}

	private readonly List<YamlNode> children = new List<YamlNode>();

	public IList<YamlNode> Children => children;

	public SequenceStyle Style { get; set; }

	public override YamlNodeType NodeType => YamlNodeType.Sequence;

	internal YamlSequenceNode(IParser parser, DocumentLoadingState state)
	{
		Load(parser, state);
	}

	private void Load(IParser parser, DocumentLoadingState state)
	{
		SequenceStart sequenceStart = parser.Consume<SequenceStart>();
		Load(sequenceStart, state);
		Style = sequenceStart.Style;
		bool flag = false;
		SequenceEnd @event;
		while (!parser.TryConsume<SequenceEnd>(out @event))
		{
			YamlNode yamlNode = YamlNode.ParseNode(parser, state);
			children.Add(yamlNode);
			flag = flag || yamlNode is YamlAliasNode;
		}
		if (flag)
		{
			state.AddNodeWithUnresolvedAliases(this);
		}
	}

	public YamlSequenceNode()
	{
	}

	public YamlSequenceNode(params YamlNode[] children)
		: this((IEnumerable<YamlNode>)children)
	{
	}

	public YamlSequenceNode(IEnumerable<YamlNode> children)
	{
		foreach (YamlNode child in children)
		{
			this.children.Add(child);
		}
	}

	public void Add(YamlNode child)
	{
		children.Add(child);
	}

	public void Add(string child)
	{
		children.Add(new YamlScalarNode(child));
	}

	internal override void ResolveAliases(DocumentLoadingState state)
	{
		for (int i = 0; i < children.Count; i++)
		{
			if (children[i] is YamlAliasNode)
			{
				children[i] = state.GetNode(children[i].Anchor, children[i].Start, children[i].End);
			}
		}
	}

	internal override void Emit(IEmitter emitter, EmitterState state)
	{
		emitter.Emit(new SequenceStart(base.Anchor, base.Tag, base.Tag.IsEmpty, Style));
		foreach (YamlNode child in children)
		{
			child.Save(emitter, state);
		}
		emitter.Emit(new SequenceEnd());
	}

	public override void Accept(IYamlVisitor visitor)
	{
		visitor.Visit(this);
	}

	public override bool Equals(object? obj)
	{
		if (!(obj is YamlSequenceNode yamlSequenceNode) || !object.Equals(base.Tag, yamlSequenceNode.Tag) || children.Count != yamlSequenceNode.children.Count)
		{
			return false;
		}
		for (int i = 0; i < children.Count; i++)
		{
			if (!object.Equals(children[i], yamlSequenceNode.children[i]))
			{
				return false;
			}
		}
		return true;
	}

	public override int GetHashCode()
	{
		int h = 0;
		foreach (YamlNode child in children)
		{
			h = HashCode.CombineHashCodes(h, child);
		}
		return HashCode.CombineHashCodes(h, base.Tag);
	}

	[IteratorStateMachine(typeof(_003CSafeAllNodes_003Ed__19))]
	internal override IEnumerable<YamlNode> SafeAllNodes(RecursionLevel level)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSafeAllNodes_003Ed__19(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__level = level
		};
	}

	internal override string ToString(RecursionLevel level)
	{
		if (!level.TryIncrement())
		{
			return "WARNING! INFINITE RECURSION!";
		}
		StringBuilderPool.BuilderWrapper builderWrapper = StringBuilderPool.Rent();
		try
		{
			StringBuilder builder = builderWrapper.Builder;
			builder.Append("[ ");
			foreach (YamlNode child in children)
			{
				if (builder.Length > 2)
				{
					builder.Append(", ");
				}
				builder.Append(child.ToString(level));
			}
			builder.Append(" ]");
			level.Decrement();
			return builder.ToString();
		}
		finally
		{
			((IDisposable)builderWrapper).Dispose();
		}
	}

	public IEnumerator<YamlNode> GetEnumerator()
	{
		return Children.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	void IYamlConvertible.Read(IParser parser, Type expectedType, ObjectDeserializer nestedObjectDeserializer)
	{
		Load(parser, new DocumentLoadingState());
	}

	void IYamlConvertible.Write(IEmitter emitter, ObjectSerializer nestedObjectSerializer)
	{
		Emit(emitter, new EmitterState());
	}
}
