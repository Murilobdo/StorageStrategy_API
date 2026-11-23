using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using StorageStrategy.API.Configuration;
using StorageStrategy.API.Controllers;
using Xunit;

namespace StorageStrategy.Tests.Security
{
    /// <summary>
    /// US3 (FR-009 a FR-011): com a FallbackPolicy, o único jeito de abrir um endpoint é um
    /// [AllowAnonymous] explícito. Este teste trava a lista pública: incluir um endpoint público novo
    /// exige alterar a lista abaixo (decisão explícita e revisável).
    /// </summary>
    public class AnonymousEndpointsTest
    {
        private static readonly HashSet<string> AllowedAnonymous = new()
        {
            "AccountController.LoginAsync",
            "MercadoPagoWebhookController.Receive",
        };

        private static IEnumerable<(Type Controller, MethodInfo Action)> AllActions() =>
            typeof(AuthorizationSetup).Assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && typeof(ControllerBase).IsAssignableFrom(t))
                .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .Where(m => m.GetCustomAttributes<HttpMethodAttribute>(inherit: true).Any())
                    .Select(m => (t, m)));

        private static bool IsAnonymous(Type controller, MethodInfo action) =>
            action.GetCustomAttributes(inherit: true).OfType<IAllowAnonymous>().Any()
            || controller.GetCustomAttributes(inherit: true).OfType<IAllowAnonymous>().Any();

        [Fact(DisplayName = "Só login e webhook do Mercado Pago são acessíveis sem login")]
        public void Somente_endpoints_da_lista_publica_sao_anonimos()
        {
            var actions = AllActions().ToList();
            Assert.True(actions.Count > 50, $"Esperava encontrar os endpoints da API, encontrou {actions.Count}.");

            var anonymous = actions
                .Where(a => IsAnonymous(a.Controller, a.Action))
                .Select(a => $"{a.Controller.Name}.{a.Action.Name}")
                .ToHashSet();

            var unexpected = anonymous.Except(AllowedAnonymous).ToList();
            var missing = AllowedAnonymous.Except(anonymous).ToList();

            Assert.True(unexpected.Count == 0 && missing.Count == 0,
                $"Anônimos inesperados: [{string.Join(", ", unexpected)}]; faltando na lista pública: [{string.Join(", ", missing)}]");
        }

        [Fact(DisplayName = "Webhook do Mercado Pago continua anônimo na mesma rota")]
        public void Webhook_mercado_pago_continua_publico()
        {
            var controller = typeof(MercadoPagoWebhookController);
            var action = controller.GetMethod(nameof(MercadoPagoWebhookController.Receive))!;

            Assert.Equal("api/webhooks/mercadopago", controller.GetCustomAttribute<RouteAttribute>()!.Template);
            Assert.Equal("{companyId:int}", action.GetCustomAttribute<HttpPostAttribute>()!.Template);
            Assert.True(IsAnonymous(controller, action));
        }
    }
}
