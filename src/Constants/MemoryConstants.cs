using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace synacor_challange.Constants
{
	public static class MemoryConstants
	{
		public const ushort FifteenBits = 0b111_1111_1111_1111;
		public const ushort MemorySize = FifteenBits;
		public const ushort RegistrySize = 8;
		public const ushort RegistryLower = short.MaxValue + 1;
		public const ushort RegistryUpper = RegistryLower + RegistrySize;
		
	}
}
