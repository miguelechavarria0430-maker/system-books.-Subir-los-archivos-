
# Flujo de trabajo Git Flow

## 1. Ramas principales

- main: contiene las versiones estables del proyecto.
- develop: integra el desarrollo de las funcionalidades.

## 2. Ramas de características

Se utilizan las siguientes ramas:

- feature/models-libro
- feature/models-usuario
- feature/models-prestamo
- feature/models-enum-estado-prestamo
- feature/models-testing-objetos

## 3. Proceso de trabajo

1. Crear cada rama feature a partir de develop.
2. Implementar la funcionalidad correspondiente.
3. Registrar los cambios mediante commits descriptivos.
4. Publicar la rama en GitHub.
5. Crear un Pull Request hacia develop.
6. Revisar e integrar los cambios mediante Merge.

## 4. Integración final

La rama develop reúne las funcionalidades implementadas y verificadas. Cuando el proyecto está completo y estable, sus cambios pueden integrarse en main.

## 5. Convención de commits

- feat: nuevas funcionalidades.
- fix: correcciones.
- docs: documentación.
- build: configuración del proyecto.
- test: pruebas.

## 6. Resultado esperado

Un proyecto organizado mediante Git Flow, con historial de commits y Pull Requests que permiten identificar el desarrollo de cada funcionalidad.
