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
	/// rmem: 15 a b
	///   read memory at address <b> and write it to <a>
	/// </summary>
	public class Rmem : IOperation
	{
		protected ILogger Logger { get; }
		public Rmem(ILogger logger)
		{
			Logger = logger;
		}
		public ushort GetOpCode() => 15;
		public ushort GetNumParameters() => 2;
		public bool Execute(IVirtualMemory memory)
		{
			ushort a = memory.ReadNext();
			ushort b = memory.TryReadRegistry(memory.ReadNext());
			var result = memory.Read(b);
			Logger.Debug($"- a: {a}, b: {b}, result: {result}");
			memory.Write(a, result);
			return true;
		}
	}
}
