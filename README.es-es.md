# Dis.Uuid

[![NuGet](https://img.shields.io/nuget/v/Dis.Uuid.svg)](https://www.nuget.org/packages/Dis.Uuid/)
[![Downloads](https://img.shields.io/nuget/dt/Dis.Uuid.svg)](https://www.nuget.org/packages/Dis.Uuid/)
[![License](https://img.shields.io/badge/license-BSD%203--Clause-blue.svg)](LICENSE.txt)

Una biblioteca .NET de alto rendimiento para generar identificadores **UUID versión 7** (UUIDv7) con capacidades de ordenación temporal.

## 🎯 ¿Qué es UUID v7?

UUID versión 7 es una variante de UUID ordenada por tiempo que combina:
- **Timestamp de 48 bits** (precisión de milisegundos)
- **12 bits de datos aleatorios** para ordenación sub-milisegundo
- **62 bits de datos aleatorios** para unicidad

Este diseño garantiza:
- ✅ **Ordenación cronológica** - UUIDs más nuevos son lexicográficamente mayores
- ✅ **Rendimiento de base de datos** - excelente para claves primarias e índices
- ✅ **Unicidad global** - resistente a colisiones en sistemas distribuidos
- ✅ **Thread safety** - seguro para uso concurrente

## 🚀 Instalación

```bash
dotnet add package Dis.Uuid
```

O vía Package Manager Console:
```powershell
Install-Package Dis.Uuid
```

## 📖 Uso

```csharp
using Dis.Uuid;

// Genera un nuevo UUID v7
Guid id = UuidV7.New();
Console.WriteLine(id); // ej: 018f-4230-1234-7000-abcdef123456

// Úsalo en tus modelos
public class Pedido
{
    public Guid Id { get; set; } = UuidV7.New();
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    // ... otras propiedades
}
```

## 🔧 Características

- **Alto Rendimiento**: Usa `Span<byte>` y `ThreadLocal<RandomNumberGenerator>` para rendimiento óptimo
- **Thread Safe**: Generación concurrente en múltiples hilos
- **Eficiencia de Memoria**: Cero asignaciones de heap durante la generación de UUID
- **Compatible con Estándares**: Sigue la especificación RFC 4122 UUID v7
- **Optimizado para .NET 8+**: Aprovecha las mejoras de rendimiento de .NET más reciente

## 📊 Rendimiento

La generación de UUID v7 es extremadamente rápida y eficiente:

```csharp
// Ejemplo de benchmark
var sw = Stopwatch.StartNew();
for (int i = 0; i < 1_000_000; i++)
{
    var uuid = UuidV7.New();
}
sw.Stop();
Console.WriteLine($"Generados 1M UUIDs en {sw.ElapsedMilliseconds}ms");
```

## 🔄 Ordenación Temporal

Uno de los principales beneficios del UUID v7 es la ordenación temporal natural:

```csharp
var id1 = UuidV7.New();
await Task.Delay(1); // Pequeño retraso
var id2 = UuidV7.New();

// id2 siempre será lexicográficamente mayor que id1
Assert.True(string.Compare(id1.ToString(), id2.ToString()) < 0);
```

## 🎯 Casos de Uso

Perfecto para:
- **Claves Primarias de Base de Datos** - Mejor rendimiento que UUIDs tradicionales
- **Sistemas Distribuidos** - Unicidad global con ordenación temporal
- **Event Sourcing** - Ordenación cronológica natural de eventos
- **Sistemas de Logging** - Entradas de log ordenadas por tiempo
- **Microservicios** - Generación consistente de IDs entre servicios

## 🔒 Thread Safety

La biblioteca es completamente thread-safe y puede usarse concurrentemente:

```csharp
// Seguro para usar en múltiples hilos
var tasks = Enumerable.Range(0, 10)
    .Select(_ => Task.Run(() => 
    {
        var ids = new List<Guid>();
        for (int i = 0; i < 1000; i++)
        {
            ids.Add(UuidV7.New());
        }
        return ids;
    }));

var results = await Task.WhenAll(tasks);
// Todos los UUIDs generados serán únicos
```

## 🧪 Pruebas

La biblioteca incluye pruebas exhaustivas que cubren:
- Validación de unicidad
- Verificación de ordenación temporal
- Pruebas de thread safety
- Benchmarks de rendimiento

Ejecutar pruebas:
```bash
dotnet test
```

## 📋 Requisitos

- .NET 8.0 o superior
- Compatible con todas las cargas de trabajo .NET 8+ (Console, Web, Desktop, etc.)

## 🤝 Contribuyendo

¡Las contribuciones son bienvenidas! No dudes en enviar issues y pull requests.

1. Haz fork del repositorio
2. Crea una rama de feature
3. Realiza tus cambios
4. Añade pruebas si es necesario
5. Envía un pull request

## 📄 Licencia

Este proyecto está licenciado bajo la Licencia BSD 3-Clause - consulta el archivo [LICENSE.txt](LICENSE.txt) para más detalles.

## 🔗 Referencias

- [RFC 4122 - Especificación UUID](https://tools.ietf.org/html/rfc4122)
- [UUID Version 7 Draft](https://datatracker.ietf.org/doc/html/draft-ietf-uuidrev-rfc4122bis)
- [Documentación .NET Guid](https://docs.microsoft.com/es-es/dotnet/api/system.guid)

---

**Hecho con ❤️ para la comunidad .NET**