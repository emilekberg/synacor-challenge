using synacor_challange.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace synacor_challange.Instructions
{
	/// <summary>
	/// call: 17 a
	///   write the address of the next instruction to the stack and jump to <a>
	/// </summary>
	public class Call : IOperation
	{

		public ushort GetOpCode() => 17;
		public ushort GetNumParameters() => 1;
		public bool Execute(IVirtualMemory memory)
		{
			ushort a = memory.TryReadRegistry(memory.ReadNext());
			ushort b = (ushort)(memory.GetAddressPointer());
			memory.PushStack(b);
			memory.ChangeAddressPointer(a);
			return true;
		}
	}
}
