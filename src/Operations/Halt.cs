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
	/// halt: 0
	///   stop execution and terminate the program
	/// </summary>
	public class Halt : IOperation
	{
		public ushort GetOpCode() => 0;
		public ushort GetNumParameters() => 0;
		public bool Execute(IVirtualMemory memory)
		{
			return false;
		}
	}
}
