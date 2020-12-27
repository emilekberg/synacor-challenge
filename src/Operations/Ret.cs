using synacor_challange.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace synacor_challange.Instructions
{
	/// <summary>
	/// ret: 18
	///   remove the top element from the stack and jump to it; empty stack = halt
	/// </summary>
	public class Ret : IOperation
	{
		public ushort GetOpCode() => 18;
		public ushort GetNumParameters() => 0;
		public bool Execute(IVirtualMemory memory)
		{
			if (!memory.TryPopStack(out var value))
			{
				throw new ArgumentException("Stack is empty.");
			}
			memory.ChangeAddressPointer(value);
			return true;
		}
	}
}
