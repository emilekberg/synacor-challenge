using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Serilog;
using synacor_challange.Instructions;
using synacor_challange.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace synacor_challange.Extensions
{
	public static class ServiceCollectionExtension
	{
		public static IServiceCollection AddSerilog(this IServiceCollection serviceCollection)
		{
			File.WriteAllText(@"./log.txt", string.Empty);
			var log = new LoggerConfiguration()
				// .MinimumLevel.Debug()
				.WriteTo.File("log.txt")
			#if DEBUG
				// .WriteTo.Debug()
			#endif
				.CreateLogger();
			serviceCollection.AddSingleton<ILogger>(log);
			return serviceCollection;
		}
		/// <summary>
		/// Adds all VirtualMachine operations (implementing IOperations) to the ServiceCollection.
		/// </summary>
		/// <param name="serviceCollection"></param>
		/// <returns></returns>
		public static IServiceCollection AddVmOperations(this IServiceCollection serviceCollection)
		{
			var baseType = typeof(IOperation);
			var types = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(s => s.GetTypes())
				.Where(p => baseType.IsAssignableFrom(p))
				.Where(p => p != baseType)
				.ToList();
			types.ForEach(type =>
			{
				var serviceDescriptor = new ServiceDescriptor(typeof(IOperation), type, ServiceLifetime.Singleton);
				serviceCollection.TryAddEnumerable(serviceDescriptor);
			});
			return serviceCollection;
		}
	}
}
