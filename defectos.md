# Gestión de Defectos – Registraduría

---

### Defecto 01
- **Caso:** edad -1 con persona viva e id válido
- **Esperado:** `INVALID_AGE`
- **Obtenido:** `VALID`
- **Causa probable:** falta de validación de límites de edad en `Registry.RegisterVoter`
- **Estado:** Abierto

---

### Defecto 02
- **Caso:** persona `null` pasada al método `RegisterVoter`
- **Esperado:** `INVALID`
- **Obtenido:** `NullReferenceException`
- **Causa probable:** ausencia de guardia defensiva al inicio del método
- **Estado:** Abierto

---

### Defecto 03
- **Caso:** persona con id = 0
- **Esperado:** `INVALID`
- **Obtenido:** `VALID`
- **Causa probable:** no se valida que el id sea positivo
- **Estado:** Abierto
