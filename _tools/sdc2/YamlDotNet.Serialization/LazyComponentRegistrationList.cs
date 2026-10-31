using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization;

internal sealed class LazyComponentRegistrationList<TArgument, TComponent> : IEnumerable<Func<TArgument, TComponent>>, IEnumerable
{
	public sealed class LazyComponentRegistration
	{
		public readonly Type ComponentType;

		public readonly Func<TArgument, TComponent> Factory;

		public LazyComponentRegistration(Type componentType, Func<TArgument, TComponent> factory)
		{
			ComponentType = componentType;
			Factory = factory;
		}
	}

	public sealed class TrackingLazyComponentRegistration
	{
		public readonly Type ComponentType;

		public readonly Func<TComponent, TArgument, TComponent> Factory;

		public TrackingLazyComponentRegistration(Type componentType, Func<TComponent, TArgument, TComponent> factory)
		{
			ComponentType = componentType;
			Factory = factory;
		}
	}

	private class RegistrationLocationSelector : IRegistrationLocationSelectionSyntax<TComponent>
	{
		private readonly LazyComponentRegistrationList<TArgument, TComponent> registrations;

		private readonly LazyComponentRegistration newRegistration;

		public RegistrationLocationSelector(LazyComponentRegistrationList<TArgument, TComponent> registrations, LazyComponentRegistration newRegistration)
		{
			this.registrations = registrations;
			this.newRegistration = newRegistration;
		}

		void IRegistrationLocationSelectionSyntax<TComponent>.InsteadOf<TRegistrationType>()
		{
			if (newRegistration.ComponentType != typeof(TRegistrationType))
			{
				registrations.EnsureNoDuplicateRegistrationType(newRegistration.ComponentType);
			}
			int index = registrations.EnsureRegistrationExists<TRegistrationType>();
			registrations.entries[index] = newRegistration;
		}

		void IRegistrationLocationSelectionSyntax<TComponent>.After<TRegistrationType>()
		{
			registrations.EnsureNoDuplicateRegistrationType(newRegistration.ComponentType);
			int num = registrations.EnsureRegistrationExists<TRegistrationType>();
			registrations.entries.Insert(num + 1, newRegistration);
		}

		void IRegistrationLocationSelectionSyntax<TComponent>.Before<TRegistrationType>()
		{
			registrations.EnsureNoDuplicateRegistrationType(newRegistration.ComponentType);
			int index = registrations.EnsureRegistrationExists<TRegistrationType>();
			registrations.entries.Insert(index, newRegistration);
		}

		void IRegistrationLocationSelectionSyntax<TComponent>.OnBottom()
		{
			registrations.EnsureNoDuplicateRegistrationType(newRegistration.ComponentType);
			registrations.entries.Add(newRegistration);
		}

		void IRegistrationLocationSelectionSyntax<TComponent>.OnTop()
		{
			registrations.EnsureNoDuplicateRegistrationType(newRegistration.ComponentType);
			registrations.entries.Insert(0, newRegistration);
		}
	}

	private class TrackingRegistrationLocationSelector : ITrackingRegistrationLocationSelectionSyntax<TComponent>
	{
		private readonly LazyComponentRegistrationList<TArgument, TComponent> registrations;

		private readonly TrackingLazyComponentRegistration newRegistration;

		public TrackingRegistrationLocationSelector(LazyComponentRegistrationList<TArgument, TComponent> registrations, TrackingLazyComponentRegistration newRegistration)
		{
			this.registrations = registrations;
			this.newRegistration = newRegistration;
		}

		void ITrackingRegistrationLocationSelectionSyntax<TComponent>.InsteadOf<TRegistrationType>()
		{
			if (newRegistration.ComponentType != typeof(TRegistrationType))
			{
				registrations.EnsureNoDuplicateRegistrationType(newRegistration.ComponentType);
			}
			int index = registrations.EnsureRegistrationExists<TRegistrationType>();
			Func<TArgument, TComponent> innerComponentFactory = registrations.entries[index].Factory;
			registrations.entries[index] = new LazyComponentRegistration(newRegistration.ComponentType, (TArgument arg) => newRegistration.Factory(innerComponentFactory(arg), arg));
		}
	}

	[CompilerGenerated]
	private sealed class _003Cget_InReverseOrder_003Ed__10 : IEnumerable<Func<TArgument, TComponent>>, IEnumerable, IEnumerator<Func<TArgument, TComponent>>, IDisposable, IEnumerator
	{
		private int _003C_003E1__state;

		private Func<TArgument, TComponent> _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public LazyComponentRegistrationList<TArgument, TComponent> _003C_003E4__this;

		private int _003Ci_003E5__2;

		Func<TArgument, TComponent> IEnumerator<Func<TArgument, TComponent>>.Current
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
		public _003Cget_InReverseOrder_003Ed__10(int _003C_003E1__state)
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
			LazyComponentRegistrationList<TArgument, TComponent> lazyComponentRegistrationList = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003Ci_003E5__2 = lazyComponentRegistrationList.entries.Count - 1;
				break;
			case 1:
			{
				_003C_003E1__state = -1;
				int num2 = _003Ci_003E5__2 - 1;
				_003Ci_003E5__2 = num2;
				break;
			}
			}
			if (_003Ci_003E5__2 >= 0)
			{
				_003C_003E2__current = lazyComponentRegistrationList.entries[_003Ci_003E5__2].Factory;
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
		IEnumerator<Func<TArgument, TComponent>> IEnumerable<Func<TArgument, TComponent>>.GetEnumerator()
		{
			_003Cget_InReverseOrder_003Ed__10 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_InReverseOrder_003Ed__10(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Func<TArgument, TComponent>>)this).GetEnumerator();
		}
	}

	private readonly List<LazyComponentRegistration> entries = new List<LazyComponentRegistration>();

	public int Count => entries.Count;

	public IEnumerable<Func<TArgument, TComponent>> InReverseOrder
	{
		[IteratorStateMachine(typeof(LazyComponentRegistrationList<, >._003Cget_InReverseOrder_003Ed__10))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			_003Cget_InReverseOrder_003Ed__10 _003Cget_InReverseOrder_003Ed__ = new _003Cget_InReverseOrder_003Ed__10(-2);
			_003Cget_InReverseOrder_003Ed__._003C_003E4__this = this;
			return _003Cget_InReverseOrder_003Ed__;
		}
	}

	public LazyComponentRegistrationList<TArgument, TComponent> Clone()
	{
		LazyComponentRegistrationList<TArgument, TComponent> lazyComponentRegistrationList = new LazyComponentRegistrationList<TArgument, TComponent>();
		foreach (LazyComponentRegistration entry in entries)
		{
			lazyComponentRegistrationList.entries.Add(entry);
		}
		return lazyComponentRegistrationList;
	}

	public void Clear()
	{
		entries.Clear();
	}

	public void Add(Type componentType, Func<TArgument, TComponent> factory)
	{
		entries.Add(new LazyComponentRegistration(componentType, factory));
	}

	public void Remove(Type componentType)
	{
		for (int i = 0; i < entries.Count; i++)
		{
			if (entries[i].ComponentType == componentType)
			{
				entries.RemoveAt(i);
				return;
			}
		}
		throw new KeyNotFoundException("A component registration of type '" + componentType.FullName + "' was not found.");
	}

	public IRegistrationLocationSelectionSyntax<TComponent> CreateRegistrationLocationSelector(Type componentType, Func<TArgument, TComponent> factory)
	{
		return new RegistrationLocationSelector(this, new LazyComponentRegistration(componentType, factory));
	}

	public ITrackingRegistrationLocationSelectionSyntax<TComponent> CreateTrackingRegistrationLocationSelector(Type componentType, Func<TComponent, TArgument, TComponent> factory)
	{
		return new TrackingRegistrationLocationSelector(this, new TrackingLazyComponentRegistration(componentType, factory));
	}

	public IEnumerator<Func<TArgument, TComponent>> GetEnumerator()
	{
		return entries.Select((LazyComponentRegistration e) => e.Factory).GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	private int IndexOfRegistration(Type registrationType)
	{
		for (int i = 0; i < entries.Count; i++)
		{
			if (registrationType == entries[i].ComponentType)
			{
				return i;
			}
		}
		return -1;
	}

	private void EnsureNoDuplicateRegistrationType(Type componentType)
	{
		if (IndexOfRegistration(componentType) != -1)
		{
			throw new InvalidOperationException("A component of type '" + componentType.FullName + "' has already been registered.");
		}
	}

	private int EnsureRegistrationExists<TRegistrationType>()
	{
		int num = IndexOfRegistration(typeof(TRegistrationType));
		if (num == -1)
		{
			throw new InvalidOperationException("A component of type '" + typeof(TRegistrationType).FullName + "' has not been registered.");
		}
		return num;
	}
}
