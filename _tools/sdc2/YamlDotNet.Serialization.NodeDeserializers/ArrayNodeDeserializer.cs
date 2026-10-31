using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Serialization.Utilities;

namespace YamlDotNet.Serialization.NodeDeserializers;

internal sealed class ArrayNodeDeserializer : INodeDeserializer
{
	private sealed class ArrayList : IList, ICollection, IEnumerable
	{
		[CompilerGenerated]
		private sealed class _003CGetEnumerator_003Ed__25 : IEnumerator<object>, IDisposable, IEnumerator
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public ArrayList _003C_003E4__this;

			private int _003Ci_003E5__2;

			object IEnumerator<object>.Current
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
			public _003CGetEnumerator_003Ed__25(int _003C_003E1__state)
			{
				this._003C_003E1__state = _003C_003E1__state;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				_003C_003E1__state = -2;
			}

			private bool MoveNext()
			{
				int num = _003C_003E1__state;
				ArrayList arrayList = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003Ci_003E5__2 = 0;
					break;
				case 1:
				{
					_003C_003E1__state = -1;
					int num2 = _003Ci_003E5__2 + 1;
					_003Ci_003E5__2 = num2;
					break;
				}
				}
				if (_003Ci_003E5__2 < arrayList.Count)
				{
					_003C_003E2__current = arrayList.data[_003Ci_003E5__2];
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
		}

		private object?[] data;

		public bool IsFixedSize => false;

		public bool IsReadOnly => false;

		public object? this[int index]
		{
			get
			{
				return data[index];
			}
			set
			{
				data[index] = value;
			}
		}

		public int Count { get; private set; }

		public bool IsSynchronized => false;

		public object SyncRoot => data;

		public ArrayList()
		{
			Clear();
		}

		public int Add(object? value)
		{
			if (Count == data.Length)
			{
				Array.Resize(ref data, data.Length * 2);
			}
			data[Count] = value;
			return Count++;
		}

		public void Clear()
		{
			data = new object[10];
			Count = 0;
		}

		bool IList.Contains(object? value)
		{
			throw new NotSupportedException();
		}

		int IList.IndexOf(object? value)
		{
			throw new NotSupportedException();
		}

		void IList.Insert(int index, object? value)
		{
			throw new NotSupportedException();
		}

		void IList.Remove(object? value)
		{
			throw new NotSupportedException();
		}

		void IList.RemoveAt(int index)
		{
			throw new NotSupportedException();
		}

		public void CopyTo(Array array, int index)
		{
			Array.Copy(data, 0, array, index, Count);
		}

		[IteratorStateMachine(typeof(_003CGetEnumerator_003Ed__25))]
		public IEnumerator GetEnumerator()
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003CGetEnumerator_003Ed__25(0)
			{
				_003C_003E4__this = this
			};
		}
	}

	private readonly INamingConvention enumNamingConvention;

	private readonly ITypeInspector typeInspector;

	public ArrayNodeDeserializer(INamingConvention enumNamingConvention, ITypeInspector typeInspector)
	{
		this.enumNamingConvention = enumNamingConvention;
		this.typeInspector = typeInspector;
	}

	public bool Deserialize(IParser parser, Type expectedType, Func<IParser, Type, object?> nestedObjectDeserializer, out object? value, ObjectDeserializer rootDeserializer)
	{
		if (!expectedType.IsArray)
		{
			value = false;
			return false;
		}
		Type itemType = expectedType.GetElementType();
		ArrayList arrayList = new ArrayList();
		Array array = null;
		CollectionNodeDeserializer.DeserializeHelper(itemType, parser, nestedObjectDeserializer, arrayList, canUpdate: true, enumNamingConvention, typeInspector, PromiseResolvedHandler);
		array = Array.CreateInstance(itemType, arrayList.Count);
		arrayList.CopyTo(array, 0);
		value = array;
		return true;
		void PromiseResolvedHandler(int index, object? value)
		{
			if (array == null)
			{
				throw new InvalidOperationException("Destination array is still null");
			}
			array.SetValue(TypeConverter.ChangeType(value, itemType, enumNamingConvention, typeInspector), index);
		}
	}
}
