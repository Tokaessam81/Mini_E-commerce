using E_commerce.Application.Persistence;
using E_commerce.Application.Service.Contract;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Tests.IntegrationTests.Helpers
{
    public class TestFixture
    {
        public Mock<IUnitOfWork> MockUnitOfWork { get; private set; }
        public Mock<ITokenService> MockTokenService { get; private set; }

        public TestFixture()
        {
            MockUnitOfWork = new Mock<IUnitOfWork>();
            MockTokenService = new Mock<ITokenService>();

        }

    }
}
