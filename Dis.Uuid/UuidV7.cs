using System.Security.Cryptography;

namespace Dis.Uuid;

public static class UuidV7
{
	private static readonly ThreadLocal<RandomNumberGenerator> Rng = new(RandomNumberGenerator.Create);

	public static Guid New()
	{
		var unixMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

		Span<byte> uuid = stackalloc byte[16];

		uuid[0] = (byte)(unixMs >> 40);
		uuid[1] = (byte)(unixMs >> 32);
		uuid[2] = (byte)(unixMs >> 24);
		uuid[3] = (byte)(unixMs >> 16);
		uuid[4] = (byte)(unixMs >> 8);
		uuid[5] = (byte)unixMs;

		Rng.Value?.GetBytes(uuid[6..]);

		uuid[6] = (byte)(0x70 | (uuid[6] & 0x0F));

		uuid[8] = (byte)(0x80 | (uuid[8] & 0x3F));

		return new Guid(uuid);
	}
}