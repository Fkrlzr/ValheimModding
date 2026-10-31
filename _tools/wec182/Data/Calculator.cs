using System;
using System.Globalization;
using Common;
using UnityEngine;

namespace Data;

public class Calculator
{
	private sealed class DoubleParser
	{
		private readonly string expression;

		private int index;

		public DoubleParser(string expression)
		{
			this.expression = expression;
			base._002Ector();
		}

		public double Parse()
		{
			double result = ParseExpression();
			SkipWhiteSpace();
			if (index != expression.Length)
			{
				throw new InvalidOperationException("Failed to parse expression: " + expression);
			}
			return result;
		}

		private double ParseExpression()
		{
			double num = ParseTerm();
			while (true)
			{
				SkipWhiteSpace();
				if (TryRead('+'))
				{
					num += ParseTerm();
					continue;
				}
				if (!TryRead('-'))
				{
					break;
				}
				num -= ParseTerm();
			}
			return num;
		}

		private double ParseTerm()
		{
			double num = ParseUnary();
			while (true)
			{
				SkipWhiteSpace();
				if (TryRead('*'))
				{
					num *= ParseUnary();
					continue;
				}
				if (!TryRead('/'))
				{
					break;
				}
				num /= ParseUnary();
			}
			return num;
		}

		private double ParseUnary()
		{
			SkipWhiteSpace();
			if (TryRead('+'))
			{
				return ParseUnary();
			}
			if (TryRead('-'))
			{
				return 0.0 - ParseUnary();
			}
			return ParsePower();
		}

		private double ParsePower()
		{
			double num = ParsePrimary();
			SkipWhiteSpace();
			if (TryRead('^'))
			{
				num = Math.Pow(num, ParseUnary());
			}
			return num;
		}

		private double ParsePrimary()
		{
			SkipWhiteSpace();
			if (TryRead('('))
			{
				double result = ParseExpression();
				SkipWhiteSpace();
				if (!TryRead(')'))
				{
					throw new InvalidOperationException("Failed to parse expression: " + expression);
				}
				return result;
			}
			return ParseNumber();
		}

		private double ParseNumber()
		{
			SkipWhiteSpace();
			int num = index;
			bool flag = false;
			while (index < expression.Length && char.IsDigit(expression[index]))
			{
				flag = true;
				index++;
			}
			if (index < expression.Length && expression[index] == '.')
			{
				index++;
				while (index < expression.Length && char.IsDigit(expression[index]))
				{
					flag = true;
					index++;
				}
			}
			if (index < expression.Length && (expression[index] == 'e' || expression[index] == 'E'))
			{
				int num2 = index;
				index++;
				if (index < expression.Length && (expression[index] == '+' || expression[index] == '-'))
				{
					index++;
				}
				int num3 = index;
				while (index < expression.Length && char.IsDigit(expression[index]))
				{
					index++;
				}
				if (num3 == index)
				{
					index = num2;
				}
			}
			if (!flag)
			{
				throw new InvalidOperationException("Failed to parse expression: " + expression);
			}
			return double.Parse(expression.Substring(num, index - num), NumberFormatInfo.InvariantInfo);
		}

		private bool TryRead(char c)
		{
			if (index >= expression.Length || expression[index] != c)
			{
				return false;
			}
			index++;
			return true;
		}

		private void SkipWhiteSpace()
		{
			while (index < expression.Length && char.IsWhiteSpace(expression[index]))
			{
				index++;
			}
		}
	}

	private sealed class LongParser
	{
		private readonly string expression;

		private int index;

		public LongParser(string expression)
		{
			this.expression = expression;
			base._002Ector();
		}

		public long Parse()
		{
			long result = ParseExpression();
			SkipWhiteSpace();
			if (index != expression.Length)
			{
				throw new InvalidOperationException("Failed to parse expression: " + expression);
			}
			return result;
		}

		private long ParseExpression()
		{
			long num = ParseTerm();
			while (true)
			{
				SkipWhiteSpace();
				if (TryRead('+'))
				{
					num += ParseTerm();
					continue;
				}
				if (!TryRead('-'))
				{
					break;
				}
				num -= ParseTerm();
			}
			return num;
		}

		private long ParseTerm()
		{
			long num = ParseUnary();
			while (true)
			{
				SkipWhiteSpace();
				if (TryRead('*'))
				{
					num *= ParseUnary();
					continue;
				}
				if (!TryRead('/'))
				{
					break;
				}
				num /= ParseUnary();
			}
			return num;
		}

		private long ParseUnary()
		{
			SkipWhiteSpace();
			if (TryRead('+'))
			{
				return ParseUnary();
			}
			if (TryRead('-'))
			{
				return -ParseUnary();
			}
			return ParsePower();
		}

		private long ParsePower()
		{
			long num = ParsePrimary();
			SkipWhiteSpace();
			if (TryRead('^'))
			{
				long num2 = ParseUnary();
				if (num2 < 0)
				{
					throw new InvalidOperationException("Failed to parse expression: " + expression);
				}
				num = Pow(num, num2);
			}
			return num;
		}

		private long ParsePrimary()
		{
			SkipWhiteSpace();
			if (TryRead('('))
			{
				long result = ParseExpression();
				SkipWhiteSpace();
				if (!TryRead(')'))
				{
					throw new InvalidOperationException("Failed to parse expression: " + expression);
				}
				return result;
			}
			return ParseNumber();
		}

		private long ParseNumber()
		{
			SkipWhiteSpace();
			int num = index;
			while (index < expression.Length && char.IsDigit(expression[index]))
			{
				index++;
			}
			if (num == index)
			{
				throw new InvalidOperationException("Failed to parse expression: " + expression);
			}
			return long.Parse(expression.Substring(num, index - num), NumberFormatInfo.InvariantInfo);
		}

		private static long Pow(long value, long exponent)
		{
			long num = 1L;
			checked
			{
				while (exponent > 0)
				{
					if ((exponent & 1) == 1)
					{
						num *= value;
					}
					exponent >>= 1;
					if (exponent > 0)
					{
						value *= value;
					}
				}
				return num;
			}
		}

		private bool TryRead(char c)
		{
			if (index >= expression.Length || expression[index] != c)
			{
				return false;
			}
			index++;
			return true;
		}

		private void SkipWhiteSpace()
		{
			while (index < expression.Length && char.IsWhiteSpace(expression[index]))
			{
				index++;
			}
		}
	}

	public static Vector3 EvaluateVector3(string expression)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		string[] array = Parse.Split(expression);
		if (Parse.TryDistanceAngle(array, out var vector))
		{
			return vector;
		}
		Vector3 zero = Vector3.zero;
		zero.x = EvaluateFloat(array[0]).GetValueOrDefault();
		if (array.Length > 1)
		{
			zero.z = EvaluateFloat(array[1]).GetValueOrDefault();
		}
		if (array.Length > 2)
		{
			zero.y = EvaluateFloat(array[2]).GetValueOrDefault();
		}
		return zero;
	}

	public static Quaternion EvaluateQuaternion(string expression)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		Vector3 zero = Vector3.zero;
		string[] array = Parse.Split(expression);
		zero.y = EvaluateFloat(array[0]).GetValueOrDefault();
		if (array.Length > 1)
		{
			zero.x = EvaluateFloat(array[1]).GetValueOrDefault();
		}
		if (array.Length > 2)
		{
			zero.z = EvaluateFloat(array[2]).GetValueOrDefault();
		}
		return Quaternion.Euler(zero);
	}

	public static int? EvaluateInt(string expression)
	{
		try
		{
			return (int?)EvaluateLong(expression);
		}
		catch
		{
			return null;
		}
	}

	public static float? EvaluateFloat(string expression)
	{
		try
		{
			return (float)EvaluateDouble(expression);
		}
		catch
		{
			return null;
		}
	}

	private static double EvaluateDouble(string expression)
	{
		return new DoubleParser(expression.Replace("**", "^")).Parse();
	}

	public static long? EvaluateLong(string expression)
	{
		try
		{
			return EvalLong(expression);
		}
		catch
		{
			return null;
		}
	}

	private static long EvalLong(string expression)
	{
		return new LongParser(expression.Replace("**", "^")).Parse();
	}
}
