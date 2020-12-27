using Microsoft.Extensions.DependencyInjection;
using Serilog;
using synacor_challange.Extensions;
using synacor_challange.Interfaces;
using System;
using System.IO;
using System.Linq;
using synacor_challange.Parsers;

namespace synacor_challange
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.ForegroundColor = ConsoleColor.Green;
			// clear file if debug
			File.WriteAllText(@"./log.txt", string.Empty); 
			var log = new LoggerConfiguration()
				.WriteTo.File("log.txt")
#if DEBUG
				//.WriteTo.Debug()
#endif
				.CreateLogger();
			var serviceCollection = new ServiceCollection();

			serviceCollection.AddSingleton<ILogger>(log);

			RegisterServices(serviceCollection);

			var serviceProvider = serviceCollection.BuildServiceProvider();

			var vm = serviceProvider.GetService<VirtualMachine>();
#if DEBUG
			vm.DebugPrintOperations();
#endif
			var program = ProgramParser.BinaryToProgram("./Programs/challenge.bin");
			
			vm.LoadProgram(program);
			/*
			vm.SaveState();
			var program2 = ProgramParser.DisassembledToProgram("./program.txt");

			vm.LoadProgram(program);
			vm.SaveState();
			*/
			vm.Run();
		}

		public static void RegisterServices(ServiceCollection services) => services
			.AddSingleton<VirtualMachine>()
			.AddSingleton<IVirtualMemory, VirtualMemory>()
			.AddVmOperations();

	}
}
