using System;
using System.Collections.Generic;
using System.Linq;
namespace CoinBruteForcer
{
	class Program
	{
		static void Main(string[] args)
		{
			var options = new List<int>
			{
				2,3,5,7,9
			};
			var coinNames = new Dictionary<int, string>()
			{
				[2] = "red coin",
				[3] = "corroded coin",
				[5] = "shiny coin",
				[7] = "concave coin",
				[9] = "blue coin"
			};
			var permutations = new List<List<int>>();
			Permutations(options, new List<int>(), ref permutations);

			foreach(var permutation in permutations)
			{
				var a = permutation[0];
				var b = permutation[1];
				var c = permutation[2];
				var d = permutation[3];
				var e = permutation[4];
				var result = Formula(a, b, c, d, e);
				Console.WriteLine($"{a} + {b} * {c}^2 + {d}^2 - {e} = {result}");
				if (result == 399)
				{
					Console.WriteLine($"{a},{b},{c},{d},{e} is the one");
					Console.WriteLine($"{coinNames[a]},{coinNames[b]},{coinNames[c]},{coinNames[d]},{coinNames[e]} is the one");
					break;
				}
			}
			Console.ReadKey();
		}

		public static void Permutations(List<int> values, List<int> result, ref List<List<int>> all)
		{
			for(int i = 0; i < values.Count; i++)
			{
				var val = values[i];
				var newValues = values.Except(new List<int> { val }).ToList();
				var newResult = result.ToList();
				newResult.Add(val);
				

				if (newValues.Count == 0)
				{
					all.Add(newResult);
				}
				Permutations(newValues, newResult, ref all);
			}
		}

		public static double Formula(int a, int b, int c, int d, int e)
		{
			return a + b * Math.Pow(c, 2) + Math.Pow(d, 3) - e;
		}
	}
}
