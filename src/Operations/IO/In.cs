using Serilog;
using synacor_challange.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace synacor_challange.Instructions
{
	/// <summary>
	/// in: 20 a
	///     read a character from the terminal and write its ascii code to <a>; it can be assumed that once input starts, it will continue until a newline is encountered; 
	///     this means that you can safely read whole lines from the keyboard and trust that they will be fully read
	/// </summary>
	public class In : IOperation
	{
		const string Path = "./input.txt";
		protected Queue<char> Input { get; }
 		protected ILogger Logger { get; }
		protected bool Write { get; } = false;
		protected bool Read { get; } = true;
		public In(ILogger logger)
		{
			Logger = logger;
			EnsureFileExists(Path);
			Input = new Queue<char>(File.ReadAllText(Path));

		}

		public void EnsureFileExists(string path)
		{
			if (!File.Exists(path))
			{
				// Create a file to write to.
				using StreamWriter sw = File.CreateText(path);
			}
		}

		public ushort GetOpCode() => 20;
		public ushort GetNumParameters() => 1;

		public bool Execute(IVirtualMemory memory)
		{
			var a = memory.ReadNext();
			if (!Read || !Input.TryDequeue(out char c))
			{
				var input = Console.ReadKey();

				c = input.KeyChar;
				if (c == '\r')
				{
					c = '\n';
				}
				if(Write)
				{
					using StreamWriter sw = File.AppendText(Path);
					sw.Write(c);
				}

			}
			else
				Console.Write(c);
			var cAsUInt16 = Convert.ToUInt16(c);

			Logger.Debug($"read {cAsUInt16}/{c} into {a}");

			memory.Write(a, Convert.ToUInt16(cAsUInt16));
			return true;
		}

		
	}
}
