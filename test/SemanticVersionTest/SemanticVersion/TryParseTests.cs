using System.Collections.Generic;

namespace SemanticVersionTest {
	using SemVersion;

	using Xunit;


	public class TryParseTests {
		[Fact]
		public void TryParseReturnsVersion() {
			SemanticVersion version;
			var result = SemanticVersion.TryParse("1.2.3", out version);

			Assert.True(result);
			Assert.Equal(new SemanticVersion(1, 2, 3), version);
		}

		[Fact]
		public void TryParseNullReturnsFalse() {
			SemanticVersion version;
			var result = SemanticVersion.TryParse(null, out version);

			Assert.False(result);
			Assert.Null(version);
		}

		[Fact]
		public void TryParseEmptyStringReturnsFalse() {
			SemanticVersion version;
			var result = SemanticVersion.TryParse(string.Empty, out version);

			Assert.False(result);
			Assert.Null(version);
		}

		[Fact]
		public void TryParseInvalidStringReturnsFalse() {
			SemanticVersion version;
			var result = SemanticVersion.TryParse("invalid-version", out version);

			Assert.False(result);
			Assert.Null(version);
		}

		[Fact]
		public void TryParseMissingMinorReturnsFalse() {
			SemanticVersion version;
			var result = SemanticVersion.TryParse("1", out version);

			Assert.False(result);
			Assert.Null(version);
		}

		[Fact]
		public void TryParseMissingPatchReturnsFalse() {
			SemanticVersion version;
			var result = SemanticVersion.TryParse("1.2", out version);

			Assert.False(result);
			Assert.Null(version);
		}

		[Fact]
		public void TryParseMissingPatchValueReturnsFalse() {
			SemanticVersion version;
			// Trailing separator but no value
			var result = SemanticVersion.TryParse("1.2.", out version);

			Assert.False(result);
			Assert.Null(version);
		}

		[Fact]
		public void TryParseMissingPatchValueWithPrereleaseReturnsFalse() {
			SemanticVersion version;

			var result = SemanticVersion.TryParse("1.2-alpha", out version);

			Assert.False(result);
			Assert.Null(version);
		}

		[Fact]
		public void TryParseMissingPatchWithPrereleaseReturnsFalse() {
			SemanticVersion version;
			// Trailing separator but no value
			var result = SemanticVersion.TryParse("1.2-alpha.", out version);
			Assert.False(result);
			Assert.Null(version);
		}

		[Fact]
		public void TryParseMajorZeroWithTrailingSeparator() {
			SemanticVersion version;
			var result = SemanticVersion.TryParse("0. ", out version);

			Assert.False(result);
			Assert.Null(version);
		}

		[Fact]
		public void TryParseMinorZeroWithTrailingSeparator() {
			SemanticVersion version;
			var result = SemanticVersion.TryParse("1.0.  ", out version);

			Assert.False(result);
			Assert.Null(version);
		}

		[Fact]
		public void TryParsePatchZeroWithTrailingSeparator() {
			SemanticVersion version;
			var result = SemanticVersion.TryParse("1.2.0.  ", out version);

			Assert.False(result);
			Assert.Null(version);
		}


		[Fact]
		public void TryParseInvalidMajorMinorPatchValues() {
			SemanticVersion version;
			var invalidAtoms = new string[] { "-01", "-1", "00", "01", "0 2" };
			var validAtoms = new string[] { "0", "1", "10", "1234" };

			var list = new List<string>();
			list.AddRange(invalidAtoms);
			list.AddRange(validAtoms);


			var testValues = list.ToArray();
			bool result = false;
			string verStr;

			foreach(var major in invalidAtoms) {
				foreach(var minor in validAtoms) {
					foreach(var patch in validAtoms) {

						verStr = string.Format("{0}.{1}.{2}", major, minor, patch);
						result = SemanticVersion.TryParse(verStr, out version);
						Assert.False(result, verStr);
						Assert.Null(version);

						foreach(var prerelease in validAtoms) {

							verStr = string.Format("{0}.{1}.{2}-{3}", major, minor, patch, prerelease);
							result = SemanticVersion.TryParse(verStr, out version);
							Assert.False(result, verStr);
							Assert.Null(version);

							foreach(var build in validAtoms) {

								verStr = string.Format("{0}.{1}.{2}-{3}+{4}", major, minor, patch, prerelease, build);
								result = SemanticVersion.TryParse(verStr, out version);
								Assert.False(result);
								Assert.Null(version);
							}

						}
					}
				}
			}
		}


		[Fact]
		public void TryParseEmptyInBuild() {
			SemanticVersion version;
			var result = SemanticVersion.TryParse("1.2.3+", out version);

			Assert.True(result);

			// The test restult below is intended, as build shouldn't factor 
			// into version precedende (and therefore equality).
			Assert.Equal("1.2.3", version.ToString());
		}

		[Fact] //(Skip="Broken")]
		public void TryParseEmptyInBuildWithPrerelease() {
			SemanticVersion version;
			var result = SemanticVersion.TryParse("1.2.3-preR+", out version);

			Assert.True(result);

			// The test restult below is intended, as build shouldn't factor 
			// into version precedende (and therefore equality).
			Assert.Equal("1.2.3-preR", version.ToString());
		}

	}
}