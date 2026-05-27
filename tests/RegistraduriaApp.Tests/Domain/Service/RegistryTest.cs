using Edu.Unisabana.Tyvs.Domain.Model;
using Edu.Unisabana.Tyvs.Domain.Service;
using Xunit;

namespace Edu.Unisabana.Tyvs.Domain.Service;

/// <summary>
/// Pruebas del servicio Registry siguiendo TDD (Red → Green → Refactor)
/// Patrón AAA (Arrange – Act – Assert) y BDD (Given – When – Then)
/// </summary>
public class RegistryTest
{
    // ──────────────────────────────────────────────────────────────────
    // CICLO 1 – Camino feliz (persona válida)
    // ──────────────────────────────────────────────────────────────────

    [Fact]
    public void ShouldRegisterValidPerson()
    {
        // Given: una persona viva, mayor de edad, con id válido
        // Arrange
        var registry = new Registry();
        var person = new Person("Ana", 1, 30, Gender.FEMALE, true);

        // When: intento registrarla
        // Act
        var result = registry.RegisterVoter(person);

        // Then: el resultado debe ser VALID
        // Assert
        Assert.Equal(RegisterResult.VALID, result);
    }

    // ──────────────────────────────────────────────────────────────────
    // CICLO 2 – Persona muerta
    // ──────────────────────────────────────────────────────────────────

    [Fact]
    public void ShouldRejectDeadPerson()
    {
        // Given: una persona con alive = false
        // Arrange
        var registry = new Registry();
        var dead = new Person("Carlos", 2, 40, Gender.MALE, false);

        // When: intento registrarla
        // Act
        var result = registry.RegisterVoter(dead);

        // Then: el resultado debe ser DEAD
        // Assert
        Assert.Equal(RegisterResult.DEAD, result);
    }

    // ──────────────────────────────────────────────────────────────────
    // CICLO 3 – Persona nula (validación defensiva)
    // ──────────────────────────────────────────────────────────────────

    [Fact]
    public void ShouldReturnInvalidWhenPersonIsNull()
    {
        // Given: la persona es null
        // Arrange
        var registry = new Registry();

        // When: intento registrar null
        // Act
        var result = registry.RegisterVoter(null);

        // Then: el resultado debe ser INVALID
        // Assert
        Assert.Equal(RegisterResult.INVALID, result);
    }

    // ──────────────────────────────────────────────────────────────────
    // CICLO 4 – Id inválido (≤ 0)
    // ──────────────────────────────────────────────────────────────────

    [Fact]
    public void ShouldRejectWhenIdIsZeroOrNegative()
    {
        // Given: persona con id = 0, edad 25, viva
        // Arrange
        var registry = new Registry();
        var person = new Person("Luis", 0, 25, Gender.MALE, true);

        // When: intento registrarla
        // Act
        var result = registry.RegisterVoter(person);

        // Then: el resultado debe ser INVALID
        // Assert
        Assert.Equal(RegisterResult.INVALID, result);
    }

    // ──────────────────────────────────────────────────────────────────
    // CICLO 5 – Menor de edad (valor límite 17)
    // ──────────────────────────────────────────────────────────────────

    [Fact]
    public void ShouldRejectUnderageAt17()
    {
        // Given: persona viva con 17 años e id válido
        // Arrange
        var registry = new Registry();
        var person = new Person("Pedro", 3, 17, Gender.MALE, true);

        // When: intento registrarla
        // Act
        var result = registry.RegisterVoter(person);

        // Then: el resultado debe ser UNDERAGE
        // Assert
        Assert.Equal(RegisterResult.UNDERAGE, result);
    }

    // ──────────────────────────────────────────────────────────────────
    // CICLO 6 – Mayor de edad (valor límite 18)
    // ──────────────────────────────────────────────────────────────────

    [Fact]
    public void ShouldAcceptAdultAt18()
    {
        // Given: persona viva con exactamente 18 años e id válido
        // Arrange
        var registry = new Registry();
        var person = new Person("Maria", 4, 18, Gender.FEMALE, true);

        // When: intento registrarla
        // Act
        var result = registry.RegisterVoter(person);

        // Then: el resultado debe ser VALID
        // Assert
        Assert.Equal(RegisterResult.VALID, result);
    }

    // ──────────────────────────────────────────────────────────────────
    // CICLO 7 – Edad máxima permitida (valor límite 120)
    // ──────────────────────────────────────────────────────────────────

    [Fact]
    public void ShouldAcceptMaxAge120()
    {
        // Given: persona viva con 120 años e id válido
        // Arrange
        var registry = new Registry();
        var person = new Person("Abuela", 5, 120, Gender.FEMALE, true);

        // When: intento registrarla
        // Act
        var result = registry.RegisterVoter(person);

        // Then: el resultado debe ser VALID
        // Assert
        Assert.Equal(RegisterResult.VALID, result);
    }

    // ──────────────────────────────────────────────────────────────────
    // CICLO 8 – Edad inválida por encima del límite (121)
    // ──────────────────────────────────────────────────────────────────

    [Fact]
    public void ShouldRejectInvalidAgeOver120()
    {
        // Given: persona viva con 121 años e id válido
        // Arrange
        var registry = new Registry();
        var person = new Person("Fantasma", 6, 121, Gender.MALE, true);

        // When: intento registrarla
        // Act
        var result = registry.RegisterVoter(person);

        // Then: el resultado debe ser INVALID_AGE
        // Assert
        Assert.Equal(RegisterResult.INVALID_AGE, result);
    }

    // ──────────────────────────────────────────────────────────────────
    // CICLO 9 – Edad negativa (inválida por debajo del límite)
    // ──────────────────────────────────────────────────────────────────

    [Fact]
    public void ShouldRejectNegativeAge()
    {
        // Given: persona viva con edad -1 e id válido
        // Arrange
        var registry = new Registry();
        var person = new Person("Error", 7, -1, Gender.MALE, true);

        // When: intento registrarla
        // Act
        var result = registry.RegisterVoter(person);

        // Then: el resultado debe ser INVALID_AGE
        // Assert
        Assert.Equal(RegisterResult.INVALID_AGE, result);
    }

    // ──────────────────────────────────────────────────────────────────
    // CICLO 10 – Inscripción duplicada (mismo id dos veces)
    // ──────────────────────────────────────────────────────────────────

    [Fact]
    public void ShouldRejectDuplicateRegistration()
    {
        // Given: una persona válida ya registrada
        // Arrange
        var registry = new Registry();
        var person1 = new Person("Jorge", 777, 25, Gender.MALE, true);
        var person2 = new Person("Jorge Clon", 777, 30, Gender.MALE, true);
        registry.RegisterVoter(person1);

        // When: intento registrar otra persona con el mismo id
        // Act
        var result = registry.RegisterVoter(person2);

        // Then: el resultado debe ser DUPLICATED
        // Assert
        Assert.Equal(RegisterResult.DUPLICATED, result);
    }
}
