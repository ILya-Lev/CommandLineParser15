using System;
using System.Collections.Generic;
using System.Linq;

namespace StringComparison.BusinessLogic
{
	internal class Mismatch
	{
		public string LeftHandSidePart { get; set; } = "";
		public string RightHandSidePart { get; set; } = "";
		public int StartsAt { get; set; }
		public void AddLeftHandSideSymbol(char symbol) => LeftHandSidePart += symbol;
		public void AddRightHandSideSymbol(char symbol) => RightHandSidePart += symbol;
	}

	public class Comparisor
	{
		public void Compare(string lhs, string rhs)
		{
			IReadOnlyList<Mismatch> differences = GetDifferences(lhs, rhs).ToList();

		}

		private static IEnumerable<Mismatch> GetDifferences(string lhs, string rhs)
		{
			for (int position = 0; position < Math.Min(lhs.Length, rhs.Length); position++)
			{
				if (lhs[position] != rhs[position])
				{
					var difference = new Mismatch { StartsAt = position };
					for (int shift = 0; shift < Math.Min(lhs.Length, rhs.Length) - position; shift++)
					{
						if (lhs[position + shift] != rhs[position + shift])
						{
							difference.AddLeftHandSideSymbol(lhs[position + shift]);
							difference.AddRightHandSideSymbol(rhs[position + shift]);
						}
						else
						{
							position += shift - 1;
							break;
						}
					}
					yield return difference;
				}
			}
		}
	}
}
