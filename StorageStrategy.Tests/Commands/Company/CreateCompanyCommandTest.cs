using StorageStrategy.Domain.Commands.Company;
using System.Net;
using Xunit;

namespace StorageStrategy.Tests.Commands.Company
{
    public class CreateCompanyCommandTest : CommandBaseTest
    {
        [Fact(DisplayName = "Sucesso ao Criar uma Empresa")]
        public void Sucesso_ao_criar_uma_empresa()
        {
            CreateCompanyCommand createCompany = new(
                companyId: 1,
                name:"Nome da Empresa",
                description: "Descrição da Empresa",
                isActive: true, 
                createAt: "3/3/2024", 
                validate: "3/4/2024",
                cNPJ: "123618725",
                phone: "12312345",
                address: "Rua do teste",
                adminUserName: "nome qualquer",
                adminUserEmail: "admin@hotmail.com",
                password: "123456"
            );

            Assert.True(createCompany.IsValid());
        }

        [Fact(DisplayName = "Erro ao Criar uma Empresa sem Nome")]
        public void Erro_ao_criar_uma_empresa_sem_nome()
        {
            CreateCompanyCommand createCompany = new(
                companyId: 1,
                name: string.Empty,
                description: "Descrição da Empresa",
                isActive: true,
                createAt: "3/3/2024",
                validate: "3/4/2024",
                cNPJ: "123618725",
                phone: "12312345",
                address: "Rua do teste",
                adminUserName: "nome qualquer",
                adminUserEmail: "admin@hotmail.com",
                password: "123456"
            );

            Assert.True(MensagemDeErroExistente(createCompany.GetErros(), "O Nome e obrigatório"));
        }

        [Fact(DisplayName = "Erro ao Criar uma Empresa sem Descricao")]
        public void Erro_ao_criar_uma_empresa_sem_descricao()
        {
            CreateCompanyCommand createCompany = new(
                companyId: 1,
                name: "Nome da Empresa",
                description: string.Empty,
                isActive: true,
                createAt: "3/3/2024",
                validate: "3/4/2024",
                cNPJ: "123618725",
                phone: "12312345",
                address: "Rua do teste",
                adminUserName: "nome qualquer",
                adminUserEmail: "admin@hotmail.com",
                password: "123456"
            );

            Assert.True(MensagemDeErroExistente(createCompany.GetErros(), "A Descrição e obrigatório"));
        }

        [Fact(DisplayName = "Erro ao Criar uma Empresa sem Data de Criação")]
        public void Erro_ao_criar_uma_empresa_sem_data_de_criacao()
        {
            CreateCompanyCommand createCompany = new(
                companyId: 1,
                name: "Nome da Empresa",
                description: "Descrição da Empresa",
                isActive: true,
                createAt: string.Empty,
                validate: "3/4/2024",
                cNPJ: "123618725",
                phone: "12312345",
                address: "Rua do teste",
                adminUserName: "nome qualquer",
                adminUserEmail: "admin@hotmail.com",
                password: "123456"
            );

            Assert.True(MensagemDeErroExistente(createCompany.GetErros(), "A Data de criação e obrigatório"));
        }

        [Fact(DisplayName = "Erro ao Criar uma Empresa sem Data Final da Licenca")]
        public void Erro_ao_criar_uma_empresa_sem_data_de_validade()
        {
            CreateCompanyCommand createCompany = new(
                companyId: 1,
                name: "Nome da Empresa",
                description: "Descrição da Empresa",
                isActive: true,
                createAt: "3/4/2024",
                validate: string.Empty,
                cNPJ: "123618725",
                phone: "12312345",
                address: "Rua do teste",
                adminUserName: "nome qualquer",
                adminUserEmail: "admin@hotmail.com",
                password: "123456"
            );

            Assert.True(MensagemDeErroExistente(createCompany.GetErros(), "A Data de validade e obrigatório"));
        }

        [Fact(DisplayName = "Erro ao Criar uma Empresa sem CNPJ")]
        public void Erro_ao_criar_uma_empresa_sem_data_de_cnpj()
        {
            CreateCompanyCommand createCompany = new(
                companyId: 1,
                name: "Nome da Empresa",
                description: "Descrição da Empresa",
                isActive: true,
                createAt: "3/4/2024",
                validate: "3/4/2024",
                cNPJ: string.Empty,
                phone: "12312345",
                address: "Rua do teste",
                adminUserName: "nome qualquer",
                adminUserEmail: "admin@hotmail.com",
                password: "123456"
            );

            Assert.True(MensagemDeErroExistente(createCompany.GetErros(), "O CNPJ e obrigatório"));
        }

        [Fact(DisplayName = "Erro ao Criar uma Empresa sem Telefone")]
        public void Erro_ao_criar_uma_empresa_sem_data_de_telefone()
        {
            CreateCompanyCommand createCompany = new(
                companyId: 1,
                name: "Nome da Empresa",
                description: "Descrição da Empresa",
                isActive: true,
                createAt: "3/4/2024",
                validate: "3/4/2024",
                cNPJ: "12312345",
                phone: string.Empty,
                address: "Rua do teste",
                adminUserName: "nome qualquer",
                adminUserEmail: "admin@hotmail.com",
                password: "123456"
            );

            Assert.True(MensagemDeErroExistente(createCompany.GetErros(), "O Telefone e obrigatório"));
        }

        [Fact(DisplayName = "Erro ao Criar uma Empresa sem Endereço")]
        public void Erro_ao_criar_uma_empresa_sem_data_de_endereco()
        {
            CreateCompanyCommand createCompany = new(
                companyId: 1,
                name: "Nome da Empresa",
                description: "Descrição da Empresa",
                isActive: true,
                createAt: "3/4/2024",
                validate: "3/4/2024",
                cNPJ: "12312345",
                phone: "123758123",
                address: string.Empty,
                adminUserName: "nome qualquer",
                adminUserEmail: "admin@hotmail.com",
                password: "123456"
            );

            Assert.True(MensagemDeErroExistente(createCompany.GetErros(), "O Endereço e obrigatório"));
        }

    }
}