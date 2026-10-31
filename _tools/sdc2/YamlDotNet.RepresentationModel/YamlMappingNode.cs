using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Core.ObjectPool;
using YamlDotNet.Helpers;
using YamlDotNet.Serialization;

namespace YamlDotNet.RepresentationModel;

internal sealed class YamlMappingNode : YamlNode, IEnumerable<KeyValuePair<YamlNode, YamlNode>>, IEnumerable, IYamlConvertible
{
	[CompilerGenerated]
	private sealed class _003CSafeAllNodes_003Ed__23 : IEnumerable<YamlNode>, IEnumerable, IEnumerator<YamlNode>, IDisposable, IEnumerator
	{
		private int _003C_003E1__state;

		private YamlNode _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private RecursionLevel level;

		public RecursionLevel _003C_003E3__level;

		public YamlMappingNode _003C_003E4__this;

		private IEnumerator<KeyValuePair<YamlNode, YamlNode>> _003C_003E7__wrap1;

		private KeyValuePair<YamlNode, YamlNode> _003Cchild_003E5__3;

		private IEnumerator<YamlNode> _003C_003E7__wrap3;

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
		public _003CSafeAllNodes_003Ed__23(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if ((uint)(num - -5) <= 2u || (uint)(num - 2) <= 1u)
			{
				try
				{
					switch (num)
					{
					case -4:
					case 2:
						try
						{
						}
						finally
						{
							_003C_003Em__Finally2();
						}
						break;
					case -5:
					case 3:
						try
						{
						}
						finally
						{
							_003C_003Em__Finally3();
						}
						break;
					}
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = null;
			_003Cchild_003E5__3 = default(KeyValuePair<YamlNode, YamlNode>);
			_003C_003E7__wrap3 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				YamlMappingNode yamlMappingNode = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
				{
					_003C_003E1__state = -1;
					RecursionLevel recursionLevel = level;
					Mark start = yamlMappingNode.Start;
					Mark end = yamlMappingNode.End;
					recursionLevel.Increment(in start, in end);
					_003C_003E2__current = yamlMappingNode;
					_003C_003E1__state = 1;
					return true;
				}
				case 1:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = yamlMappingNode.children.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0184;
				case 2:
					_003C_003E1__state = -4;
					goto IL_00f1;
				case 3:
					{
						_003C_003E1__state = -5;
						goto IL_015e;
					}
					IL_0184:
					if (_003C_003E7__wrap1.MoveNext())
					{
						_003Cchild_003E5__3 = _003C_003E7__wrap1.Current;
						_003C_003E7__wrap3 = _003Cchild_003E5__3.Key.SafeAllNodes(level).GetEnumerator();
						_003C_003E1__state = -4;
						goto IL_00f1;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					level.Decrement();
					return false;
					IL_015e:
					if (_003C_003E7__wrap3.MoveNext())
					{
						YamlNode current = _003C_003E7__wrap3.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 3;
						return true;
					}
					_003C_003Em__Finally3();
					_003C_003E7__wrap3 = null;
					_003Cchild_003E5__3 = default(KeyValuePair<YamlNode, YamlNode>);
					goto IL_0184;
					IL_00f1:
					if (_003C_003E7__wrap3.MoveNext())
					{
						YamlNode current2 = _003C_003E7__wrap3.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap3 = null;
					_003C_003E7__wrap3 = _003Cchild_003E5__3.Value.SafeAllNodes(level).GetEnumerator();
					_003C_003E1__state = -5;
					goto IL_015e;
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
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -3;
			if (_003C_003E7__wrap3 != null)
			{
				_003C_003E7__wrap3.Dispose();
			}
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -3;
			if (_003C_003E7__wrap3 != null)
			{
				_003C_003E7__wrap3.Dispose();
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
			_003CSafeAllNodes_003Ed__23 _003CSafeAllNodes_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CSafeAllNodes_003Ed__ = this;
			}
			else
			{
				_003CSafeAllNodes_003Ed__ = new _003CSafeAllNodes_003Ed__23(0)
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

	private readonly OrderedDictionary<YamlNode, YamlNode> children = new OrderedDictionary<YamlNode, YamlNode>();

	public IOrderedDictionary<YamlNode, YamlNode> Children => children;

	public MappingStyle Style { get; set; }

	public override YamlNodeType NodeType => YamlNodeType.Mapping;

	internal YamlMappingNode(IParser parser, DocumentLoadingState state)
	{
		Load(parser, state);
	}

	private void Load(IParser parser, DocumentLoadingState state)
	{
		MappingStart mappingStart = parser.Consume<MappingStart>();
		Load(mappingStart, state);
		Style = mappingStart.Style;
		bool flag = false;
		MappingEnd @event;
		while (!parser.TryConsume<MappingEnd>(out @event))
		{
			YamlNode yamlNode = YamlNode.ParseNode(parser, state);
			YamlNode yamlNode2 = YamlNode.ParseNode(parser, state);
			if (!children.TryAdd(yamlNode, yamlNode2))
			{
				Mark start = yamlNode.Start;
				Mark end = yamlNode.End;
				throw new YamlException(in start, in end, $"Duplicate key {yamlNode}");
			}
			flag = flag || yamlNode is YamlAliasNode || yamlNode2 is YamlAliasNode;
		}
		if (flag)
		{
			state.AddNodeWithUnresolvedAliases(this);
		}
	}

	public YamlMappingNode()
	{
	}

	public YamlMappingNode(params KeyValuePair<YamlNode, YamlNode>[] children)
		: this((IEnumerable<KeyValuePair<YamlNode, YamlNode>>)children)
	{
	}

	public YamlMappingNode(IEnumerable<KeyValuePair<YamlNode, YamlNode>> children)
	{
		foreach (KeyValuePair<YamlNode, YamlNode> child in children)
		{
			this.children.Add(child);
		}
	}

	public YamlMappingNode(params YamlNode[] children)
		: this((IEnumerable<YamlNode>)children)
	{
	}

	public YamlMappingNode(IEnumerable<YamlNode> children)
	{
		using IEnumerator<YamlNode> enumerator = children.GetEnumerator();
		while (enumerator.MoveNext())
		{
			YamlNode current = enumerator.Current;
			if (!enumerator.MoveNext())
			{
				throw new ArgumentException("When constructing a mapping node with a sequence, the number of elements of the sequence must be even.");
			}
			Add(current, enumerator.Current);
		}
	}

	public void Add(YamlNode key, YamlNode value)
	{
		children.Add(key, value);
	}

	public void Add(string key, YamlNode value)
	{
		children.Add(new YamlScalarNode(key), value);
	}

	public void Add(YamlNode key, string value)
	{
		children.Add(key, new YamlScalarNode(value));
	}

	public void Add(string key, string value)
	{
		children.Add(new YamlScalarNode(key), new YamlScalarNode(value));
	}

	internal override void ResolveAliases(DocumentLoadingState state)
	{
		Dictionary<YamlNode, YamlNode> dictionary = null;
		Dictionary<YamlNode, YamlNode> dictionary2 = null;
		foreach (KeyValuePair<YamlNode, YamlNode> child in children)
		{
			if (child.Key is YamlAliasNode)
			{
				if (dictionary == null)
				{
					dictionary = new Dictionary<YamlNode, YamlNode>();
				}
				dictionary.Add(child.Key, state.GetNode(child.Key.Anchor, child.Key.Start, child.Key.End));
			}
			if (child.Value is YamlAliasNode)
			{
				if (dictionary2 == null)
				{
					dictionary2 = new Dictionary<YamlNode, YamlNode>();
				}
				dictionary2.Add(child.Key, state.GetNode(child.Value.Anchor, child.Value.Start, child.Value.End));
			}
		}
		if (dictionary2 != null)
		{
			foreach (KeyValuePair<YamlNode, YamlNode> item in dictionary2)
			{
				children[item.Key] = item.Value;
			}
		}
		if (dictionary == null)
		{
			return;
		}
		foreach (KeyValuePair<YamlNode, YamlNode> item2 in dictionary)
		{
			YamlNode value = children[item2.Key];
			children.Remove(item2.Key);
			children.Add(item2.Value, value);
		}
	}

	internal override void Emit(IEmitter emitter, EmitterState state)
	{
		emitter.Emit(new MappingStart(base.Anchor, base.Tag, isImplicit: true, Style));
		foreach (KeyValuePair<YamlNode, YamlNode> child in children)
		{
			child.Key.Save(emitter, state);
			child.Value.Save(emitter, state);
		}
		emitter.Emit(new MappingEnd());
	}

	public override void Accept(IYamlVisitor visitor)
	{
		visitor.Visit(this);
	}

	public override bool Equals(object? obj)
	{
		if (!(obj is YamlMappingNode yamlMappingNode) || !object.Equals(base.Tag, yamlMappingNode.Tag) || children.Count != yamlMappingNode.children.Count)
		{
			return false;
		}
		foreach (KeyValuePair<YamlNode, YamlNode> child in children)
		{
			if (!yamlMappingNode.children.TryGetValue(child.Key, out YamlNode value) || !object.Equals(child.Value, value))
			{
				return false;
			}
		}
		return true;
	}

	public override int GetHashCode()
	{
		int num = base.GetHashCode();
		foreach (KeyValuePair<YamlNode, YamlNode> child in children)
		{
			num = HashCode.CombineHashCodes(num, child.Key);
			num = (child.Value.Anchor.IsEmpty ? HashCode.CombineHashCodes(num, child.Value) : HashCode.CombineHashCodes(num, child.Value.Anchor));
		}
		return num;
	}

	[IteratorStateMachine(typeof(_003CSafeAllNodes_003Ed__23))]
	internal override IEnumerable<YamlNode> SafeAllNodes(RecursionLevel level)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSafeAllNodes_003Ed__23(-2)
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
			builder.Append("{ ");
			foreach (KeyValuePair<YamlNode, YamlNode> child in children)
			{
				if (builder.Length > 2)
				{
					builder.Append(", ");
				}
				builder.Append("{ ").Append(child.Key.ToString(level)).Append(", ")
					.Append(child.Value.ToString(level))
					.Append(" }");
			}
			builder.Append(" }");
			level.Decrement();
			return builder.ToString();
		}
		finally
		{
			((IDisposable)builderWrapper).Dispose();
		}
	}

	public IEnumerator<KeyValuePair<YamlNode, YamlNode>> GetEnumerator()
	{
		return children.GetEnumerator();
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

	public static YamlMappingNode FromObject(object mapping)
	{
		if (mapping == null)
		{
			throw new ArgumentNullException("mapping");
		}
		YamlMappingNode yamlMappingNode = new YamlMappingNode();
		foreach (PropertyInfo publicProperty in mapping.GetType().GetPublicProperties())
		{
			if (publicProperty.CanRead && publicProperty.GetGetMethod(nonPublic: false).GetParameters().Length == 0)
			{
				object value = publicProperty.GetValue(mapping, null);
				YamlNode yamlNode = value as YamlNode;
				if (yamlNode == null)
				{
					string text = Convert.ToString(value, CultureInfo.InvariantCulture);
					yamlNode = text ?? string.Empty;
				}
				yamlMappingNode.Add(publicProperty.Name, yamlNode);
			}
		}
		return yamlMappingNode;
	}
}
