-- Executar somente após aplicar as migrations, em um banco novo e vazio.
-- Não cria dispositivos: cadastre cada ESP32 pela aplicação Web.
SET XACT_ABORT ON;

BEGIN TRY
    BEGIN TRANSACTION;

    IF EXISTS (SELECT 1 FROM dbo.Projects) OR EXISTS (SELECT 1 FROM dbo.Zones)
        THROW 50001, 'O banco ja contem projetos ou zonas. Carga inicial cancelada.', 1;

    DECLARE @ProjectId uniqueidentifier = NEWID();

    INSERT INTO dbo.Projects (Id, Name)
    VALUES (@ProjectId, N'Maquete TCC');

    INSERT INTO dbo.Zones (Id, ProjectId, Name, DisplayOrder)
    VALUES (NEWID(), @ProjectId, N'Zone 1', 1),
           (NEWID(), @ProjectId, N'Zone 2', 2),
           (NEWID(), @ProjectId, N'Zone 3', 3),
           (NEWID(), @ProjectId, N'Zone 4', 4),
           (NEWID(), @ProjectId, N'Zone 5', 5),
           (NEWID(), @ProjectId, N'Zone 6', 6);

    COMMIT TRANSACTION;
    SELECT @ProjectId AS ProjectId;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;
