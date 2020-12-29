using Serilog;
using synacor_challange.Interfaces;
using synacor_challange.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace synacor_challange.Operations
{
	/// <summary>
	/// add: 9 a b c
	///   assign into<a> the sum of<b> and <c> (modulo 32768)
	/// </summary>
	public class Add : IOperation
	{
		protected ILogger Logger;
		public Add(ILogger logger)
		{
			Logger = logger;
		}
		public ushort GetOpCode() => 9;
		public ushort GetNumParameters() => 3;
		public bool Execute(IVirtualMemory memory)
		{
			ushort a = memory.ReadNext();
			ushort b = memory.TryReadRegistry(memory.ReadNext());
			ushort c = memory.TryReadRegistry(memory.ReadNext());

			ushort result = (ushort)((b + c) % 32768);
			Logger.Debug($"- a: {a}, b: {b}, c: {c}, result: {result}");
			memory.Write(a, result);
			return true;
		}
	}
}
