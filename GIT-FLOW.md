# Guía de Git Flow para EV07

> Ejecutar estos comandos en la terminal, dentro de la carpeta del repositorio. Si el repositorio ya tiene `develop`, no volver a crearla.

## 1. Preparar ramas base
```bash
git status
git branch
```

Si `develop` todavía no existe:
```bash
git checkout main
git pull origin main
git checkout -b develop
git push -u origin develop
```

Si ya existe:
```bash
git checkout develop
git pull origin develop
```

## 2. Repetir este flujo para cada componente
Para cada rama: partir de `develop`, crear la feature, agregar el archivo, hacer commit, subir la rama, volver a develop y fusionar.

### Libro
```bash
git checkout develop
git checkout -b feature/models-libro
git add Models/Libro.cs
git commit -m "feat: create Libro class with properties and constructors"
git push -u origin feature/models-libro
git checkout develop
git merge --no-ff feature/models-libro -m "merge: integrate models-libro into develop"
git push origin develop
```

### Usuario
```bash
git checkout develop
git checkout -b feature/models-usuario
git add Models/Usuario.cs
git commit -m "feat: create Usuario class with properties and methods"
git push -u origin feature/models-usuario
git checkout develop
git merge --no-ff feature/models-usuario -m "merge: integrate models-usuario into develop"
git push origin develop
```

### Prestamo
```bash
git checkout develop
git checkout -b feature/models-prestamo
git add Models/Prestamo.cs
git commit -m "feat: create Prestamo class and date methods"
git push -u origin feature/models-prestamo
git checkout develop
git merge --no-ff feature/models-prestamo -m "merge: integrate models-prestamo into develop"
git push origin develop
```

### EstadoPrestamo
```bash
git checkout develop
git checkout -b feature/models-enum-estado-prestamo
git add Models/EstadoPrestamo.cs
git commit -m "feat: add EstadoPrestamo enumeration"
git push -u origin feature/models-enum-estado-prestamo
git checkout develop
git merge --no-ff feature/models-enum-estado-prestamo -m "merge: integrate enum into develop"
git push origin develop
```

### Pruebas con objetos
```bash
git checkout develop
git checkout -b feature/models-testing-objetos
git add Program.cs
git commit -m "test: add sample objects and model validation menu"
git push -u origin feature/models-testing-objetos
git checkout develop
git merge --no-ff feature/models-testing-objetos -m "merge: integrate object testing into develop"
git push origin develop
```

## 3. Verificar integración
```bash
git checkout develop
git status
git branch -a
git log --oneline --graph --all
```

Si el repositorio remoto no está configurado, revisar:
```bash
git remote -v
```

No inventar ni editar el historial para simular ramas o merges. Las capturas de evidencia deben mostrar comandos ejecutados y resultados reales.
