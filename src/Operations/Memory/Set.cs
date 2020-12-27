using Serilog;
using synacor_challange.Instructions;
using synacor_challange.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace synacor_challange.Operations
{
	/// <summary>
	/// set: 1 a b
	///   set register<a> to the value of <b>
	/// </summary>
	public class Set : IOperation
	{
		protected ILogger Logger { get; }
		public Set(ILogger logger)
		{
			Logger = logger;
		}
		public ushort GetOpCode() => 1;
		public ushort GetNumParameters() => 2;
		public bool Execute(IVirtualMemory memory)
		{
			ushort a = memory.ReadNext();
			ushort b = memory.ReadNext();
			Logger.Debug($"- a: {a}, b: {b}");
			if (memory.IsRegistry(b))
			{
				b = memory.Read(b);
			}
			Logger.Debug($"- a: {a}, b: {b}");
			memory.Write(a, b);
			return true;
		}
	}
}
