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
		for (var i = 0; i < count; i++) guids.Add(UuidV7.New());

		// Assert
		Assert.Equal(count, guids.Count);
	}

	[Fact]
	public async Task New_ShouldGenerateIncreasingGuidsAcrossTimeIntervals()
	{
		// Arrange & Act
		var guid1 = UuidV7.New();
		await Task.Delay(5);
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
				for (var j = 0; j < guidsPerThread; j++) threadGuids.Add(UuidV7.New());

				lock (lockObject)
				{
					foreach (var guid in threadGuids) allGuids.Add(guid);
				}
			}))
			.ToArray();

		await Task.WhenAll(tasks);

		// Assert
		Assert.Equal(threadsCount * guidsPerThread, allGuids.Count);
	}
}