using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Sia.Gateway.Tests.TestDoubles
{
    public class MockConfig : IConfigurationRoot
    {
        private static Dictionary<string, string> _configProperties;
        public MockConfig(Dictionary<string, string> props)
        {
            _configProperties = props;
        }

        public IConfigurationSection GetSection(string key)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IConfigurationSection> GetChildren()
        {
            throw new NotImplementedException();
        }

        public IChangeToken GetReloadToken()
        {
            throw new NotImplementedException();
        }

        public string this[string key]
        {
            get
            {
                string result;
                _configProperties.TryGetValue(key, out result);
                return result;
    }
            set { throw new NotImplementedException(); }
        }

        public void Reload()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IConfigurationProvider> Providers { get; }
    }
}
