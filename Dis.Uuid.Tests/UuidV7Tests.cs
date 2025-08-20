using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dis.Uuid.Tests;

public class UuidV7Tests
{
	[Fact]
	public void New_ShouldReturnValidGuid()
	{
		// Act
		var guid = UuidV7.New();

		// Assert
		Assert.NotEqual(Guid.Empty, guid);
	}

	[Fact]
	public void New_ShouldReturnUniqueGuids()
	{
		// Arrange
		const int count = 1000;
		var guids = new HashSet<Guid>();

		// Act
		for (var i = 0; i < count; i++)
			guids.Add(UuidV7.New());

		// Assert
		Assert.Equal(count, guids.Count);
	}

	[Fact]
	public async Task New_ShouldGenerateIncreasingGuidsAcrossTimeIntervals()
	{
		// Arrange & Act
		var guid1 = UuidV7.New();
		await Task.Delay(100);
		var guid2 = UuidV7.New();

		// Assert
		Assert.True(string.CompareOrdinal(guid1.ToString(), guid2.ToString()) < 0,
			"Later generated GUID should be lexicographically greater");
	}

	[Fact]
	public async Task New_ShouldBeThreadSafe()
	{
		// Arrange
		const int threadsCount = 10;
		const int guidsPerThread = 1000;
		var allGuids = new HashSet<Guid>();
		var lockObject = new object();

		// Act
		var tasks = Enumerable.Range(0, threadsCount)
			.Select(_ => Task.Run(() =>
			{
				var threadGuids = new List<Guid>();
				for (var j = 0; j < guidsPerThread; j++)
					threadGuids.Add(UuidV7.New());

				lock (lockObject)
				{
					foreach (var guid in threadGuids)
						allGuids.Add(guid);
				}
			}))
			.ToArray();

		await Task.WhenAll(tasks);

		// Assert
		Assert.Equal(threadsCount * guidsPerThread, allGuids.Count);
	}

	[Fact]
	public void New_ShouldHaveCorrectVersion()
	{
		// Act
		var guid = UuidV7.New();
		var bytes = guid.ToByteArray();

		// Assert - UUID v7 deve ter version = 7 nos 4 bits mais significativos do byte 6
		var version = (bytes[6] & 0xF0) >> 4;
		Assert.Equal(7, version);
	}

	[Fact]
	public void New_ShouldHaveCorrectVariant()
	{
		// Act
		var guid = UuidV7.New();
		var bytes = guid.ToByteArray();

		// Assert - RFC 4122 variant (10xx nos 2 bits mais significativos do byte 8)
		var variant = (bytes[8] & 0xC0) >> 6;
		Assert.Equal(2, variant); // 10 em binário = 2 em decimal
	}

	[Fact]
	public void New_ShouldHaveTimestampInFirstBytes()
	{
		// Act
		var before = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
		var guid = UuidV7.New();
		var after = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

		// Extract timestamp from first 6 bytes
		var bytes = guid.ToByteArray();
		var timestamp = ((long)bytes[0] << 40) |
						((long)bytes[1] << 32) |
						((long)bytes[2] << 24) |
						((long)bytes[3] << 16) |
						((long)bytes[4] << 8) |
						bytes[5];

		// Assert - timestamp deve estar no intervalo
		Assert.True(timestamp >= before && timestamp <= after,
			$"Timestamp {timestamp} should be between {before} and {after}");
	}
}