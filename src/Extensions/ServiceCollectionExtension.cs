using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using synacor_challange.Instructions;
using synacor_challange.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace synacor_challange.Extensions
{
	public static class ServiceCollectionExtension
	{
		/// <summary>
		/// Adds all VirtualMachine operations (implementing IOperations) to the ServiceCollection.
		/// </summary>
		/// <param name="serviceCollection"></param>
		/// <returns></returns>
		public static IServiceCollection AddVmOperations(this IServiceCollection serviceCollection)
		{

			var types = GetInstructionsFromAssemblies();
			types.ForEach(type =>
			{
				var serviceDescriptor = new ServiceDescriptor(typeof(IOperation), type, ServiceLifetime.Singleton);
				serviceCollection.TryAddEnumerable(serviceDescriptor);
			});
			return serviceCollection;
		}
		private static List<Type> GetInstructionsFromAssemblies()
		{
			var baseType = typeof(IOperation);
			var types = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(s => s.GetTypes())
				.Where(p => baseType.IsAssignableFrom(p))
				.Where(p => p != baseType);
			return types.ToList();
		}
	}
}
