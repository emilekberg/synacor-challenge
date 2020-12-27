using Serilog;
using synacor_challange.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace synacor_challange.Instructions
{
	/// <summary>
	/// out: 19 a
	///   write the character represented by ascii code<a> to the termina
	/// </summary>
	public class Out : IOperation
	{
		protected ILogger Logger { get; }
		public Out(ILogger logger)
		{
			Logger = logger;
		}
		public ushort GetOpCode() => 19;
		public ushort GetNumParameters() => 1;

		public bool Execute(IVirtualMemory memory)
		{
			var a = memory.ReadNext();
			Logger.Debug($"- a:{a}");
			if (memory.IsRegistry(a))
			{
				a = memory.Read(a);
			}
			char ch = Convert.ToChar(a);
			Logger.Debug($"- a:{a}, ch:{ch}");
			Console.Write(ch);
			return true;
		}
	}
}
