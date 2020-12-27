using synacor_challange.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace synacor_challange.Instructions
{
	/// <summary>
	/// mult: 10 a b c
	///   store into <a> the product of <b> and <c> (modulo 32768)
	/// </summary>
	public class Mult : IOperation
	{
		public ushort GetOpCode() => 10;
		public ushort GetNumParameters() => 3;
		public bool Execute(IVirtualMemory memory)
		{
			ushort a = memory.ReadNext();
			ushort b = memory.ReadNext();
			ushort c = memory.ReadNext();

			if (memory.IsRegistry(b))
			{
				b = memory.Read(b);
			}
			if (memory.IsRegistry(c))
			{
				c = memory.Read(c);
			}

			ushort result = (ushort)((b * c) % 32768);

			memory.Write(a, result);
			return true;
		}
	}
}
