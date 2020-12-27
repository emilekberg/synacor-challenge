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
	/// jt: 7 a b
	///   if <a> is nonzero, jump to<b>
	/// </summary>
	public class Jt : IOperation
	{
		protected ILogger Logger { get; }
		public Jt(ILogger logger)
		{
			Logger = logger;
		}
		public ushort GetOpCode() => 7;
		public ushort GetNumParameters() => 2;
		public bool Execute(IVirtualMemory memory)
		{
			var a = memory.ReadNext();
			var b = memory.ReadNext();
			Logger.Debug($"- a:{a}, b:{b}");

			if(memory.IsRegistry(a))
			{
				a = memory.Read(a);
			}
			if (memory.IsRegistry(b))
			{
				b = memory.Read(b);
			}
			Logger.Debug($"- a:{a}, b:{b}");
			if (a != 0)
			{
				Logger.Debug($"- a != 0, jumping to {b}");
				memory.ChangeAddressPointer(b);
			}
			return true;
		}
	}
}
