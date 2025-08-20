using System;
using System.Security.Cryptography;
using System.Threading;

namespace Dis.Uuid;

/// <summary>
/// UUID version 7 generator.
/// </summary>
public static class UuidV7
{
	private static readonly ThreadLocal<RandomNumberGenerator> Rng = new(RandomNumberGenerator.Create);

	/// <summary>
	/// Generates a new UUIDv7.
	/// </summary>
	/// <returns>A new <see cref="Guid"/> UUID v7</returns>
	public static Guid New()
	{
		var unixMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

		Span<byte> uuid = stackalloc byte[16];

		// Timestamp (48 bits)
		uuid[0] = (byte)(unixMs >> 40);
		uuid[1] = (byte)(unixMs >> 32);
		uuid[2] = (byte)(unixMs >> 24);
		uuid[3] = (byte)(unixMs >> 16);
		uuid[4] = (byte)(unixMs >> 8);
		uuid[5] = (byte)unixMs;

		// Gera bytes aleatórios para posições 6-15
		Rng.Value?.GetBytes(uuid.Slice(6));

		// Version (4 bits) - versão 7
		uuid[6] = (byte)(0x70 | (uuid[6] & 0x0F));

		// Variant (2 bits) - RFC 4122
		uuid[8] = (byte)(0x80 | (uuid[8] & 0x3F));

		return new Guid(uuid);
	}
}