using System;
using System.Collections.Generic;
using System.Threading;

namespace RegistryVerificator
{
	class Program
	{
		/// <summary>
		/// Record for registry, only R0, R1 and R7 is viable for calculating this.
		/// </summary>
		public record Registry(ushort R0, ushort R1, ushort R7);
		/// <summary>
		/// Caches using tuples. 
		/// </summary>
		public static Dictionary<(ushort r0, ushort r1), (ushort r0, ushort r1)> Cache = new ();
		/// <summary>
		/// Caches using records.
		/// </summary>
		public static Dictionary<Registry, Registry> Memoization = new();
	

		static void Main(string[] args)
		{
			// trial and error to find good value.
			var stackSize = 100_000_000;
			// increase the call stack size, since this algorithm is recursive.
			// You could write a algorithm to flatten it, but bruteforce and trial and error was easier for me.
			var thread = new Thread(new ThreadStart(DoWork), stackSize);
			thread.Start();
		}
		public static void DoWork()
		{
			bool succeded;
			ushort i = 25700;
			
			Registry reg = new Registry(4, 1, i);
			Console.WriteLine($"starting with {reg}");
			do
			{
				if(i % 100 == 0)
				{
					Console.WriteLine(i);
				}
				Memoization.Clear();
				 var res = Address06027_Memo_Records(reg with { R7 = i});
				if(res.R0 != 6)
				{
					succeded = false;
				}
				else
				{
					succeded = true;
					Console.WriteLine($"result is {res}");
					Console.WriteLine($"Set registry 7 to {i} to teleport correctly");
				}
				i++;
			} while (!succeded);
			Console.ReadKey();
		}
		/// <summary>
		/// Readable implementation of what's going on inside of the assembly code.
		/// This implementation will reach stack limit by C#, so we need to optimize it.
		/// </summary>
		/// <param name="reg"></param>
		/// <param name="stack"></param>
		public static void Address06027_Slow(ushort[] reg, Stack<ushort> stack)
		{
			// jt $0 06035
			if (reg[0] == 0)
			{
				// add $0 $1 00001
				reg[0] = Add(reg[1], 1);

				// reg[0] = (ushort)((reg[1] + 1) % 32768);
				// ret
				return;
			}
			// jt $1 06048
			if (reg[1] == 0)
			{
				// add $0 $0 32767
				// reg[0] = (ushort)((reg[0] + 32767) % 32768);
				reg[0] = SubtractOne(reg[0]);
				// set $1 $7
				reg[1] = reg[7];
				// call 06027
				Address06027_Slow(reg, stack);
				// ret
				return;
			}
			// push $0
			stack.Push(reg[0]);
			// add $1 $1 32767
			// reg[1] = (ushort)((reg[1] + 32767) % 32768);
			reg[1]--;
			// call 06027
			Address06027_Slow(reg, stack);
			// set $1 $0
			reg[1] = reg[0];
			// pop $0
			reg[0] = (ushort)(stack.Pop() - 1);
			// add $0 $0 32767
			// reg[0] = (ushort)((reg[0] + 32767) % 32768);
			// call 06027
			Address06027_Slow(reg, stack);
		}
		/// <summary>
		/// Tuple implementation for cache with external call for memoization.
		/// </summary>
		/// <param name="reg"></param>
		public static void Address06027_Memo_Tuple(ushort[] reg)
		{
			(ushort r0, ushort r1) result;
			if (reg[0] == 0)
			{
				reg[0] = Add(reg[1], 1);
				return;
			}
			if (reg[1] == 0)
			{
				reg[0] = SubtractOne(reg[0]);
				reg[1] = reg[7];
				result = CacheCall(reg[0], reg[1], reg[7]);
				reg[0] = result.r0;
				reg[1] = result.r1;
				return;
			}
			reg[1] = SubtractOne(reg[1]);
			reg[1] = CacheCall(reg[0], reg[1], reg[7]).r0;
			reg[0] = SubtractOne(reg[0]);

			result = CacheCall(reg[0], reg[1], reg[7]);
			reg[0] = result.r0;
			reg[1] = result.r1;
		}
		public static (ushort r0, ushort r1) CacheCall(ushort r0, ushort r1, ushort r7)
		{
			if (!Cache.TryGetValue((r0, r1), out var c))
			{
				ushort[] reg = new ushort[] { r0, r1, 0, 0, 0, 0, 0, r7 };
				Address06027_Memo_Tuple(reg);
				c = (reg[0], reg[1]);
				Cache.Add((r0, r1), c);
			}
			return (c.r0, c.r1);
		}
		/// <summary>
		/// Memoization with records instead. IMO a bit cleaner than using tuples.
		/// It's still not very fast, so when calling i'm skipping values that i've parsed before to speed it up a bit.
		/// </summary>
		/// <param name="r"></param>
		/// <returns></returns>
		public static Registry Address06027_Memo_Records(Registry registry)
		{
			if(Memoization.TryGetValue(registry, out var result))
			{
				return result;
			}
			if(registry.R0 == 0)
			{
				result = registry with
				{
					R0 = Add(registry.R1, 1)
				};
				Memoization.Add(registry, result);
				return result;
			}
			if(registry.R1 == 0)
			{
				result = Address06027_Memo_Records(registry with
				{
					R0 = SubtractOne(registry.R0),
					R1 = registry.R7
				});
				Memoization.Add(registry, result);
				return result;
			}
			ushort registryOne = Address06027_Memo_Records(registry with
			{
				R1 = SubtractOne(registry.R1)
			}).R0;
			result = Address06027_Memo_Records(registry with
			{
				R0 = SubtractOne(registry.R0),
				R1 = registryOne
			});
			Memoization.Add(registry, result);
			return result;
		}
		/// <summary>
		/// Adds RHS to LHS, and loops around using modulus.
		/// Helper method to get the code a bit more readable.
		/// </summary>
		/// <param name="lhs"></param>
		/// <param name="rhs"></param>
		/// <returns></returns>
		public static ushort Add(ushort lhs, ushort rhs)
		{
			return (ushort)((lhs + rhs) % 32768);
		}
		/// <summary>
		/// Subtracts one from LHS.
		/// Helper method to get the code a bit more readable.
		/// </summary>
		/// <param name="lhs"></param>
		/// <returns></returns>
		public static ushort SubtractOne(ushort lhs)
		{
			return Add(lhs, 32767);
		}
	}
}
