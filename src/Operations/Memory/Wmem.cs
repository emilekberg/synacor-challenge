using Serilog;
using synacor_challange.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace synacor_challange.Instructions
{
	/// <summary>
	/// wmem: 16 a b
	///   write the value from <b> into memory at address <a>
	/// </summary>
	public class Wmem : IOperation
	{
		protected ILogger Logger { get; }
		public Wmem(ILogger logger)
		{
			Logger = logger;
		}
		public ushort GetOpCode() => 16;
		public ushort GetNumParameters() => 2;
		public bool Execute(IVirtualMemory memory)
		{
			ushort a = memory.TryReadRegistry(memory.ReadNext());
			ushort b = memory.TryReadRegistry(memory.ReadNext());
			Logger.Debug($"- a: {a}, b: {b}");
			memory.Write(a, b);
			return true;
		}
	}
}
