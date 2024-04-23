using Explorer.Encounters.API.Dtos;
using FluentResults;

namespace Explorer.Encounters.API.Internal
{
    public interface IInternalEncounterExecutionService
    {
        Result<List<EncounterExecutionDto>> GetByEncounter(long encounterId);
    }
}
