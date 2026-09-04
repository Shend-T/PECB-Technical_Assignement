namespace backend.DTOs.Error;

public enum ErrorCode
{
    InvalidStatus,
    InvalidStatusTransition,
    TicketClosed,
    AgentNotFound,
    TicketNotFound,
    AgentInactive,
    AgentNotAssigned,
    InvalidDepartment,
    InvalidPriority,
    DuplicateInstance
}