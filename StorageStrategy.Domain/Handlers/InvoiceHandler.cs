

// using MediatR;
// using StorageStrategy.Domain.Commands.NFE;
// using StorageStrategy.Domain.Services.Refit;
// using StorageStrategy.Models;

// namespace StorageStrategy.Domain.Handlers
// {
//     public class InvoiceHandler : HandlerBase,
//         IRequestHandler<CreateNFCommand, Result>,
//         IRequestHandler<SearchNFQuery, Result>
//     {
//         private readonly ISefazService _sefaz;

//         public InvoiceHandler(ISefazService sefazService)
//         {
//             _sefaz = sefazService;
//         }

//         public async Task<Result> Handle(CreateNFCommand request, CancellationToken cancellationToken)
//         {
//             if (!request.IsValid())
//                 return CreateError(request.GetErros(), "Dados inválidos");

//             var result = await _sefaz.CreateNFCe(request);

//             return CreateResponse(result, "Envio realizado");
//         }

//         public async Task<Result> Handle(SearchNFQuery request, CancellationToken cancellationToken)
//         {
//             var result = await _sefaz.GetNFCe(request.Id);

//             return CreateResponse(result, "Nota encontrada");
//         }
//     }
// }
