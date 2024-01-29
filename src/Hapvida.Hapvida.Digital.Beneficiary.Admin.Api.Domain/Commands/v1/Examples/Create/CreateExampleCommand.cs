using Hapvida.Core.Domain.Models;

namespace Hapvida.Hapvida.Digital.Beneficiary.Admin.Api.Domain.Commands.v1.Examples.Create;

public sealed class CreateExampleCommand : Command
{
    public string? CustomField { get; set; }
}