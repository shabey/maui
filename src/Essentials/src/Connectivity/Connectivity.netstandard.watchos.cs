using System.Collections.Generic;

namespace Microsoft.Maui.Networking
{
	/// <include file="../../docs/Microsoft.Maui.Essentials/Connectivity.xml" path="Type[@FullName='Microsoft.Maui.Essentials.Connectivity']/Docs" />
	partial class ConnectivityImplementation : IConnectivity
	{
		public NetworkAccess NetworkAccess =>
			throw ExceptionUtils.NotSupportedOrImplementedException;

		public IEnumerable<ConnectionProfile> ConnectionProfiles =>
			throw ExceptionUtils.NotSupportedOrImplementedException;

		public void StartListeners() =>
			throw ExceptionUtils.NotSupportedOrImplementedException;

		public void StopListeners() =>
			throw ExceptionUtils.NotSupportedOrImplementedException;
	}
}
