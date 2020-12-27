using Serilog;
using synacor_challange.Instructions;
using synacor_challange.Interfaces;
using synacor_challange.Parsers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace synacor_challange
{
	public class VirtualMachine
	{
		protected IVirtualMemory Memory { get; init; }
		protected Dictionary<ushort, IOperation> Operations { get; init; }
		protected ILogger Logger { get; init; }
		public VirtualMachine(IEnumerable<IOperation> operations, IVirtualMemory memory, ILogger logger)
		{
			Logger = logger;
			Memory = memory;
			Operations = operations.ToDictionary(x => x.GetOpCode(), x => x);
		}

		public void DebugPrintOperations()
		{
			var sb = new StringBuilder();
			sb.AppendLine("Registered operations:");
			foreach(var key in Operations.Keys.OrderBy(x => x))
			{
				sb.AppendLine($"- {key.ToString().PadLeft(2, '0')}: {Operations[key].GetType().Name}");
			}
			var s = sb.ToString();
			Logger.Information(s);
		}

		public void Run()
		{
			Logger.Information($"Running program in memory...");
			uint numCalls = 0;
			bool continueRunning;
			do
			{
				if(Memory.GetAddressPointer() is 05451 or 06027 && false)
				{
					var a = 05605;
					Memory.SetRegistry(7, 14234);
				}
				var opCode= Memory.ReadNext();
				numCalls++;
				if (!Operations.TryGetValue(opCode, out var operation))
				{
					Logger.Error($"{opCode} not found amongst registered operations");
				}
				Logger.Debug($"operation#{numCalls}, {Memory.GetAddressPointer()}, opcode: {opCode} - {operation.GetType().Name}");
				continueRunning = operation.Execute(Memory);

			} while (continueRunning);
			Console.ReadKey();
		}

		public void LoadProgram(ushort[] program)
		{
			Logger.Information($"Loading program to memory...");
			Memory.Copy(program);
			Logger.Information($"Loading program to memory done!");
		}

		public void SaveState()
		{
			Logger.Information("Saving state to file...");
			var current = Memory.GetAddressPointer();
			Memory.ChangeAddressPointer(0);
			SaveFileParser.SaveStateToFile(Memory, "./program.txt", Operations);
			Memory.ChangeAddressPointer(current);
			Logger.Information("Saving state to file done!");
		}
	}
}
