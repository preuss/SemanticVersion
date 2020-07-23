namespace SemanticVersionTest {
	using System;
	using SemVersion;
	using Xunit;

	public class ParseTests {
		[Fact]
		public void Parse() {
			SemanticVersion version = SemanticVersion.Parse("1.1.1");

			Assert.Equal(1U, version.Major);
			Assert.Equal(1U, version.Minor);
			Assert.Equal(1U, version.Patch);
		}

		[Fact]
		public void ParseValidZeroes() {
			SemanticVersion version = SemanticVersion.Parse("0.1.2");
			Assert.Equal(0U, version.Major);

			version = SemanticVersion.Parse("1.0.2");
			Assert.Equal(0U, version.Minor);

			version = SemanticVersion.Parse("1.2.0");
			Assert.Equal(0U, version.Patch);
		}

		[Fact]
		public void ParseMaxInts() {
			const UInt32 value = UInt32.MaxValue;
			SemanticVersion version = SemanticVersion.Parse(string.Format("{0}.{0}.{0}", value));

			Assert.Equal(value, version.Major);
			Assert.Equal(value, version.Minor);
			Assert.Equal(value, version.Patch);
		}

		[Fact]
		public void ParsePrerelease() {
			SemanticVersion version = SemanticVersion.Parse("1.1.1-alpha-12");

			Assert.Equal(1U, version.Major);
			Assert.Equal(1U, version.Minor);
			Assert.Equal(1U, version.Patch);
			Assert.Equal("alpha-12", version.Release);
		}

		[Fact]
		public void ParseBuild() {
			SemanticVersion version = SemanticVersion.Parse("1.1.1+nightly.23.43-foo");

			Assert.Equal(1U, version.Major);
			Assert.Equal(1U, version.Minor);
			Assert.Equal(1U, version.Patch);
			Assert.Equal("nightly.23.43-foo", version.Build);
		}

		[Fact]
		public void ParseComplete() {
			SemanticVersion version = SemanticVersion.Parse("1.1.1-alpha-12+nightly.23.43-foo");

			Assert.Equal(1U, version.Major);
			Assert.Equal(1U, version.Minor);
			Assert.Equal(1U, version.Patch);
			Assert.Equal("alpha-12", version.Release);
			Assert.Equal("nightly.23.43-foo", version.Build);
		}

		[Fact]
		public void ParseNonComplientCharacters() {
			const String specialChars = "æøå.ÆØÅ._$-£€¤#@'!^~|{}[]();";
			SemanticVersion version = SemanticVersion.Parse(string.Format("1.2.3-{0}+{0}", specialChars));

			Assert.Equal(1U, version.Major);
			Assert.Equal(2U, version.Minor);
			Assert.Equal(3U, version.Patch);
			Assert.Equal(specialChars, version.Release);
			Assert.Equal(specialChars, version.Build);
		}

		[Fact]
		public void ParseNullThrowsArgumentNullException() {
			Assert.Throws<ArgumentException>(() => SemanticVersion.Parse(null));
		}

		[Fact]
		public void ParseEmptyStringThrows() {
			Assert.Throws<ArgumentException>(() => SemanticVersion.Parse(string.Empty));
		}

		[Fact]
		public void ParseWhiteSpaceThrows() {
			Assert.Throws<ArgumentException>(() => SemanticVersion.Parse(" "));
		}

		[Fact]
		public void ParseLeadingZeroesThrows() {
			Assert.Throws<ArgumentException>(() => SemanticVersion.Parse("01.1.1"));
			Assert.Throws<ArgumentException>(() => SemanticVersion.Parse("1.01.1"));
			Assert.Throws<ArgumentException>(() => SemanticVersion.Parse("1.1.01"));
		}

		[Fact]
		public void ParseNegativeNumbersThrows() {
			Assert.Throws<ArgumentException>(() => SemanticVersion.Parse("-1.1.1"));
			Assert.Throws<ArgumentException>(() => SemanticVersion.Parse("1.-1.1"));
			Assert.Throws<ArgumentException>(() => SemanticVersion.Parse("1.1.-1"));
		}

		[Fact]
		public void ParseInvalidStringThrows() {
			Assert.Throws<ArgumentException>(() => SemanticVersion.Parse("invalid-version"));
		}

		[Fact]
		public void ParseMissingMinorThrows() {
			Assert.Throws<ArgumentException>(() => SemanticVersion.Parse("1"));
		}

		[Fact]
		public void ParseMissingPatchThrows() {
			Assert.Throws<ArgumentException>(() => SemanticVersion.Parse("1.2"));
		}

		[Fact]
		public void ParseMissingPatchWithPrereleaseThrows() {
			Assert.Throws<ArgumentException>(() => SemanticVersion.Parse("1.2-alpha"));
		}

		[Fact]
		public void ParseMissingLeadingHyphenForPrereleaseThrows() {
			Assert.Throws<ArgumentException>(() => SemanticVersion.Parse("1.2.3.alpha"));
		}

		[Fact]
		public void ImplicitConversion() {
			SemanticVersion version = "1.1.1";

			Assert.Equal(1U, version.Major);
			Assert.Equal(1U, version.Minor);
			Assert.Equal(1U, version.Patch);
		}

		[Fact]
		public void ImplicitConversionFail() {
			Assert.Throws<ArgumentException>(() => {
				SemanticVersion version = "1";
			});
		}
	}
}
