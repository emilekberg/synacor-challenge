using synacor_challange.Instructions;
using synacor_challange.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace synacor_challange.Parsers
{
	public static class SaveFileParser
	{
		public static void SaveStateToFile(IVirtualMemory memory, string path, Dictionary<ushort, IOperation> operations)
		{
			var sb = new StringBuilder();
			var memoryPadLength = ushort.MaxValue.ToString().Length;
			do
			{
				var currentMemAdress = memory.GetAddressPointer();
				var opCode = memory.ReadNext();

				if (operations.TryGetValue(opCode, out var operation))
				{
					if (operation.GetType() == typeof(Out))
					{
						PrintString(sb, memory);
						continue;
					}
					else
					{
						sb.Append($"{currentMemAdress.ToString().PadLeft(memoryPadLength, '0')}");
						sb.Append(" : ");
						sb.Append(operation.GetType().Name.ToLower().PadLeft(4));

						var parameters = Enumerable.Range(0, operation.GetNumParameters()).Select(x => memory.ReadNext()).ToList();
						if (parameters.Count > 0)
						{
							var str = string.Join(' ', parameters.Select(x =>
							{
								if (memory.IsRegistry(x))
								{
									return $"${memory.ToRegistry(x)}";
								}
								if (operation.GetType() == typeof(Out))
								{
									return Convert.ToChar(x).ToString().Replace("\n", "\\n");
								}
								return x.ToString().PadLeft(memoryPadLength, '0');
							}));
							sb
								.Append(' ')
								.Append(str);
						}
					}
				}
				else
				{
					sb
						.Append($"{currentMemAdress.ToString().PadLeft(memoryPadLength, '0')}")
						.Append(" : ")
						.Append(opCode);
				}
				sb.AppendLine();
			}
			while (memory.GetAddressPointer() < short.MaxValue);

			File.WriteAllText("./program.txt", string.Join(',', sb.ToString()));
		}

		public static void PrintString(StringBuilder sb, IVirtualMemory memory)
		{
			var str = new StringBuilder();
			var start = memory.GetAddressPointer() - 1;
			memory.ChangeAddressPointer((ushort)(start));
			while(true)
			{
				var opCode = memory.ReadNext();
				if (opCode != 19)
				{
					memory.ChangeAddressPointer((ushort)(memory.GetAddressPointer() - 1));
					break;
				}
				var value = memory.ReadNext();
				if(memory.IsRegistry(value))
				{
					str.Append($"${memory.ToRegistry(value)}");
				}
				else
				{
					var c = Convert.ToChar(value);
					str.Append(c);
				}

			}
			int padlen = short.MaxValue.ToString().Length;
			sb
				.Append($"{start.ToString().PadLeft(padlen, '0')}")
				.Append(" : ")
				.Append("out ")
				.AppendLine(str.ToString().Replace("\n", "\\n"));
		}
	}
}
