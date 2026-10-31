using System;
using System.Collections.Generic;
using System.Linq;

namespace YamlDotNet.Serialization.TypeInspectors;

internal class CompositeTypeInspector : TypeInspectorSkeleton
{
	private readonly IEnumerable<ITypeInspector> typeInspectors;

	public CompositeTypeInspector(params ITypeInspector[] typeInspectors)
		: this((IEnumerable<ITypeInspector>)typeInspectors)
	{
	}

	public CompositeTypeInspector(IEnumerable<ITypeInspector> typeInspectors)
	{
		this.typeInspectors = typeInspectors?.ToList() ?? throw new ArgumentNullException("typeInspectors");
	}

	public override string GetEnumName(Type enumType, string name)
	{
		foreach (ITypeInspector typeInspector in typeInspectors)
		{
			try
			{
				return typeInspector.GetEnumName(enumType, name);
			}
			catch
			{
			}
		}
		throw new ArgumentOutOfRangeException("enumType,name", "Name not found on enum type");
	}

	public override string GetEnumValue(object enumValue)
	{
		if (enumValue == null)
		{
			throw new ArgumentNullException("enumValue");
		}
		foreach (ITypeInspector typeInspector in typeInspectors)
		{
			try
			{
				return typeInspector.GetEnumValue(enumValue);
			}
			catch
			{
			}
		}
		throw new ArgumentOutOfRangeException("enumValue", $"Value not found for ({enumValue})");
	}

	public override IEnumerable<IPropertyDescriptor> GetProperties(Type type, object? container)
	{
		Type type2 = type;
		object container2 = container;
		return typeInspectors.SelectMany((ITypeInspector i) => i.GetProperties(type2, container2));
	}

	public override bool HasParseMethod(Type type)
	{
		Type type2 = type;
		return typeInspectors.Any((ITypeInspector i) => i.HasParseMethod(type2));
	}

	public override object? Parse(string value, Type expectedType)
	{
		Type expectedType2 = expectedType;
		ITypeInspector typeInspector = typeInspectors.FirstOrDefault((ITypeInspector i) => i.HasParseMethod(expectedType2));
		if (typeInspector == null)
		{
			throw new ArgumentOutOfRangeException("expectedType", "No inspector with a Parse method for type " + expectedType2.FullName + " was found.");
		}
		return typeInspector.Parse(value, expectedType2);
	}
}
