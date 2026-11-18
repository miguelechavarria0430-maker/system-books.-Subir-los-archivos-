
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

### Libro
Representa los libros registrados y controla su disponibilidad.

### Usuario
Representa a las personas registradas en la biblioteca.

### EstadoPrestamo
Enum con los estados Activo, Devuelto y Vencido.

### Prestamo
Relaciona un libro con un usuario y administra las fechas y el estado del préstamo.

## Funcionalidades
- Creación de libros y usuarios.
- Registro de préstamos.
- Consulta de información resumida y detallada.
- Validación de disponibilidad y actividad.
- Cálculo de días transcurridos.
- Verificación de préstamos vencidos.

## Ejecución

Desde una terminal con .NET 8 instalado:

```bash
## Mejoras finales

- Se organizaron las clases del sistema de biblioteca.
- Se implementaron modelos para libros, usuarios y préstamos.
- Se documentó el flujo de trabajo Git Flow.
dotnet restore
dotnet build
dotnet run
```

## Control de versiones
El proyecto utiliza Git Flow con las ramas main, develop y ramas feature para desarrollar cada modelo y las pruebas de objetos.
## Versión 1.0.0

Primera versión del sistema de biblioteca.
Incluye los modelos de libros, usuarios y préstamos,
y la documentación del flujo de trabajo Git Flow.
