using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace synacor_challange.Interfaces
{
	public class VirtualMemoryState
	{
		public ushort[] Address { get; set; }
		public ushort[] Registry { get; set; }
		public Stack<ushort> Stack { get; set; }
		public ushort CurrentAddress { get; set; }
	}
}
