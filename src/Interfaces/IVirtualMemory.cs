using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace synacor_challange.Interfaces
{
	public interface IVirtualMemory
	{
		/// <summary>
		/// Increments the CurrentAddress Pointer and returns the value in the next memory poistion.
		/// </summary>
		/// <returns></returns>
		public ushort ReadNext();

		/// <summary>
		/// Reads the value at the specified address.
		/// </summary>
		/// <param name="address"></param>
		/// <returns></returns>
		public ushort Read(ushort address);

		/// <summary>
		/// Writes a value to the memory address.
		/// </summary>
		/// <param name="destination"></param>
		/// <param name="value"></param>
		public void Write(ushort address, ushort value);

		/// <summary>
		/// Copy data to memory.
		/// </summary>
		/// <param name="data"></param>
		public void Copy(ushort[] data);

		/// <summary>
		/// Changes the memory pointer to the specified address.
		/// </summary>
		/// <param name="address"></param>
		public void ChangeAddressPointer(ushort address);
		/// <summary>
		/// Returns the current address of the memory pointer.
		/// </summary>
		/// <returns></returns>
		public ushort GetAddressPointer();

		/// <summary>
		/// Pushes a value to the stack.
		/// </summary>
		/// <param name="value"></param>
		public void PushStack(ushort value);
		/// <summary>
		/// Pops a value from the stack.
		/// </summary>
		/// <returns></returns>
		public bool TryPopStack(out ushort value);
		/// <summary>
		/// Returns true if the registry is a memory address or not.
		/// </summary>
		/// <param name="address"></param>
		/// <returns></returns>
		public bool IsRegistry(ushort address);

		/// <summary>
		/// Sets registry to a specific value. 
		/// Example: SetRegistry(1, 40); will set registry 1 to 40.
		/// </summary>
		public void SetRegistry(ushort registry, ushort value);
	}
}
