# EV07 - Sistema de Biblioteca en C#

## Descripción
Proyecto de consola en C# que define las entidades principales de un sistema de biblioteca mediante programación orientada a objetos.

## Requisitos implementados
- Clase `Libro`: propiedades, constructor vacío y completo, `ResumenCorto()`, `DetalleCompleto()` y `ToString()`. `Disponible` inicia en `true`.
- Clase `Usuario`: propiedades, constructor vacío y completo, `ResumenCorto()`, `DetalleCompleto()` y `ToString()`. `Activo` inicia en `true`.
- Enumerador `EstadoPrestamo`: `Activo`, `Devuelto` y `Vencido`.
- Clase `Prestamo`: propiedades `DateTime` y `DateTime?`, constructor vacío y completo, `EstaVencido()`, `DiasTranscurridos()`, `ResumenCorto()`, `DetalleCompleto()` y `ToString()`. Inicia en estado `Activo` y `FechaDevolucion = null`.
- Programa de prueba: dos libros, dos usuarios y un préstamo, con menú para consultar resúmenes, detalles y validaciones.

## Estructura
```text
system-books/
├── Models/
│   ├── Libro.cs
│   ├── Usuario.cs
│   ├── Prestamo.cs
│   └── EstadoPrestamo.cs
├── Program.cs
├── SystemBooks.csproj
├── .gitignore
└── README.md
```

## Ejecución
Se requiere instalar el SDK de .NET 8 o superior. Desde la carpeta del proyecto:

```bash
dotnet restore
dotnet build
dotnet run
```

## Pruebas manuales
Al ejecutar el programa:
1. Seleccionar `1` para ver los dos libros.
2. Seleccionar `2` para ver los dos usuarios.
3. Seleccionar `3` para ver el préstamo.
4. Seleccionar `4` para comprobar disponibilidad, actividad, estado, vencimiento, días transcurridos y fecha de devolución nula.
5. Seleccionar `5` para mostrar toda la información.
6. Seleccionar `0` para salir.

El préstamo de prueba tiene tres días transcurridos y fecha límite cuatro días después de la fecha actual. Por eso, en condiciones normales, `EstaVencido()` devuelve `False`.

## Gestión de versiones con Git Flow
Las ramas, commits, publicaciones y merges deben ejecutarse realmente en un repositorio Git. El archivo `GIT-FLOW.md` contiene una secuencia de referencia. No se debe afirmar que se ejecutaron si no se realizaron.
