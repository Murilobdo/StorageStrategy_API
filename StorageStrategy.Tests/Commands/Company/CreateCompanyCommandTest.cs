using StorageStrategy.Domain.Commands.Company;
using Xunit;

namespace StorageStrategy.Tests.Commands.Company
{
    public class CreateCompanyCommandTest : CommandBaseTest
    {
        [Fact]
        public void Sucesso_ao_criar_uma_empresa()
        {
            CreateCompanyCommand createCompany = new(
                companyId: 1,
                name:"Nome da Empresa",
                description: "Descrição da Empresa",
                isActive: true, 
                createAt: DateTime.Now, 
                validate: DateTime.Now
            );

            Assert.True(createCompany.IsValid());
        }
    }
}