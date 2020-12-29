using Serilog;
using synacor_challange.Constants;
using synacor_challange.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace synacor_challange
{
	public class VirtualMemory : IVirtualMemory
	{
		public ushort[] Address { get; set; }
		public Stack<ushort> Stack { get; set; }

		public ushort CurrentAddress { get; set; }
		public ushort[] Registry { get; set; }
		private ILogger Logger { get; }
		public VirtualMemory(ILogger logger)
		{
			Logger = logger;
			InitializeMemory();
		}

		public void InitializeMemory()
		{
			Logger.Information("Initializing memory...");
			Logger.Information($"creating general memory, size {MemoryConstants.MemorySize}...");
			Address = Enumerable.Range(0, MemoryConstants.MemorySize).Select(_ => (ushort)0).ToArray();
			Logger.Information($"creating registry, size {MemoryConstants.RegistrySize}...");
			Registry = Enumerable.Range(0, MemoryConstants.RegistrySize).Select(_ => (ushort)0).ToArray();
			Stack = new Stack<ushort>();
			CurrentAddress = 0;
			Logger.Information("Initializing memory done!");
		}
		public void Copy(ushort[] data)
		{
			InitializeMemory();
			int maxlen = MemoryConstants.FifteenBits;
			if (data.Length > maxlen)
			{
				throw new Exception($"Program exceeds allowed length. Max: {maxlen}, Length: {data.Length}");
			}
			Array.Copy(data, Address, data.Length);
		}


		public ushort GetAddressPointer() => CurrentAddress;
		public void ChangeAddressPointer(ushort address) => CurrentAddress = address;
		#region Memory
		public ushort ReadNext() => Address[CurrentAddress++];

		public ushort Read(ushort address) => Address[address];

		public void Write(ushort addressOrRegister, ushort value)
		{
			if(IsRegistry(addressOrRegister))
			{
				var registry = ToRegistry(addressOrRegister);
				WriteRegistry(registry, value);
			}
			else
			{
				Address[addressOrRegister] = value;
			}
			
		}
		#endregion
		#region stack

		public void PushStack(ushort value) => Stack.Push(value);
		public bool TryPopStack(out ushort value) => Stack.TryPop(out value);
		#endregion
		#region registry
		public ushort ReadRegistry(ushort registry) => Registry[registry];
		public ushort TryReadRegistry(ushort value)
		{
			if(IsRegistry(value))
			{
				var registry = ToRegistry(value);
				return ReadRegistry(registry);
			}
			return value;
		}
		public void WriteRegistry(ushort registry, ushort value) => Registry[registry] = value;

		public bool IsRegistry(ushort address)
		{
			return address >= MemoryConstants.RegistryLower && address < MemoryConstants.RegistryUpper;
		}
		public ushort ToRegistry(ushort value)
		{
			return (ushort)(value - MemoryConstants.RegistryLower);
		}
		#endregion

	}
}
