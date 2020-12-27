using synacor_challange.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace synacor_challange.Interfaces
{
	public interface IOperation
	{
		/// <summary>
		/// Returns true if the instruction can parse the command given.
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		public ushort GetOpCode();
		/// <summary>
		/// Executes the operation.
		/// </summary>
		/// <param name="memory"></param>
		/// <returns>Returns false if the program shall halt.</returns>
		public bool Execute(IVirtualMemory memory);
		/// <summary>
		/// Returns the number of parameters read by the operation.
		/// </summary>
		/// <returns></returns>
		public ushort GetNumParameters();
	}
}
