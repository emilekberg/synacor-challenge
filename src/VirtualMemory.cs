using Serilog;
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
		public ushort[] Address { get; protected set; }
		public Stack<ushort> Stack { get; protected set; }

		public ushort CurrentAddress { get; protected set; }
		public ushort[] Registry
		{
			get => Address.Skip(short.MaxValue).Take(8+1).ToArray();
		}
		private ILogger Logger { get; }
		public VirtualMemory(ILogger logger)
		{
			Logger = logger;
			InitializeMemory();
		}

		public void InitializeMemory()
		{
			Logger.Information("Initializing memory...");
			Address = Enumerable.Range(0, short.MaxValue + 9).Select(_ => (ushort)0).ToArray();
			Stack = new Stack<ushort>();
			CurrentAddress = 0;
			Logger.Information("Initializing memor done!");
		}

		public ushort GetAddressPointer() => CurrentAddress;

		/// <summary>
		/// Increments the CurrentAddress Pointer and returns the value in the next memory poistion.
		/// </summary>
		/// <returns></returns>
		public ushort ReadNext()
		{
			return Address[CurrentAddress++];
		}

		public ushort Read(ushort address)
		{
			return Address[address];
		}

		/// <summary>
		/// Writes a value to the memory address
		/// </summary>
		/// <param name="destination"></param>
		/// <param name="value"></param>
		public void Write(ushort address, ushort value)
		{
			Address[address] = value;
		}

		public void Copy(ushort[] data)
		{
			// 15-bit.
			int maxlen = 0b111111111111111;
			if (data.Length > maxlen)
			{
				throw new Exception($"Program exceeds allowed length. Max: {maxlen}, Length: {data.Length}");
			}
			Array.Copy(data, Address, data.Length);
		}

		public void ChangeAddressPointer(ushort address)
		{
			CurrentAddress = address;
		}

		public void PushStack(ushort value) => Stack.Push(value);
		public bool TryPopStack(out ushort value) => Stack.TryPop(out value);

		public bool IsRegistry(ushort address)
		{
			const int RegistryLower = short.MaxValue;
			const int RegistryUpper = short.MaxValue + 8;
			return address > RegistryLower && address <= RegistryUpper;
		}

		public void SetRegistry(ushort registry, ushort value)
		{
			Address[short.MaxValue + registry] = value;
		}
	}
}
