using AutoMapper;
using Hapvida.Hapvida.Digital.Beneficiary.Admin.Api.Domain.Entities.v1;

namespace Hapvida.Hapvida.Digital.Beneficiary.Admin.Api.Infra.Data.Queries.v1.Examples.GetAll;

public sealed class GetAllQueryProfile : Profile
{
    public GetAllQueryProfile()
    {
        CreateMap<Example, GetAllQueryResponseDetail>(MemberList.None);
    }
}