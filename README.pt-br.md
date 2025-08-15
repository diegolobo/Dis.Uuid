# Dis.Uuid

[![NuGet](https://img.shields.io/nuget/v/Dis.Uuid.svg)](https://www.nuget.org/packages/Dis.Uuid/)
[![Downloads](https://img.shields.io/nuget/dt/Dis.Uuid.svg)](https://www.nuget.org/packages/Dis.Uuid/)
[![License](https://img.shields.io/badge/license-BSD%203--Clause-blue.svg)](LICENSE.txt)

Uma biblioteca .NET de alta performance para gerar identificadores **UUID versão 7** (UUIDv7) com capacidades de ordenação temporal.

## 🎯 O que é UUID v7?

UUID versão 7 é uma variante de UUID ordenada por tempo que combina:
- **Timestamp de 48 bits** (precisão de milissegundos)
- **12 bits de dados aleatórios** para ordenação sub-milissegundo
- **62 bits de dados aleatórios** para unicidade

Este design garante:
- ✅ **Ordenação cronológica** - UUIDs mais novos são lexicograficamente maiores
- ✅ **Performance de banco de dados** - excelente para chaves primárias e índices
- ✅ **Unicidade global** - resistente a colisões em sistemas distribuídos
- ✅ **Thread safety** - seguro para uso concorrente

## 🚀 Instalação

```bash
dotnet add package Dis.Uuid
```

Ou via Package Manager Console:
```powershell
Install-Package Dis.Uuid
```

## 📖 Uso

```csharp
using Dis.Uuid;

// Gera um novo UUID v7
Guid id = UuidV7.New();
Console.WriteLine(id); // ex: 018f-4230-1234-7000-abcdef123456

// Use em seus modelos
public class Pedido
{
    public Guid Id { get; set; } = UuidV7.New();
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
    // ... outras propriedades
}
```

## 🔧 Funcionalidades

- **Alta Performance**: Usa `Span<byte>` e `ThreadLocal<RandomNumberGenerator>` para performance ótima
- **Thread Safe**: Geração concorrente em múltiplas threads
- **Eficiência de Memória**: Zero alocações de heap durante a geração de UUID
- **Compatível com Padrões**: Segue a especificação RFC 4122 UUID v7
- **Otimizado para .NET 8+**: Aproveita as melhorias de performance do .NET mais recente

## 📊 Performance

A geração de UUID v7 é extremamente rápida e eficiente:

```csharp
// Exemplo de benchmark
var sw = Stopwatch.StartNew();
for (int i = 0; i < 1_000_000; i++)
{
    var uuid = UuidV7.New();
}
sw.Stop();
Console.WriteLine($"Gerados 1M UUIDs em {sw.ElapsedMilliseconds}ms");
```

## 🔄 Ordenação Temporal

Um dos principais benefícios do UUID v7 é a ordenação temporal natural:

```csharp
var id1 = UuidV7.New();
await Task.Delay(1); // Pequeno atraso
var id2 = UuidV7.New();

// id2 sempre será lexicograficamente maior que id1
Assert.True(string.Compare(id1.ToString(), id2.ToString()) < 0);
```

## 🎯 Casos de Uso

Perfeito para:
- **Chaves Primárias de Banco de Dados** - Melhor performance que UUIDs tradicionais
- **Sistemas Distribuídos** - Unicidade global com ordenação temporal
- **Event Sourcing** - Ordenação cronológica natural de eventos
- **Sistemas de Log** - Entradas de log ordenadas por tempo
- **Microsserviços** - Geração consistente de IDs entre serviços

## 🔒 Thread Safety

A biblioteca é completamente thread-safe e pode ser usada concorrentemente:

```csharp
// Seguro para usar em múltiplas threads
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
// Todos os UUIDs gerados serão únicos
```

## 🧪 Testes

A biblioteca inclui testes abrangentes cobrindo:
- Validação de unicidade
- Verificação de ordenação temporal
- Testes de thread safety
- Benchmarks de performance

Execute os testes:
```bash
dotnet test
```

## 📋 Requisitos

- .NET 8.0 ou superior
- Compatível com todas as cargas de trabalho .NET 8+ (Console, Web, Desktop, etc.)

## 🤝 Contribuindo

Contribuições são bem-vindas! Sinta-se à vontade para enviar issues e pull requests.

1. Faça um fork do repositório
2. Crie uma branch de feature
3. Faça suas alterações
4. Adicione testes se necessário
5. Envie um pull request

## 📄 Licença

Este projeto está licenciado sob a Licença BSD 3-Clause - veja o arquivo [LICENSE.txt](LICENSE.txt) para detalhes.

## 🔗 Referências

- [RFC 4122 - Especificação UUID](https://tools.ietf.org/html/rfc4122)
- [UUID Version 7 Draft](https://datatracker.ietf.org/doc/html/draft-ietf-uuidrev-rfc4122bis)
- [Documentação .NET Guid](https://docs.microsoft.com/pt-br/dotnet/api/system.guid)

---

**Feito com ❤️ para a comunidade .NET**