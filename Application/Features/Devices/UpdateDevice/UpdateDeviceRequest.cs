using FastEndpoints;
using FluentValidation;
using SmartLighting.Api.Domain.Hardware;

namespace SmartLighting.Api.Application.Features.Devices.UpdateDevice;

public sealed class UpdateDeviceRequest
{
    public Guid DeviceId { get; init; }
    public string Name { get; init; } = string.Empty;
    public IReadOnlyCollection<SensorItem> Sensors { get; init; } = [];
    public IReadOnlyCollection<LuminaireItem> Luminaires { get; init; } = [];

    public sealed record SensorItem(SensorSlot Slot, string Name, Guid ZoneId);

    public sealed record LuminaireItem(
        LuminaireSlot Slot,
        string Name,
        IReadOnlyCollection<Guid> ZoneIds,
        decimal NominalPowerWatts);
}

public sealed class UpdateDeviceValidator : Validator<UpdateDeviceRequest>
{
    private const int MaximumNameLength = 50;

    public UpdateDeviceValidator()
    {
        RuleFor(request => request.DeviceId).NotEmpty();
        RuleFor(request => request.Name).NotEmpty().MaximumLength(MaximumNameLength);

        RuleFor(request => request.Sensors)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .Must(sensors => sensors.Count <= 3)
            .WithMessage("Um dispositivo pode ter no máximo 3 sensores.")
            .Must(sensors => sensors.Select(sensor => sensor.Slot).Distinct().Count() == sensors.Count)
            .WithMessage("Os sensores não podem repetir o mesmo slot.");

        RuleFor(request => request.Luminaires)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .Must(luminaires => luminaires.Count <= 6)
            .WithMessage("Um dispositivo pode ter no máximo 6 luminárias.")
            .Must(luminaires => luminaires.Select(luminaire => luminaire.Slot).Distinct().Count() == luminaires.Count)
            .WithMessage("As luminárias não podem repetir o mesmo slot.");

        RuleForEach(request => request.Sensors).ChildRules(sensor =>
        {
            sensor.RuleFor(item => item.Slot).IsInEnum()
                .WithMessage("O slot do sensor deve estar entre 1 e 3.");
            sensor.RuleFor(item => item.Name).NotEmpty().MaximumLength(MaximumNameLength);
            sensor.RuleFor(item => item.ZoneId).NotEmpty();
        });

        RuleForEach(request => request.Luminaires).ChildRules(luminaire =>
        {
            luminaire.RuleFor(item => item.Slot).IsInEnum()
                .WithMessage("O slot da luminária deve estar entre 1 e 6.");
            luminaire.RuleFor(item => item.Name).NotEmpty().MaximumLength(MaximumNameLength);
            luminaire.RuleFor(item => item.ZoneIds)
                .NotNull()
                .Must(zoneIds => zoneIds is { Count: > 0 } &&
                                 zoneIds.All(zoneId => zoneId != Guid.Empty) &&
                                 zoneIds.Distinct().Count() == zoneIds.Count)
                .WithMessage("Informe zonas válidas e sem repetição para a luminária.");
            luminaire.RuleFor(item => item.NominalPowerWatts)
                .GreaterThan(0)
                .PrecisionScale(10, 2, false);
        });
    }
}
