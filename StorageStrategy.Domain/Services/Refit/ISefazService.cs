using Refit;
using StorageStrategy.Domain.Commands.NFE;

namespace StorageStrategy.Domain.Services.Refit
{
    public interface ISefazService
    {
        [Post("/nfce/create")]
        [Headers("Authorization: Bearer")]
        Task<bool> CreateNFCe(CreateNFCommand request);

        [Put("/nfce/update")]
        [Headers("Authorization: Bearer")]
        Task UpdateNFCe();

        [Delete("/nfce/delete")]
        [Headers("Authorization: Bearer")]
        Task DeleteNFCe();

        [Get("/nfce/get?id={idNFCe}")]
        [Headers("Authorization: Bearer")]
        Task<bool> GetNFCe(Guid id);

        [Get("/nfce/get/nota-pdf?id={idNFCe}")]
        [Headers("Authorization: Bearer")]
        Task GenerateNotePDF();

        [Get("/nfce/get/dunfe-pdf?id={idNFCe}")]
        [Headers("Authorization: Bearer")]
        Task GenerateDunfePDF();
    }
}
