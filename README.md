# Microsserviço com .NET 5

**Retorna o tempo atual de uma cidade usando a OpenWeather API.** É um projeto simples para estudar o funcionamento de um microsserviço.

Referência: [(video) Creating a .NET5 Microservice](https://youtu.be/MIJJCR3ndQQ)

## Index

- [Microsserviço com .NET 5](#microsserviço-com-net-5)
  - [Index](#index)
  - [Tecnologias e ferramentas](#tecnologias-e-ferramentas)
  - [Rotas](#rotas)
    - [Exemplo](#exemplo)
  - [Integração com uma API externa no backend](#integração-com-uma-api-externa-no-backend)
    - [Variáveis ambiente](#variáveis-ambiente)
    - [Transient Error Policy](#transient-error-policy)
      - [Circuit breaker](#circuit-breaker)
    - [Health Checks](#health-checks)
    - [Logs](#logs)


## Tecnologias e ferramentas

* .NET Core v5.0.102
* [Open Weather API](https://openweathermap.org/api)
* Insomnia
* HttpClient
* [Polly](https://github.com/App-vNext/Polly)

## Rotas

`GET /v1/weather/:city` - retorna dados do tempo da cidade :city

### Exemplo

Requisição: `/v1/weather/campinas`

Resposta:
```
{
  "date": "2021-02-11T19:54:25",
  "temperatureC": 22,
  "temperatureF": 71,
  "summary": "light rain"
}
```


## Integração com uma API externa no backend

* HttpClient
* Record Types

Veja o arquivo [WeatherClient.cs](WeatherClient.cs), que consome a URL através do método `GetFromJsonAsync`.
  

### Variáveis ambiente

* .NET secret manager
  
  `dotnet user-secret init`

  `dotnet user-secret set {}:{} {VALUE}`

  mais o arquivo [ServiceSettings.cs](ServiceSettings.cs) para acessá-los

Para saber mais acesse a [documentação](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets)


### Transient Error Policy

* Polly
  
  `dotnet add package Microsoft.Extensions.Http.Polly`

No arquivo [Startup.cs](Startup.cs), como uma "extensão" do HttpClient
```
  .AddTransientHttpErrorPolicy(builder => 
    builder.WaitAndRetryAsync(10, retryAttempt => 
      TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
    )
  );
```

#### Circuit breaker

Limite de tentativas para Retry connection

```
  .AddTransientHttpErrorPolicy(builder => 
    builder.CircuitBreakerAsync(3, TimeSpan.FromSeconds(10))
  );
```


### Health Checks

Endpoint para checar o estado do serviço

```
  services.AddHealthChecks();

  ...

  app.UseEndpoints(endpoints =>
  {
    endpoints.MapControllers();
    endpoints.MapHealthChecks("/health");
  });
```

Aditional checks
  * classe [`ExternalEndpointHealthCheck`](ExternalEndpointHealthCkeck.cs)
  * `.AddCheck<type>("name")`

### Logs

* JSON output

Em [`Program.cs`](Program.cs):
```
.ConfigureLogging((context, logging) => {
  if (context.HostingEnviroment.IsProduction())
  {
    logging.ClearProviders(); // Remove o console simples
    logging.AddJsonConsole();
  }
})
```
