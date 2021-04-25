#nullable enable
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Maui.Hosting
{
	public interface IMauiServiceCollection : IServiceCollection
	{
		bool TryGetService(Type serviceType, out ServiceDescriptor? descriptor);

		IEnumerable<ServiceDescriptor> GetServices(Type serviceType);
	}
}