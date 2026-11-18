
# Sistema de Biblioteca - SystemBooks

## Evidencia EV07

Definición de clases y objetos base del sistema de biblioteca en C# aplicando programación orientada a objetos y Git Flow.

## Descripción

Aplicación de consola desarrollada en C# que representa las operaciones básicas de una biblioteca mediante clases, objetos, propiedades, constructores y métodos.

## Tecnologías

- C#
- .NET 8
- Git
- GitHub

## Estructura del proyecto

```text
SystemBooks/
├── Models/
│   ├── Libro.cs
│   ├── Usuario.cs
│   ├── Prestamo.cs
│   └── EstadoPrestamo.cs
├── Program.cs
├── SystemBooks.csproj
├── README.md
└── GIT-FLOW.md
```

## Modelos implementados

- **Libro:** representa los libros y controla su disponibilidad.
- **Usuario:** representa a las personas registradas.
- **EstadoPrestamo:** enumera los estados Activo, Devuelto y Vencido.
- **Prestamo:** relaciona un libro con un usuario y administra fechas y estado.

## Funcionalidades

- Creación de libros y usuarios.
- Registro de préstamos.
- Consulta de información resumida y detallada.
- Validación de disponibilidad y actividad.
- Cálculo de días transcurridos.
- Verificación de préstamos vencidos.

## Ejecución

Con .NET 8 instalado, ejecutar:

```bash
dotnet restore
dotnet build
dotnet run
```

## Control de versiones

Se utiliza Git Flow con las ramas main, develop, feature, release y hotfix.

## Versión 1.0.1

Actualización correctiva posterior a la versión 1.0.0.
