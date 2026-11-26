using StorageStrategy.Models;
using StorageStrategy.Tests.FakeRepository;
using Xunit;

namespace StorageStrategy.Tests.Handler
{
    public class PixChargeExpirationTest
    {
        [Fact(DisplayName = "Cobrança Pendente Vencida deve Transicionar para Expirado")]
        public async Task Cobranca_pendente_vencida_deve_expirar()
        {
            var repo = new FakePixChargeRepository();
            var vencida = new PixChargeEntity
            {
                PixChargeId = 1,
                CompanyId = 1,
                CommandId = 1,
                Status = PixChargeStatusEnum.AguardandoPagamento,
                ExpiraEm = DateTime.Now.AddMinutes(-1)
            };
            repo.charges.Add(vencida);

            var expiradas = await repo.GetExpiredPendingAsync();
            foreach (var charge in expiradas)
                charge.Status = PixChargeStatusEnum.Expirado;

            Assert.Single(expiradas);
            Assert.Equal(PixChargeStatusEnum.Expirado, vencida.Status);
        }

        [Fact(DisplayName = "Cobrança já Paga não deve ser Afetada pela Expiração")]
        public async Task Cobranca_paga_nao_deve_ser_afetada()
        {
            var repo = new FakePixChargeRepository();
            var paga = new PixChargeEntity
            {
                PixChargeId = 1,
                CompanyId = 1,
                CommandId = 1,
                Status = PixChargeStatusEnum.Pago,
                ExpiraEm = DateTime.Now.AddMinutes(-1)
            };
            repo.charges.Add(paga);

            var expiradas = await repo.GetExpiredPendingAsync();

            Assert.Empty(expiradas);
            Assert.Equal(PixChargeStatusEnum.Pago, paga.Status);
        }

        [Fact(DisplayName = "Cobrança Pendente ainda Válida não deve ser Expirada")]
        public async Task Cobranca_pendente_valida_nao_deve_expirar()
        {
            var repo = new FakePixChargeRepository();
            var pendente = new PixChargeEntity
            {
                PixChargeId = 1,
                CompanyId = 1,
                CommandId = 1,
                Status = PixChargeStatusEnum.AguardandoPagamento,
                ExpiraEm = DateTime.Now.AddHours(1)
            };
            repo.charges.Add(pendente);

            var expiradas = await repo.GetExpiredPendingAsync();

            Assert.Empty(expiradas);
        }
    }
}
