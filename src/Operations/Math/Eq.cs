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
	/// eq: 4 a b 
	///   set <a> to 1 if <b> is equal to <c>; set it to 0 otherwise
	/// </summary>
	public class Eq : IOperation
	{
		protected ILogger Logger { get; }
		public Eq(ILogger logger)
		{
			Logger = logger;
		}
		public ushort GetOpCode() => 4;
		public ushort GetNumParameters() => 3;
		public bool Execute(IVirtualMemory memory)
		{
			var a = memory.ReadNext();
			var b = memory.ReadNext();
			var c = memory.ReadNext();
			Logger.Debug($"- a: {a}, b: {b}, c: {c}");
			if (memory.IsRegistry(b))
			{
				b = memory.Read(b);
			}
			if (memory.IsRegistry(c))
			{
				c = memory.Read(c);
			}

			ushort val = b == c ? 1 : 0;
			Logger.Debug($"- a: {a}, b: {b}, c: {c}, b == c: {val}");
			memory.Write(a, val);
			return true;
		}
	}
}
