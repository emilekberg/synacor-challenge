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
	/// jmp: 6 a
	///   jump to<a>
	/// </summary>
	public class Jmp : IOperation
	{
		public ushort GetOpCode() => 6;
		public ushort GetNumParameters() => 1;
		public bool Execute(IVirtualMemory memory)
		{
			ushort a = memory.TryReadRegistry(memory.ReadNext());
			memory.ChangeAddressPointer(a);
			return true;
		}
	}
}
