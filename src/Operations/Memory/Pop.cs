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
	/// pop: 3 a
	///   remove the top element from the stack and write it into <a>; empty stack = error
	/// </summary>
	public class Pop : IOperation
	{
		public ushort GetOpCode() => 3;
		public ushort GetNumParameters() => 1;
		public bool Execute(IVirtualMemory memory)
		{
			var a = memory.ReadNext();
			if(!memory.TryPopStack(out var value))
			{
				throw new ArgumentException("error, error");
			}
			memory.Write(a, value);
			return true;
		}
	}
}
