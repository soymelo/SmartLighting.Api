namespace SmartLighting.Api.Domain.Hardware;

public static class Esp32Pinout
{
    public static int GetGpio(SensorSlot slot) => slot switch
    {
        SensorSlot.Sensor1 => 32,
        SensorSlot.Sensor2 => 34,
        SensorSlot.Sensor3 => 35,
        _ => throw new ArgumentOutOfRangeException(nameof(slot), slot, "Slot de sensor inválido.")
    };

    public static int GetGpio(LuminaireSlot slot) => slot switch
    {
        LuminaireSlot.Led1 => 18,
        LuminaireSlot.Led2 => 19,
        LuminaireSlot.Led3 => 21,
        LuminaireSlot.Led4 => 25,
        LuminaireSlot.Led5 => 26,
        LuminaireSlot.Led6 => 27,
        _ => throw new ArgumentOutOfRangeException(nameof(slot), slot, "Slot de luminária inválido.")
    };
}
