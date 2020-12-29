using synacor_challange.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace synacor_challange.Instructions
{
	/// <summary>
	/// not: 14 a b
	///   stores 15-bit bitwise inverse of <b> in <a>
	/// </summary>
	public class Not : IOperation
	{
		public ushort GetOpCode() => 14;
		public ushort GetNumParameters() => 2;
		public bool Execute(IVirtualMemory memory)
		{
			ushort a = memory.ReadNext();
			ushort b = memory.TryReadRegistry(memory.ReadNext());

			// stores the 15 bitwise invert
			var result = (ushort)(~b & 0b111111111111111);

			memory.Write(a, (ushort)result);
			return true;
		}
	}
}
