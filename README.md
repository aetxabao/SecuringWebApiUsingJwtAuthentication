# SecuringWebApiUsingJwtAuthentication

Adaptación a SQLite del código del artículo [secure-asp-net-core-web-api-using-jwt-authentication](http://codingsonata.com/secure-asp-net-core-web-api-using-jwt-authentication/ "codingsonata") en el que se implementa un servidor REST utilizando JWT.

## Consideraciones

La base de datos SQLite está en el fichero Customers.db. Para ejecutar el servidor es necesario disponer del paquete correspondiente:

```
dotnet add package Microsoft.EntityFrameworkCore.SQLite
```

El API del servicio puede ser visible en [Swagger](https://localhost:5001/swagger/index.html "API Swagger") cuando se está ejecutando.

![API del servicio](https://github.com/aetxabao/SecuringWebApiUsingJwtAuthentication/blob/main/swagger_SWAUJA.png?raw=true)

