using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.RepresentationModel;

internal class YamlAliasNode : YamlNode
{
	[CompilerGenerated]
	private sealed class _003CSafeAllNodes_003Ed__7 : IEnumerable<YamlNode>, IEnumerable, IEnumerator<YamlNode>, IDisposable, IEnumerator
	{
		private int _003C_003E1__state;

		private YamlNode _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public YamlAliasNode _003C_003E4__this;

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
		public _003CSafeAllNodes_003Ed__7(int _003C_003E1__state)
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
			YamlAliasNode yamlAliasNode = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = yamlAliasNode;
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
			_003CSafeAllNodes_003Ed__7 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CSafeAllNodes_003Ed__7(0)
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

	public override YamlNodeType NodeType => YamlNodeType.Alias;

	internal YamlAliasNode(AnchorName anchor)
	{
		base.Anchor = anchor;
	}

	internal override void ResolveAliases(DocumentLoadingState state)
	{
		throw new NotSupportedException("Resolving an alias on an alias node does not make sense");
	}

	internal override void Emit(IEmitter emitter, EmitterState state)
	{
		throw new NotSupportedException("A YamlAliasNode is an implementation detail and should never be saved.");
	}

	public override void Accept(IYamlVisitor visitor)
	{
		throw new NotSupportedException("A YamlAliasNode is an implementation detail and should never be visited.");
	}

	public override bool Equals(object? obj)
	{
		if (obj is YamlAliasNode yamlAliasNode && Equals(yamlAliasNode))
		{
			return object.Equals(base.Anchor, yamlAliasNode.Anchor);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	internal override string ToString(RecursionLevel level)
	{
		return "*" + base.Anchor;
	}

	[IteratorStateMachine(typeof(_003CSafeAllNodes_003Ed__7))]
	internal override IEnumerable<YamlNode> SafeAllNodes(RecursionLevel level)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSafeAllNodes_003Ed__7(-2)
		{
			_003C_003E4__this = this
		};
	}
}
