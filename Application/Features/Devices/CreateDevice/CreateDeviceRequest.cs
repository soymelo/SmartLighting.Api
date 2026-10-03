using FastEndpoints;
using FluentValidation;
using SmartLighting.Api.Domain.Hardware;

namespace SmartLighting.Api.Application.Features.Devices.CreateDevice;

public sealed class CreateDeviceRequest
{
    public Guid ProjectId { get; init; }
    public string Name { get; init; } = string.Empty;
    public IReadOnlyCollection<SensorItem> Sensors { get; init; } = [];
    public IReadOnlyCollection<LuminaireItem> Luminaires { get; init; } = [];

    public sealed record SensorItem(
        SensorSlot Slot,
        string Name,
        Guid ZoneId);

    public sealed record LuminaireItem(
        LuminaireSlot Slot,
        string Name,
        Guid ZoneId,
        decimal NominalPowerWatts);
}

public sealed class CreateDeviceValidator : Validator<CreateDeviceRequest>
{
    private const int MaximumNameLength = 50;
    private const int MaximumSensors = 3;
    private const int MaximumLuminaires = 6;

    public CreateDeviceValidator()
    {
        RuleFor(request => request.ProjectId)
            .NotEmpty();

        RuleFor(request => request.Name)
            .NotEmpty()
            .MaximumLength(MaximumNameLength);

        RuleFor(request => request.Sensors)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .Must(sensors => sensors.Count <= MaximumSensors)
            .WithMessage($"Um dispositivo pode ter no máximo {MaximumSensors} sensores.")
            .Must(HaveDistinctSensorSlots)
            .WithMessage("Os sensores não podem repetir o mesmo slot.");

        RuleFor(request => request.Luminaires)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .Must(luminaires => luminaires.Count <= MaximumLuminaires)
            .WithMessage($"Um dispositivo pode ter no máximo {MaximumLuminaires} luminárias.")
            .Must(HaveDistinctLuminaireSlots)
            .WithMessage("As luminárias não podem repetir o mesmo slot.");

        RuleForEach(request => request.Sensors).ChildRules(sensor =>
        {
            sensor.RuleFor(item => item.Slot)
                .IsInEnum()
                .WithMessage("O slot do sensor deve estar entre 1 e 3.");
            sensor.RuleFor(item => item.Name)
                .NotEmpty()
                .MaximumLength(MaximumNameLength);
            sensor.RuleFor(item => item.ZoneId)
                .NotEmpty();
        });

        RuleForEach(request => request.Luminaires).ChildRules(luminaire =>
        {
            luminaire.RuleFor(item => item.Slot)
                .IsInEnum()
                .WithMessage("O slot da luminária deve estar entre 1 e 6.");
            luminaire.RuleFor(item => item.Name)
                .NotEmpty()
                .MaximumLength(MaximumNameLength);
            luminaire.RuleFor(item => item.ZoneId)
                .NotEmpty();
            luminaire.RuleFor(item => item.NominalPowerWatts)
                .GreaterThan(0)
                .PrecisionScale(10, 2, false);
        });
    }

    private static bool HaveDistinctSensorSlots(IReadOnlyCollection<CreateDeviceRequest.SensorItem> sensors) =>
        sensors.Select(sensor => sensor.Slot).Distinct().Count() == sensors.Count;

    private static bool HaveDistinctLuminaireSlots(
        IReadOnlyCollection<CreateDeviceRequest.LuminaireItem> luminaires) =>
        luminaires.Select(luminaire => luminaire.Slot).Distinct().Count() == luminaires.Count;
}
