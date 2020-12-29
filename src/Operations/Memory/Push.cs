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
	/// push: 2 a
	///   push<a> onto the stac
	/// </summary>
	public class Push : IOperation
	{
		public ushort GetOpCode() => 2;
		public ushort GetNumParameters() => 1;
		public bool Execute(IVirtualMemory memory)
		{
			ushort a = memory.TryReadRegistry(memory.ReadNext());
			memory.PushStack(a);
			return true;
		}
	}
}
