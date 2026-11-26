using RetoExcepciones;

Console.WriteLine("=== RETO EXCEPCIONES ===");
Console.Write("Por favor, introduce tu edad: ");

try
{
    int edad = int.Parse(Console.ReadLine() ?? "");

    if (edad < 0 || edad > 120)
    {
        throw new EdadInvalidaException(
            "La edad debe estar entre 0 y 120 años"
        );
    }

    Console.WriteLine("Edad válida: " + edad);
}
catch (FormatException)
{
    Console.WriteLine("Error: Debes introducir un número válido");
}
catch (EdadInvalidaException ex)
{
    Console.WriteLine(ex.Message);
}
finally
{
    Console.WriteLine("Proceso de validación finalizado");
}
