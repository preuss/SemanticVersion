namespace SemanticVersionTest.Comparer {
	using SemVersion;
	using System.Collections.Generic;
	using Xunit;

	public class CompareTests {
		[Fact]
		public void Compare() {
			SemanticVersion left = SemanticVersion.NewVersion(1, 0, 0);
			SemanticVersion right = SemanticVersion.NewVersion(1, 0, 0);

			VersionComparer comparer = new VersionComparer();

			Assert.Equal(0, comparer.Compare(left, right));
		}

		[Fact]
		public void CompareBig() {
			SemanticVersion left = new SemanticVersion(1, 2, 3, new List<string> { "SomeBodyRelease" }, SemVersion.VersionExtras.ReleaseStageEnum.Beta, 4, new List<string> { "BuildWithSomeName" });
			SemanticVersion right = new SemanticVersion(1, 2, 3, new List<string> { "SomeBodyRelease" }, SemVersion.VersionExtras.ReleaseStageEnum.Beta, 4, new List<string> { "BuildWithSomeName" });

			VersionComparer comparer = new VersionComparer();

			Assert.Equal(0, comparer.Compare(left, right));
		}

		[Fact]
		public void CompareLeftNull() {
			SemanticVersion right = SemanticVersion.NewVersion(1, 0, 0);

			VersionComparer comparer = new VersionComparer();

			Assert.Equal(-1, comparer.Compare(null, right));
		}

		[Fact]
		public void CompareRightNull() {
			SemanticVersion left = SemanticVersion.NewVersion(1, 0, 0);

			VersionComparer comparer = new VersionComparer();

			Assert.Equal(1, comparer.Compare(left, null));
		}

		[Fact]
		public void CompareBothNull() {
			VersionComparer comparer = new VersionComparer();

			Assert.Equal(0, comparer.Compare(null, null));
		}

		[Fact]
		public void ComparePrereleaseRelease() {
			SemanticVersion left = SemanticVersion.NewVersion(1, 0, 0, "beta");
			SemanticVersion right = SemanticVersion.NewVersion(1, 0, 0);

			VersionComparer comparer = new VersionComparer();

			// "When major, minor, and patch are equal, a pre-release
			// version has lower precedence than a normal version."
			Assert.Equal(-1, comparer.Compare(left, right));
		}

		[Fact]
		public void ComparePrereleaseNumeric() {
			SemanticVersion left = SemanticVersion.NewVersion(1, 0, 0, "alpha.23");
			SemanticVersion right = SemanticVersion.NewVersion(1, 0, 0, "alpha.5");

			VersionComparer comparer = new VersionComparer();

			// "identifiers consisting of only digits are compared numerically"
			Assert.Equal(1, comparer.Compare(left, right));
		}

		[Fact]
		public void ComparePrereleaseNonnumeric() {
			SemanticVersion left = SemanticVersion.NewVersion(1, 0, 0, "42.alpha");
			SemanticVersion right = SemanticVersion.NewVersion(1, 0, 0, "42.beta");

			VersionComparer comparer = new VersionComparer();

			// "identifiers with letters or hyphens are compared lexically in ASCII sort order"
			Assert.Equal(-1, comparer.Compare(left, right));
		}

		[Fact]
		public void ComparePrereleaseNumericVsOther() {
			SemanticVersion left = SemanticVersion.NewVersion(1, 0, 0, "alpha.1");
			SemanticVersion right = SemanticVersion.NewVersion(1, 0, 0, "alpha.beta");

			VersionComparer comparer = new VersionComparer();

			// "Numeric identifiers always have lower precedence than non-numeric identifiers."
			// Meaning: "... precede ..." (as per the above example from the spec).
			Assert.Equal(-1, comparer.Compare(left, right));
		}

		[Fact]
		public void ComparePrereleaseFieldNumber() {
			SemanticVersion left = SemanticVersion.NewVersion(1, 0, 0, "alpha");
			SemanticVersion right = SemanticVersion.NewVersion(1, 0, 0, "alpha.1");

			VersionComparer comparer = new VersionComparer();

			// "A larger set of pre-release fields has a higher
			// precedence than a smaller set, if all of the preceding
			// identifiers are equal."
			Assert.Equal(-1, comparer.Compare(left, right));
		}
	}
}
