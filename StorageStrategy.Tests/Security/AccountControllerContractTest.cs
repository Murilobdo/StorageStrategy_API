using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using StorageStrategy.API.Controllers;
using StorageStrategy.Domain.Commands.Login;
using Xunit;

namespace StorageStrategy.Tests.Security
{
    /// <summary>US1 (C-1): a identidade da troca de senha vem só do token.</summary>
    public class AccountControllerContractTest
    {
        [Fact(DisplayName = "UserId e CompanyId do ChangePasswordCommand são ignorados na desserialização")]
        public void Ids_do_corpo_sao_ignorados()
        {
            Assert.NotNull(typeof(ChangePasswordCommand).GetProperty(nameof(ChangePasswordCommand.UserId))!
                .GetCustomAttribute<JsonIgnoreAttribute>());
            Assert.NotNull(typeof(ChangePasswordCommand).GetProperty(nameof(ChangePasswordCommand.CompanyId))!
                .GetCustomAttribute<JsonIgnoreAttribute>());

            const string body = """{"userId":20,"companyId":2,"currentPassword":"a","newPassword":"b"}""";
            var command = JsonSerializer.Deserialize<ChangePasswordCommand>(body, new JsonSerializerOptions(JsonSerializerDefaults.Web))!;

            Assert.Equal(0, command.UserId);
            Assert.Equal(0, command.CompanyId);
            Assert.Equal("a", command.CurrentPassword);
            Assert.Equal("b", command.NewPassword);
        }

        [Fact(DisplayName = "AccountController não é anônimo; só o login é")]
        public void Account_controller_so_login_anonimo()
        {
            var controller = typeof(AccountController);

            Assert.Empty(controller.GetCustomAttributes<AllowAnonymousAttribute>(inherit: true));
            Assert.Empty(controller.GetMethod(nameof(AccountController.ChangePassword))!
                .GetCustomAttributes<AllowAnonymousAttribute>(inherit: true));
            Assert.NotEmpty(controller.GetMethod(nameof(AccountController.LoginAsync))!
                .GetCustomAttributes<AllowAnonymousAttribute>(inherit: true));
        }
    }
}
