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
                validate: DateTime.Now,
                adminUserEmail: "admin@hotmail.com",
                password: "123456"
            );

            Assert.True(createCompany.IsValid());
        }

        [Fact]
        public void Erro_ao_criar_uma_empresa_sem_nome()
        {
            CreateCompanyCommand createCompany = new(
                companyId: 1,
                name: string.Empty,
                description: "Descrição da Empresa",
                isActive: true, 
                createAt: DateTime.Now, 
                validate: DateTime.Now,
                adminUserEmail: "admin@hotmail.com",
                password: "123456"
            );

            Assert.True(MensagemDeErroExistente(createCompany.GetErros(), "O Nome e obrigatório"));
        }

        [Fact]
        public void Erro_ao_criar_uma_categoria_sem_descricao()
        {
            CreateCompanyCommand createCompany = new(
                companyId: 1,
                name: "Nome da Empresa",
                description: string.Empty,
                isActive: true, 
                createAt: DateTime.Now, 
                validate: DateTime.Now,
                adminUserEmail: "admin@hotmail.com",
                password: "123456"
            );

            Assert.True(MensagemDeErroExistente(createCompany.GetErros(), "A Descrição e obrigatório"));
        }

        [Fact]
        public void Erro_ao_criar_uma_categoria_sem_data_de_criacao()
        {
            CreateCompanyCommand createCompany = new(
                companyId: 1,
                name: "Nome da Empresa",
                description: "Descrição da Empresa",
                isActive: true, 
                createAt: DateTime.MinValue, 
                validate: DateTime.Now,
                adminUserEmail: "admin@hotmail.com",
                password: "123456"
            );

            Assert.True(MensagemDeErroExistente(createCompany.GetErros(), "A Data de criação e obrigatório"));
        }

        [Fact]
        public void Erro_ao_criar_uma_categoria_sem_data_de_validade()
        {
            CreateCompanyCommand createCompany = new(
                companyId: 1,
                name: "Nome da Empresa",
                description: "Descrição da Empresa",
                isActive: true, 
                createAt: DateTime.Now, 
                validate: DateTime.MinValue,
                adminUserEmail: "admin@hotmail.com",
                password: "123456"
            );

            Assert.True(MensagemDeErroExistente(createCompany.GetErros(), "A Data de validade e obrigatório"));
        }

    }
}