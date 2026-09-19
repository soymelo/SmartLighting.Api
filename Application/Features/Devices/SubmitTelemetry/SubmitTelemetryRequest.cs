using FastEndpoints;
using FluentValidation;

namespace SmartLighting.Api.Application.Features.Devices.SubmitTelemetry;

public sealed record SubmitTelemetryRequest
(
    DateTime SentAtUtc,
    IReadOnlyCollection<SubmitTelemetryRequest.SensorEventItem> SensorEvents,
    IReadOnlyCollection<SubmitTelemetryRequest.LuminaireStateChangeItem> LuminaireStateChanges
)
{

    public sealed record SensorEventItem
    (
        Guid EventIdentifier,
        Guid SensorId,
        DateTime OccurredAtUtc
    );

    public sealed record LuminaireStateChangeItem
    (
        Guid EventIdentifier,
        Guid LuminaireId,
        int BrightnessPercentage,
        DateTime ChangedAtUtc
    );
}

public sealed class SubmitTelemetryValidator : Validator<SubmitTelemetryRequest>
{
    private const int MaximumTelemetryItemsPerBatch = 50;

    public SubmitTelemetryValidator()
    {
        RuleFor(x => x.SentAtUtc)
            .NotEmpty()
            .Must(BeUtc)
            .WithMessage("A data de envio deve estar em UTC.");

        RuleFor(x => x)
            .Must(HaveValidBatchSize)
            .WithMessage($"O lote pode conter no máximo {MaximumTelemetryItemsPerBatch} itens de telemetria.");

        RuleFor(x => x.SensorEvents)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithMessage("A lista de eventos de sensor é obrigatória.")
            .Must(HasDistinctEventIdentifiers)
            .WithMessage("Os eventos de sensor não podem repetir EventIdentifier no mesmo lote.");

        RuleFor(x => x.LuminaireStateChanges)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithMessage("A lista de mudanças de estado de luminária é obrigatória.")
            .Must(HasDistinctEventIdentifiers)
            .WithMessage("As mudanças de estado de luminária não podem repetir EventIdentifier no mesmo lote.");

        RuleForEach(x => x.SensorEvents).ChildRules(sensorEvent =>
        {
            sensorEvent.RuleFor(x => x.EventIdentifier).NotEmpty();
            sensorEvent.RuleFor(x => x.SensorId).NotEmpty();
            sensorEvent.RuleFor(x => x.OccurredAtUtc)
                .NotEmpty()
                .Must(BeUtc)
                .WithMessage("A data da detecção deve estar em UTC.");
        });

        RuleForEach(x => x.LuminaireStateChanges).ChildRules(stateChange =>
        {
            stateChange.RuleFor(x => x.EventIdentifier).NotEmpty();
            stateChange.RuleFor(x => x.LuminaireId).NotEmpty();
            stateChange.RuleFor(x => x.BrightnessPercentage)
                .InclusiveBetween(0, 100);
            stateChange.RuleFor(x => x.ChangedAtUtc)
                .NotEmpty()
                .Must(BeUtc)
                .WithMessage("A data da mudança de estado deve estar em UTC.");
        });
    }

    private static bool BeUtc(DateTime value) => value.Kind == DateTimeKind.Utc;

    private static bool HaveValidBatchSize(SubmitTelemetryRequest request)
    {
        if (request.SensorEvents is null || request.LuminaireStateChanges is null)
        {
            return true;
        }

        return request.SensorEvents.Count + request.LuminaireStateChanges.Count <= MaximumTelemetryItemsPerBatch;
    }

    private static bool HasDistinctEventIdentifiers(
        IReadOnlyCollection<SubmitTelemetryRequest.SensorEventItem> sensorEvents) =>
        sensorEvents.Select(sensorEvent => sensorEvent.EventIdentifier).Distinct().Count() == sensorEvents.Count;

    private static bool HasDistinctEventIdentifiers(
        IReadOnlyCollection<SubmitTelemetryRequest.LuminaireStateChangeItem> stateChanges) =>
        stateChanges.Select(stateChange => stateChange.EventIdentifier).Distinct().Count() == stateChanges.Count;
}
