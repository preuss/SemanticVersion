using System;
namespace SemanticVersionTest {
	using SemVersion;

	using Xunit;

	public class GetHashCodeTests {
		[Fact]
		public void GetHashCodeEqual() {
			SemanticVersion left = SemanticVersion.NewVersion(1, 0, 0, "foo", "bar");
			SemanticVersion right = SemanticVersion.NewVersion(1, 0, 0, "foo", "bar");

			Assert.Equal(left.GetHashCode(), right.GetHashCode());
		}

		[Fact]
		public void GetHashCodeNotEqual() {
			SemanticVersion left = SemanticVersion.NewVersion(1, 0, 0, "foo", "bar");
			SemanticVersion right = SemanticVersion.NewVersion(2, 0, 0, "foo", "bar");

			Assert.NotEqual(left.GetHashCode(), right.GetHashCode());
		}

		[Fact]
		public void GetHashCodeNotEqualNoBuild() {
			SemanticVersion left = SemanticVersion.NewVersion(1, 0, 0, "foo");
			SemanticVersion right = SemanticVersion.NewVersion(2, 0, 0, "foo");

			Assert.NotEqual(left.GetHashCode(), right.GetHashCode());
		}

		[Fact]
		public void GetHashCodeNotEqualNoBuildNoPrerelease() {
			SemanticVersion left = SemanticVersion.NewVersion(1, 0, 0);
			SemanticVersion right = SemanticVersion.NewVersion(2, 0, 0);

			Assert.NotEqual(left.GetHashCode(), right.GetHashCode());
		}
	}
}
