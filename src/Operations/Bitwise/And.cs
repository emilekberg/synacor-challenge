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
	/// and: 12 a b 
	///   stores into <a> the bitwise and of <b> and <c>
	/// </summary>
	public class And : IOperation
	{
		public ushort GetOpCode() => 12;
		public ushort GetNumParameters() => 3;
		public bool Execute(IVirtualMemory memory)
		{
			ushort a = memory.ReadNext();
			ushort b = memory.TryReadRegistry(memory.ReadNext());
			ushort c = memory.TryReadRegistry(memory.ReadNext());

			ushort result = (ushort)(b & c);

			memory.Write(a, result);
			return true;
		}
	}
}
