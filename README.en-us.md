# Dis.Uuid

[![NuGet](https://img.shields.io/nuget/v/Dis.Uuid.svg)](https://www.nuget.org/packages/Dis.Uuid/)
[![Downloads](https://img.shields.io/nuget/dt/Dis.Uuid.svg)](https://www.nuget.org/packages/Dis.Uuid/)
[![License](https://img.shields.io/badge/license-BSD%203--Clause-blue.svg)](LICENSE.txt)

A high-performance .NET library for generating **UUID version 7** (UUIDv7) identifiers with time-ordering capabilities.

## 🎯 What is UUID v7?

UUID version 7 is a time-ordered UUID variant that combines:
- **48-bit timestamp** (millisecond precision)
- **12-bit random data** for sub-millisecond ordering
- **62-bit random data** for uniqueness

This design ensures:
- ✅ **Chronological ordering** - newer UUIDs are lexicographically greater
- ✅ **Database performance** - excellent for primary keys and indexes
- ✅ **Global uniqueness** - collision-resistant across distributed systems
- ✅ **Thread safety** - safe for concurrent use

## 🚀 Installation

```bash
dotnet add package Dis.Uuid
```

Or via Package Manager Console:
```powershell
Install-Package Dis.Uuid
```

## 📖 Usage

```csharp
using Dis.Uuid;

// Generate a new UUID v7
Guid id = UuidV7.New();
Console.WriteLine(id); // e.g., 018f-4230-1234-7000-abcdef123456

// Use in your models
public class Order
{
    public Guid Id { get; set; } = UuidV7.New();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    // ... other properties
}
```

## 🔧 Features

- **High Performance**: Uses `Span<byte>` and `ThreadLocal<RandomNumberGenerator>` for optimal performance
- **Thread Safe**: Concurrent generation across multiple threads
- **Memory Efficient**: Zero heap allocations during UUID generation
- **Standards Compliant**: Follows RFC 4122 UUID v7 specification
- **.NET 8+ Optimized**: Takes advantage of latest .NET performance improvements

## 📊 Performance

UUID v7 generation is extremely fast and efficient:

```csharp
// Benchmark example
var sw = Stopwatch.StartNew();
for (int i = 0; i < 1_000_000; i++)
{
    var uuid = UuidV7.New();
}
sw.Stop();
Console.WriteLine($"Generated 1M UUIDs in {sw.ElapsedMilliseconds}ms");
```

## 🔄 Time Ordering

One of the key benefits of UUID v7 is natural time ordering:

```csharp
var id1 = UuidV7.New();
await Task.Delay(1); // Small delay
var id2 = UuidV7.New();

// id2 will always be lexicographically greater than id1
Assert.True(string.Compare(id1.ToString(), id2.ToString()) < 0);
```

## 🎯 Use Cases

Perfect for:
- **Database Primary Keys** - Better performance than traditional UUIDs
- **Distributed Systems** - Global uniqueness with time ordering
- **Event Sourcing** - Natural chronological ordering of events
- **Logging Systems** - Time-ordered log entries
- **Microservices** - Consistent ID generation across services

## 🔒 Thread Safety

The library is fully thread-safe and can be used concurrently:

```csharp
// Safe to use across multiple threads
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
// All generated UUIDs will be unique
```

## 🧪 Testing

The library includes comprehensive tests covering:
- Uniqueness validation
- Time ordering verification
- Thread safety testing
- Performance benchmarks

Run tests:
```bash
dotnet test
```

## 📋 Requirements

- .NET 8.0 or higher
- Compatible with all .NET 8+ workloads (Console, Web, Desktop, etc.)

## 🤝 Contributing

Contributions are welcome! Please feel free to submit issues and pull requests.

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if needed
5. Submit a pull request

## 📄 License

This project is licensed under the BSD 3-Clause License - see the [LICENSE.txt](LICENSE.txt) file for details.

## 🔗 References

- [RFC 4122 - UUID Specification](https://tools.ietf.org/html/rfc4122)
- [UUID Version 7 Draft](https://datatracker.ietf.org/doc/html/draft-ietf-uuidrev-rfc4122bis)
- [.NET Guid Documentation](https://docs.microsoft.com/en-us/dotnet/api/system.guid)

---

**Made with ❤️ for the .NET community**