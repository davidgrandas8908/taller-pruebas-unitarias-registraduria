# Taller de Pruebas Unitarias – TDD con C# y xUnit

Implementación del taller de **Desarrollo Dirigido por Pruebas (TDD)** aplicado a una arquitectura limpia, usando C# (.NET 8) y xUnit como framework de pruebas.

---

## Integrantes

Ver [`integrantes.txt`](integrantes.txt)

---

## Dominio: Registraduría Electoral

El sistema permite registrar personas como votantes para las próximas elecciones y valida que cada inscripción cumpla las reglas de negocio del dominio.

### Reglas de negocio

- Solo se registran personas vivas.
- La edad debe estar entre 18 y 120 años.
- El id debe ser un número positivo.
- No se permite más de una inscripción por número de documento.

---

## Estructura del proyecto

```
RegistraduriaApp/
├── src/
│   └── RegistraduriaApp/               # Lógica de dominio (class library)
│       └── Domain/
│           ├── Model/
│           │   ├── Person.cs
│           │   ├── Gender.cs
│           │   └── RegisterResult.cs
│           └── Service/
│               └── Registry.cs
└── tests/
    └── RegistraduriaApp.Tests/         # Pruebas unitarias (xUnit)
        └── Domain/
            └── Service/
                └── RegistryTest.cs
```

---

## Requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

---

## Ejecutar las pruebas

```bash
# Compilar y ejecutar todas las pruebas
dotnet test

# Con reporte de cobertura (equivalente a mvn jacoco:report)
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=lcov /p:CoverletOutput=../../coverage/
```

### Generar reporte HTML de cobertura

```bash
# Instalar la herramienta (una sola vez)
dotnet tool install -g dotnet-reportgenerator-globaltool

# Generar reporte
reportgenerator -reports:./coverage/coverage.info -targetdir:./coverage/report -reporttypes:Html
```

### Dónde encontrar el informe de cobertura

Todos los artefactos de cobertura quedan bajo la carpeta `coverage/` en la raíz del repositorio:

| Archivo | Ruta relativa | Descripción |
|---------|---------------|-------------|
| Datos LCOV (Coverlet) | `coverage/coverage.info` | Salida cruda usada por ReportGenerator |
| **Informe HTML** | **`coverage/report/index.html`** | Reporte visual para abrir en el navegador |

Ruta absoluta de ejemplo (sustituye por la carpeta donde clonaste el repo):

```
<tu-carpeta-del-proyecto>\coverage\report\index.html
```

Abre `coverage/report/index.html` en el navegador para ver el informe (equivalente a `target/site/jacoco/index.html` en Maven).

---

## Metodología aplicada

### TDD – Red → Green → Refactor

Cada funcionalidad fue desarrollada siguiendo el ciclo iterativo:

1. **Red:** se escribe una prueba que falla porque la funcionalidad no existe aún.
2. **Green:** se implementa el mínimo de código para que la prueba pase.
3. **Refactor:** se mejora el código sin romper las pruebas existentes.

### Patrón AAA (Arrange – Act – Assert)

Todas las pruebas siguen la misma estructura:

```csharp
[Fact]
public void ShouldRegisterValidPerson()
{
    // Arrange: preparar datos y objetos
    var registry = new Registry();
    var person = new Person("Ana", 1, 30, Gender.FEMALE, true);

    // Act: ejecutar la acción bajo prueba
    var result = registry.RegisterVoter(person);

    // Assert: verificar el resultado esperado
    Assert.Equal(RegisterResult.VALID, result);
}
```

### BDD – Given / When / Then

Cada prueba también expresa su intención en lenguaje de negocio mediante comentarios:

```
Given: una persona viva con 17 años e id válido
When:  intento registrarla
Then:  el resultado debe ser UNDERAGE
```

---

## Clases de equivalencia y valores límite

| Caso | Entrada representativa | Resultado esperado | Test |
|------|------------------------|-------------------|------|
| Persona válida | edad=30, vivo=true, id=1 | `VALID` | `ShouldRegisterValidPerson` |
| Persona muerta | vivo=false | `DEAD` | `ShouldRejectDeadPerson` |
| Persona nula | `null` | `INVALID` | `ShouldReturnInvalidWhenPersonIsNull` |
| Id inválido (0 o negativo) | id=0, edad=25, vivo=true | `INVALID` | `ShouldRejectWhenIdIsZeroOrNegative` |
| Menor de edad – límite inferior (17) | edad=17, vivo=true | `UNDERAGE` | `ShouldRejectUnderageAt17` |
| Adulto – límite inferior (18) | edad=18, vivo=true | `VALID` | `ShouldAcceptAdultAt18` |
| Edad máxima válida (120) | edad=120, vivo=true | `VALID` | `ShouldAcceptMaxAge120` |
| Edad inválida por encima (121) | edad=121, vivo=true | `INVALID_AGE` | `ShouldRejectInvalidAgeOver120` |
| Edad negativa | edad=-1, vivo=true | `INVALID_AGE` | `ShouldRejectNegativeAge` |
| Inscripción duplicada | mismo id registrado dos veces | `DUPLICATED` | `ShouldRejectDuplicateRegistration` |

---

## Equivalencias Java / Maven → C# / .NET

| Java | C# |
|------|----|
| JUnit 4 | xUnit |
| JaCoCo | Coverlet + ReportGenerator |
| `mvn clean test` | `dotnet test` |
| `pom.xml` | `.csproj` |
| `package` | `namespace` |

---

## Gestión de defectos

Ver [`defectos.md`](defectos.md) para el registro de defectos encontrados durante el desarrollo.
