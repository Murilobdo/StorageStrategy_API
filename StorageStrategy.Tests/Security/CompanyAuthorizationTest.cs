using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using StorageStrategy.API.Configuration;
using StorageStrategy.API.Controllers;
using Xunit;

namespace StorageStrategy.Tests.Security
{
    /// <summary>US2 (C-2): só o admin da plataforma administra empresas; GetById segue para qualquer autenticado.</summary>
    public class CompanyAuthorizationTest
    {
        private static readonly Type Controller = typeof(CompanyController);

        private static AuthorizeAttribute[] AuthorizeOf(string action) =>
            Controller.GetMethod(action)!.GetCustomAttributes<AuthorizeAttribute>(inherit: true).ToArray();

        [Fact(DisplayName = "CompanyController exige login na classe, sem policy nem roles")]
        public void Classe_exige_login()
        {
            var attributes = Controller.GetCustomAttributes<AuthorizeAttribute>(inherit: true).ToArray();

            Assert.Single(attributes);
            Assert.Null(attributes[0].Policy);
            Assert.Null(attributes[0].Roles);
        }

        [Theory(DisplayName = "Operações de administração exigem a política PlatformAdmin")]
        [InlineData(nameof(CompanyController.Get))]
        [InlineData(nameof(CompanyController.Post))]
        [InlineData(nameof(CompanyController.Inactivate))]
        [InlineData(nameof(CompanyController.Renovate))]
        public void Operacoes_exigem_platform_admin(string action)
        {
            Assert.Contains(AuthorizeOf(action), a => a.Policy == AuthorizationSetup.PlatformAdminPolicy);
        }

        [Fact(DisplayName = "GetById não exige PlatformAdmin (o Site usa para a própria empresa)")]
        public void Get_by_id_para_qualquer_autenticado()
        {
            Assert.All(AuthorizeOf(nameof(CompanyController.GetById)), a =>
            {
                Assert.Null(a.Policy);
                Assert.Null(a.Roles);
            });
        }

        [Fact(DisplayName = "Nenhuma ação do CompanyController é anônima")]
        public void Nenhuma_acao_anonima()
        {
            Assert.Empty(Controller.GetCustomAttributes<AllowAnonymousAttribute>(inherit: true));
            foreach (var method in Controller.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                Assert.Empty(method.GetCustomAttributes<AllowAnonymousAttribute>(inherit: true));
        }
    }
}
