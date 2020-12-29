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
			var serviceProvider = BuildServiceProvider();
			var vm = serviceProvider.GetService<VirtualMachine>();
			var programBinary = ProgramParser.BinaryToProgram("./Programs/challenge.bin");
			//var programCompiled = ProgramParser.CompileProgram("./program.txt");
			vm.LoadProgram(programBinary);
			// vm.ExportProgram();
			vm.Run();
		}
		public static IServiceProvider BuildServiceProvider()
		{
			IServiceCollection serviceCollection = new ServiceCollection();
			RegisterServices(serviceCollection);
			return serviceCollection.BuildServiceProvider();
		}
		public static IServiceCollection RegisterServices(IServiceCollection services) => services
			.AddSingleton<VirtualMachine>()
			.AddSingleton<IVirtualMemory, VirtualMemory>()
			.AddVmOperations()
			.AddSerilog();

	}
}
