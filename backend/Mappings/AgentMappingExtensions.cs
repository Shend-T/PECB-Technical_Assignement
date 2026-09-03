using backend.Models;
using backend.DTOs.Agent;

namespace backend.Mappings;

public static class AgentMappingExtensions
{
    public static AgentDto ToDto(this Agent agent) => new ()
    {
        id         = agent.id,
        fullName   = agent.fullName,
        email      = agent.email,
        department = agent.department,
        active     = agent.active
    };
}