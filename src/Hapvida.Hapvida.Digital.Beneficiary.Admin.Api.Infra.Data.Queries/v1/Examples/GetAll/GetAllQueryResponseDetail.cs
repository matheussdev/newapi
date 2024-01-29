using System;

namespace Hapvida.Hapvida.Digital.Beneficiary.Admin.Api.Infra.Data.Queries.v1.Examples.GetAll;

public sealed record GetAllQueryResponseDetail
{
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? CustomField { get; set; }
}