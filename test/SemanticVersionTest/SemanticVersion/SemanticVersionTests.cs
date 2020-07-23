namespace SemanticVersionTest {
	using SemVersion;

	using Xunit;

	public class SemanticVersionTests {

		[Fact]
		public void ConstuctWithValidNulls() {
			// Null indicates zero in Major, Minor or Patch segments 
			SemanticVersion version = new SemanticVersion(null, 99, 74);
			Assert.Equal("0.99.74", version.ToString());

			version = new SemanticVersion(99, null, 74);
			Assert.Equal("99.0.74", version.ToString());

			version = new SemanticVersion(74, 66, null);
			Assert.Equal("74.66", version.ToString(trimVersionElements: true));

			// Null in prerelease or build segments is effectively ignored
			version = SemanticVersion.NewVersion(74, 66, 99, null);
			Assert.Equal("74.66.99", version.ToString());

			version = SemanticVersion.NewVersion(74, 66, 99, "Pre-R", null);
			Assert.Equal("74.66.99-Pre-R", version.ToString());
		}

	}
}
