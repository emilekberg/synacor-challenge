using Serilog;
using synacor_challange.Constants;
using synacor_challange.Instructions;
using synacor_challange.Interfaces;
using synacor_challange.Parsers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
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
#if DEBUG
			DebugPrintOperations();
#endif
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
			bool continueRunning;
			do
			{
				// when teleporter is used.
				if (Memory.GetAddressPointer() is 05451)
				{
					// the value 25734 comes from the RegistryVerificxator application, which extracts the code called from the VM, and 
					// implements it in somewhat faster c#.
					Memory.WriteRegistry(7, 25734);
				}
				// verification logic i think...
				else if (Memory.GetAddressPointer() is 05489)
				{
					// set 05489 and 05490 to noop. This will skip the verification logic.
					// instead, we run the verification logic in another program and have calcualted the value of register 7 to be 25734.
					// however with out the correct value on reg 8, this is not usable...
					Memory.Write(05489, 21);
					Memory.Write(05490, 21);
					// to tell the code that we know this value is good, we set the value of registry 0 to 6 here. Since that means the teleportation
					// device is properly setup.
					Memory.WriteRegistry(0, 00006);
				}
				// after self-test is done.
				else if (Memory.GetAddressPointer() is 00978)
				{
					// SaveState("passed-selftest.json");
				}
			
				var opCode= Memory.ReadNext();
				if (!Operations.TryGetValue(opCode, out var operation))
				{
					Logger.Error($"{opCode} not found amongst registered operations");
					continueRunning = false;
				}
				Logger.Debug($"{Memory.GetAddressPointer()}, opcode: {opCode} - {operation.GetType().Name}");
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

		public void ExportProgram()
		{
			Logger.Information("Saving state to file...");
			var current = Memory.GetAddressPointer();
			Memory.ChangeAddressPointer(0);
			SaveFileParser.SaveStateToFile(Memory, "./program.txt", Operations);
			Memory.ChangeAddressPointer(current);
			Logger.Information("Saving state to file done!");
		}

		public void SaveState(string path)
		{
			Logger.Information("Saving state...");
			ushort oldPointer = Memory.GetAddressPointer();
			// change pointer to previous command, to make sure we go back to correct state.
			Memory.ChangeAddressPointer((ushort)(oldPointer));
			VirtualMemory memory = (Memory as VirtualMemory);
			string json = JsonSerializer.Serialize(memory);
			Memory.ChangeAddressPointer(oldPointer);

			File.WriteAllText(path, json);
		}

		public void LoadState(string path)
		{
			var json = File.ReadAllText(path);
			Logger.Information("Loading state...");
			VirtualMemoryState memoryState = JsonSerializer.Deserialize<VirtualMemoryState>(json);
			VirtualMemory memory = (Memory as VirtualMemory);
			memory.InitializeMemory();

			Memory.Copy(memoryState.Address);
			Memory.ChangeAddressPointer(memoryState.CurrentAddress);
			Queue<ushort> stackAsQueue = new Queue<ushort>(memoryState.Stack);
			while(stackAsQueue.TryDequeue(out var stackValue))
			{
				Memory.PushStack(stackValue);
			}
			for(ushort i = 0; i < MemoryConstants.RegistrySize; i++)
			{
				Memory.WriteRegistry(i, memoryState.Registry[i]);
			}
			Logger.Information("Loading state done!");

		}
	}
}
