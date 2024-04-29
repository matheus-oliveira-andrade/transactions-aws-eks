using MediatR;
using Movements.Domain.Entities;

namespace Movements.Application.Queries;

public record GetAccountMovementsReportQuery(string AccountId) : IRequest<MovementReport>;