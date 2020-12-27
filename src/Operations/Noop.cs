using synacor_challange.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace synacor_challange.Instructions
{
	/// <summary>
	/// noop: 21
	///   no operation
	/// </summary>
	public class Noop : IOperation
	{
		public ushort GetOpCode() => 21;
		public ushort GetNumParameters() => 0;

		public bool Execute(IVirtualMemory memory)
		{
			// No op.
			return true;
		}
	}
}
