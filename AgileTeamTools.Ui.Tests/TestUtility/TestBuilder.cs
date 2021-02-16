using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgileTeamTools.Ui.Tests.TestUtility
{
    public class TestBuilder
    {
        private readonly Bunit.TestContext _hostContext;

        public static TestBuilder Create()
        {
            var host = new Bunit.TestContext();
            return new TestBuilder(host, new ConfigurationBuilder());
        }

        private TestBuilder(Bunit.TestContext hostContext, ConfigurationBuilder configurationBuilder)
        {
            _hostContext = hostContext;

        }

        public TestContext TestContext => _hostContext.Services.GetService<TestContext>();

        public IRenderedComponent<TComponent> GetComponent<TComponent>(Dictionary<string, object> parameters = null)
            where TComponent : IComponent
        {
            var componentParameters = (parameters ?? new Dictionary<string, object>())
                .Select(p => ComponentParameter.CreateParameter(p.Key, p.Value))
                .ToList();

            return _hostContext.RenderComponent<TComponent>(componentParameters.ToArray());

        }
    }

    public class TestContext
    {
    }
}
