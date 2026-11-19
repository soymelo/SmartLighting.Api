# SmartLighting.Api

API do protótipo SmartLighting. Gerencia os dispositivos e sua topologia,
recebe telemetria dos ESP32 e fornece os dados consultados pela aplicação Web.
A previsão de atividade e a escolha do brilho de repouso são executadas no
firmware, não nesta API.

## Executar localmente

Requer .NET SDK 10 e SQL Server. Configure
`ConnectionStrings:SmartLightingDatabase` para a sua instância SQL Server.
O valor em `appsettings.Development.json` é um exemplo para uma instância
local `SQLEXPRESS`; ajuste-o localmente ou use uma variável de ambiente.

```powershell
$env:ConnectionStrings__SmartLightingDatabase = "Server=localhost\SQLEXPRESS;Database=SmartLighting;Trusted_Connection=True;TrustServerCertificate=True"
dotnet restore
dotnet ef database update
dotnet run --launch-profile http
```

`dotnet ef` requer a ferramenta `dotnet-ef` compatível com EF Core 10. A API
usa `http://localhost:5282` nesse perfil; `GET /ping` e o Swagger permitem
conferir a execução. As migrations não são aplicadas automaticamente na
inicialização.

## Dados iniciais e escopo

As migrations criam o esquema, mas não cadastram projeto nem zonas. O
protótipo oferece consulta de projetos e cadastro/atualização de dispositivos;
não há endpoint de criação de projeto ou zona. Para um banco novo e vazio,
execute [scripts/bootstrap-maquette.sql](scripts/bootstrap-maquette.sql) após
aplicar as migrations. O script cria o projeto `Maquete TCC` e seis zonas; ele
recusa execução se já houver projetos ou zonas. Para preservar uma base
existente, use um backup validado ou prepare os cadastros de forma controlada.
Depois, cadastre os dispositivos pela Web e configure em cada ESP32 o
`DeviceId` correspondente.

Os exemplos de requisição estão em [SmartLighting.Api.http](SmartLighting.Api.http).
O firmware precisa acessar a API pelo endereço do computador na rede, não
por `localhost` da própria placa.
