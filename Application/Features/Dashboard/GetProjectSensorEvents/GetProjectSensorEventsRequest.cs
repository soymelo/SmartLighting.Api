using FastEndpoints;
using FluentValidation;

namespace SmartLighting.Api.Application.Features.Dashboard.GetProjectSensorEvents;

public sealed class GetProjectSensorEventsRequest
{
    public Guid ProjectId { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string SortBy { get; set; } = "occurredAt";
    public bool Descending { get; set; } = true;
}

public sealed class GetProjectSensorEventsValidator : Validator<GetProjectSensorEventsRequest>
{
    public GetProjectSensorEventsValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty();
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
        RuleFor(x => x.SortBy).Must(value => value is "occurredAt" or "sensorName" or "zoneName" or "deviceName");
    }
}
