using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace synacor_challange.Parsers
{
	public static class ProgramParser
	{
		/// <summary>
		/// convers a string into a program.
		/// </summary>
		/// <param name="program"></param>
		/// <returns></returns>
		public static ushort[] StringToProgram(string program) => program
			.Split(',', StringSplitOptions.RemoveEmptyEntries)
			.Select(ushort.Parse)
			.ToArray();
		/// <summary>
		/// Reads a file path containing a binary program into a program.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static ushort[] BinaryToProgram(string path)
		{
			byte[] data = File.ReadAllBytes(path);
			ushort[] program = new ushort[(int)Math.Ceiling(data.Length / 2m)];
			Buffer.BlockCopy(data, 0, program, 0, data.Length);

			return program;
		}
		public static IEnumerable<ushort> MapParameters(string str)
		{
			return str.Split(' ').Select(x =>
			{
				if (x.StartsWith("$"))
				{
					var reg = int.Parse(x.Replace("$", string.Empty));
					return (ushort)(short.MaxValue + reg);
				}
				return Convert.ToUInt16(x);
			});
		}
		public static ushort[] DisassembledToProgram(string path)
		{
			var instructionLookups = new List<string>
			{
				"halt", "set", "push", "pop", "eq",  "gt",   "jmp",  "jt",   "jf",  "add",
				"mult", "mod", "and",  "or",  "not", "rmem", "wmem", "call", "ret", "out",
				"in",   "noop"
			};
			string[] serializedData = File.ReadAllLines(path);
			var re = new Regex(@"\d+ : .*");
			var programData = new List<ushort>();
			foreach(var line in serializedData)
			{
				var split = line.Split(" : ", 2);
				var rowInfo = split[0].Trim();
				var rowData = split[1].Trim();

				var splitRowData = rowData.Split(' ', 2);
				var op = splitRowData[0].Trim();
				var opCode = (ushort)instructionLookups.IndexOf(op);
				switch(op)
				{
					default:
						programData.Add(opCode);
						if(splitRowData.Length > 1)
						{
							var parameters = MapParameters(splitRowData[1].Trim());
							programData.AddRange(parameters);
						}
						break;
					case "out":
						var outs = splitRowData[1].ToCharArray().Select(c =>
						{
							return new List<ushort>
							{
								opCode,
								Convert.ToUInt16(c)
							};
						}).SelectMany(x => x).ToList();
						programData.AddRange(outs);
						break;
				}
			}


			return programData.ToArray();
		}
	}
}
