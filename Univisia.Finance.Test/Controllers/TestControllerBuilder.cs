using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Univisia.Finance.Web.Controllers;

namespace Univisia.Finance.Test.Controllers
{
    public class TestControllerBuilder
    {
        private ClaimsIdentity _identity;
        private ClaimsPrincipal _user;
        private ControllerContext _controllerContext;

        public TestControllerBuilder()
        {
            _identity = new ClaimsIdentity();
            _user = new ClaimsPrincipal(_identity);
            _controllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = _user } };
        }

        public TestControllerBuilder WithClaims(IDictionary<string, string> claims)
        {
            _identity.AddClaims(claims.Select(c => new Claim(c.Key, c.Value)));

            return this;
        }

        public TestControllerBuilder WithIdentity(string userId, string userName)
        {
            _identity.AddClaims(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, userName)
            });

            return this;
        }

        public TestControllerBuilder WithDefaultIdentityClaims()
        {
            _identity.AddClaims(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "testId"),
                new Claim(ClaimTypes.Name, "testName")
            });

            return this;
        }

        public T Build<T>(Func<T> create)where T : BaseController
        {
            var controller = create();

            controller.ControllerContext = _controllerContext;
            
            return controller;
        }
    }
}
