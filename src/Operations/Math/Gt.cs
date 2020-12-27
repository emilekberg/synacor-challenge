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
	/// gt: 5 a b c
	///   set <a> to 1 if <b> is greater than <c>; set it to 0 otherwise
	/// </summary>
	public class Gt : IOperation
	{
		public ushort GetOpCode() => 5;
		public ushort GetNumParameters() => 3;
		public bool Execute(IVirtualMemory memory)
		{
			var a = memory.ReadNext();
			var b = memory.ReadNext();
			var c = memory.ReadNext();

			if (memory.IsRegistry(b))
			{
				b = memory.Read(b);
			}
			if (memory.IsRegistry(c))
			{
				c = memory.Read(c);
			}

			ushort val = b > c ? 1 : 0;
			memory.Write(a, val);
			return true;
		}
	}
}
