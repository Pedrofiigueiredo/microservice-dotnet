# Microsserviço com .NET 5

&#9881; Em desenvolvimento...

Referência: [(video) Creating a .NET5 Microservice]()


## Tecnologias e ferramentas

* [Open Weather API](https://openweathermap.org/api)
* Insomnia
* HttpClient

## Rotas

`GET /v1/weather/:city` - retorna dados do tempo de da cidade :city


## Integração com uma API externa no backend

* HttpClient
* Record Types
  

### Variáveis ambiente

* .NET secret manager
  
  `dotnet user-secret init`

  `dotnet user-secret set {}:{} {VALUE}`

  Para saber mais acesse a [documentação](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets)
