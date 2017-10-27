using Sia.Shared.Authentication;

namespace Sia.Gateway.Tests.TestDoubles
{
    public class DummyAuthenticatedUserContext : AuthenticatedUserContext
    {
        public DummyAuthenticatedUserContext() : base(null, null, null)
        {
        }
    }
}
