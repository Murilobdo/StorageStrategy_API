﻿using StorageStrategy.Domain.Commands.Employee;
using StorageStrategy.Domain.Commands.Products;
using StorageStrategy.Models;
using Xunit;

namespace StorageStrategy.Tests.Commands.Employee
{
    public class CreateEmployeeCommandTest : CommandBaseTest
    {
        public CreateEmployeeCommandTest()
        {

        }

        [Fact]
        public void Sucesso_ao_criar_um_funcionario()
        {
            CreateEmployeeCommand createEmplyee = new("Nome", 10, EmployeeRole.Employee, "email@teste.com", "Senha", true, 1);
            Assert.True(createEmplyee.IsValid());
        }

        [Fact]
        public void Erro_ao_criar_um_funcionario_sem_nome()
        {
            CreateEmployeeCommand createEmplyee = new(string.Empty, 10, EmployeeRole.Employee, "email@teste.com", "Senha", true, 1);

            Assert.True(MensagemDeErroExistente(createEmplyee.GetErros(), "O Nome e obrigatório"));
        }

        [Fact]
        public void Erro_ao_criar_um_funcionario_sem_companyId()
        {
            CreateEmployeeCommand createEmplyee = new("Funcionario", 10, EmployeeRole.Employee, "email@teste.com", "Senha", true, 0);


            Assert.True(MensagemDeErroExistente(createEmplyee.GetErros(), "O Id da empresa e obrigatório"));
        }

        [Fact]
        public void Erro_ao_criar_um_funcionario_sem_senha()
        {
            CreateEmployeeCommand createEmplyee = new("Funcionario", 10, EmployeeRole.Employee, "email@teste.com", string.Empty, true, 1);

            Assert.True(MensagemDeErroExistente(createEmplyee.GetErros(), "A Senha e obrigatório"));
        }

        [Fact]
        public void Erro_ao_criar_um_funcionario_sem_email()
        {
            CreateEmployeeCommand createEmplyee = new("Funcionario", 10, EmployeeRole.Employee, string.Empty, "Senha", true, 1);

            Assert.True(MensagemDeErroExistente(createEmplyee.GetErros(), "O Email e obrigatório"));
        }
    }
}
