using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.Schemas;

namespace YamlDotNet.RepresentationModel;

[DebuggerDisplay("{Value}")]
internal sealed class YamlScalarNode : YamlNode, IYamlConvertible
{
	[CompilerGenerated]
	private sealed class _003CSafeAllNodes_003Ed__20 : IEnumerable<YamlNode>, IEnumerable, IEnumerator<YamlNode>, IDisposable, IEnumerator
	{
		private int _003C_003E1__state;

		private YamlNode _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public YamlScalarNode _003C_003E4__this;

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
		public _003CSafeAllNodes_003Ed__20(int _003C_003E1__state)
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
			int num = _003C_003E1__state;
			YamlScalarNode yamlScalarNode = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = yamlScalarNode;
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				return false;
			}
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
		IEnumerator<YamlNode> IEnumerable<YamlNode>.GetEnumerator()
		{
			_003CSafeAllNodes_003Ed__20 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CSafeAllNodes_003Ed__20(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<YamlNode>)this).GetEnumerator();
		}
	}

	private bool forceImplicitPlain;

	private string? value;

	public string? Value
	{
		get
		{
			return value;
		}
		set
		{
			if (value == null)
			{
				forceImplicitPlain = true;
			}
			else
			{
				forceImplicitPlain = false;
			}
			this.value = value;
		}
	}

	public ScalarStyle Style { get; set; }

	public override YamlNodeType NodeType => YamlNodeType.Scalar;

	internal YamlScalarNode(IParser parser, DocumentLoadingState state)
	{
		Load(parser, state);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Load(IParser parser, DocumentLoadingState state)
	{
		Scalar scalar = parser.Consume<Scalar>();
		Load(scalar, state);
		string text = scalar.Value;
		if (scalar.Style == ScalarStyle.Plain && base.Tag.IsEmpty)
		{
			forceImplicitPlain = text.Length switch
			{
				0 => true, 
				1 => text == "~", 
				4 => text == "null" || text == "Null" || text == "NULL", 
				_ => false, 
			};
		}
		value = text;
		Style = scalar.Style;
	}

	public YamlScalarNode()
	{
	}

	public YamlScalarNode(string? value)
	{
		Value = value;
	}

	internal override void ResolveAliases(DocumentLoadingState state)
	{
		throw new NotSupportedException("Resolving an alias on a scalar node does not make sense");
	}

	internal override void Emit(IEmitter emitter, EmitterState state)
	{
		TagName tag = base.Tag;
		bool isPlainImplicit = tag.IsEmpty;
		if (forceImplicitPlain && Style == ScalarStyle.Plain && (Value == null || Value == ""))
		{
			tag = JsonSchema.Tags.Null;
			isPlainImplicit = true;
		}
		else if (tag.IsEmpty && Value == null && (Style == ScalarStyle.Plain || Style == ScalarStyle.Any))
		{
			tag = JsonSchema.Tags.Null;
			isPlainImplicit = true;
		}
		emitter.Emit(new Scalar(base.Anchor, tag, Value ?? string.Empty, Style, isPlainImplicit, isQuotedImplicit: false));
	}

	public override void Accept(IYamlVisitor visitor)
	{
		visitor.Visit(this);
	}

	public override bool Equals(object? obj)
	{
		if (obj is YamlScalarNode yamlScalarNode && object.Equals(base.Tag, yamlScalarNode.Tag))
		{
			return object.Equals(Value, yamlScalarNode.Value);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return HashCode.CombineHashCodes(base.Tag.GetHashCode(), Value);
	}

	public static explicit operator string?(YamlScalarNode value)
	{
		return value.Value;
	}

	internal override string ToString(RecursionLevel level)
	{
		return Value ?? string.Empty;
	}

	[IteratorStateMachine(typeof(_003CSafeAllNodes_003Ed__20))]
	internal override IEnumerable<YamlNode> SafeAllNodes(RecursionLevel level)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSafeAllNodes_003Ed__20(-2)
		{
			_003C_003E4__this = this
		};
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
