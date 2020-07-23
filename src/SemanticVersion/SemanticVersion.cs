#nullable enable

namespace SemVersion {
	using SemVersion.VersionExtras;
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.Design;
	using System.Diagnostics.CodeAnalysis;
	using System.Globalization;
	using System.Linq.Expressions;
	using System.Text;
	using System.Text.RegularExpressions;


	/// <summary>
	/// Represents a version object, compliant with the Semantic Version standard 2.0 (http://semver.org)
	/// </summary>
	public class SemanticVersion : IEquatable<SemanticVersion>, IComparable<SemanticVersion> {
		public const char BlockSeparator = '-';
		public const char ReleasePrefix = '-';
		public const char BuildPrefix = '+';
		public const char DOT = '.';
		public const bool TrimZero = true;

		private static IComparer<SemanticVersion> comparer = new VersionComparer();

		private static readonly Regex VersionExpression = new Regex(
			@"^(?<major>[0]|[1-9]+[0-9]*)((\.(?<minor>[0]|[1-9]+[0-9]*))(\.(?<patch>[0]|[1-9]+[0-9]*))?)?(\-(?<release>[0-9A-Za-zæøåÆØÅ\-\._\$\£\€\¤\#\@\'\!\^\~\|\{\}\[\]\(\)\;]*))?(\+(?<build>[0-9A-Za-zæøåÆØÅ\-\._\$\£\€\¤\#\@\'\!\^\~\|\{\}\[\]\(\)\;]*))?$",

			RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture);

		/// <summary>Initializese a new instance of the <see cref="SemanticVersion"/> class.</summary>
		/// <param name="major">The major version component.</param>
		/// <param name="minor">The minor version component.</param>
		/// <param name="patch">The patch version component.</param>
		/// <param name="releaseBlock">The releaseBlock version component.</param>
		/// <param name="build">The build version component.</param>
		public SemanticVersion(UInt32? major = null, UInt32? minor = null, UInt32? patch = null, List<string>? releaseBlocks = null, ReleaseStageEnum? releaseStage = null, UInt32? releaseStageCount = null, List<string>? buildBlocks = null) {
			this.Major = major.GetValueOrDefault(0);
			this.Minor = minor.GetValueOrDefault(0);
			this.Patch = patch.GetValueOrDefault(0);
			this.ReleaseBlocks = releaseBlocks ?? new List<string>();
			this.ReleaseStage = releaseStage ?? ReleaseStageEnum.None;
			this.ReleaseStageCount = releaseStageCount ?? 0;
			this.BuildBlocks = buildBlocks ?? new List<string>();
		}

		/// <summary>Gets the major version component.</summary>
		public UInt32 Major { get; }

		/// <summary>Gets the minor version component.</summary>
		public UInt32 Minor { get; }

		/// <summary>Gets the patch version component.</summary>
		public UInt32 Patch { get; }

		public List<String> ReleaseBlocks { get; }

		/// <summary>
		/// Gets the p
		/// </summary>
		public ReleaseStageEnum ReleaseStage { get; }

		public UInt32? ReleaseStageCount { get; }

		/// <summary>Gets the release version string component.</summary>
		public string Release {
			get {
				StringBuilder @string = new StringBuilder();
				if(ReleaseStageEnum.None != ReleaseStage) {
					@string.Append(BlockSeparator).Append(ReleaseStage.ToString());
					if(ReleaseStageCount.HasValue) {
						// TrimZero ignores ReleaseStageCount with zero
						if(!TrimZero || ReleaseStageCount != 0) {
							@string.Append(DOT).Append(ReleaseStageCount.Value);
						}
					}
				}

				foreach(string block in ReleaseBlocks) {
					@string.Append(BlockSeparator).Append(block);
				}

				if(@string.Length == 0) {
					return "";
				}

				// Removes the first block separator
				@string.Remove(0, 1);
				return @string.ToString();
			}
		}

		public List<String> BuildBlocks { get; }

		/// <summary>Gets the build version component.</summary>
		public string? Build {
			get {
				var @string = new StringBuilder();
				foreach(var block in BuildBlocks) {
					@string.Append(BlockSeparator).Append(block);
				}
				if(@string.Length == 0) {
					return "";
				}

				// Remove the first block separator
				@string.Remove(0, 1);
				return @string.ToString();
			}
		}

		public static bool operator ==(SemanticVersion left, SemanticVersion right) {
			return comparer.Compare(left, right) == 0;
		}

		public static bool operator !=(SemanticVersion left, SemanticVersion right) {
			return comparer.Compare(left, right) != 0;
		}

		public static bool operator <(SemanticVersion left, SemanticVersion right) {
			return comparer.Compare(left, right) < 0;
		}

		public static bool operator >(SemanticVersion left, SemanticVersion right) {
			return comparer.Compare(left, right) > 0;
		}

		public static bool operator <=(SemanticVersion left, SemanticVersion right) {
			return left == right || left < right;
		}

		public static bool operator >=(SemanticVersion left, SemanticVersion right) {
			return left == right || left > right;
		}

		/// <summary>Implicitly converts a string into a <see cref="SemanticVersion"/>.</summary>
		/// <param name="versionString">The string to convert.</param>
		/// <returns>The <see cref="SemanticVersion"/> object.</returns>

		public static implicit operator SemanticVersion(string versionString) {
			// ReSharper disable once ArrangeStaticMemberQualifier
			return SemanticVersion.ToSemanticVersion(versionString);
		}

		public static SemanticVersion ToSemanticVersion(string versionString) {
			// ReSharper disable once ArrangeStaticMemberQualifier
			return SemanticVersion.Parse(versionString);
		}

		/// <summary>Explicitly converts a <see cref="System.Version"/> object into a <see cref="SemanticVersion"/>.</summary>
		/// <param name="dotNetVersion">The version to convert.</param>
		/// <remarks>
		/// <para>This operator converts a C# <see cref="System.Version"/> object into the corresponding <see cref="SemanticVersion"/> object.</para>
		/// <para>Note, that with a C# version the <see cref="System.Version.Build"/> property is identical to the <see cref="Patch"/> property on a semantic version compliant object.
		/// Whereas the <see cref="System.Version.Revision"/> property is equivalent to the <see cref="Build"/> property on a semantic version.
		/// The <see cref="Release"/> property is never set, since the C# version object does not use such a notation.</para>
		/// </remarks>
		public static explicit operator SemanticVersion(Version dotNetVersion) {
			return ToSemanticVersion(dotNetVersion);
		}

		public static SemanticVersion ToSemanticVersion(Version dotNetVersion) {
			if(dotNetVersion == null) {
				throw new ArgumentNullException(nameof(dotNetVersion));
			}

			UInt32 major = Convert.ToUInt32(dotNetVersion.Major);
			UInt32 minor = Convert.ToUInt32(dotNetVersion.Minor);
			UInt32 patch = dotNetVersion.Build >= 0 ? Convert.ToUInt32(dotNetVersion.Build) : 0;
			string build = dotNetVersion.Revision >= 0 ? dotNetVersion.Revision.ToString(CultureInfo.InvariantCulture.NumberFormat) : string.Empty;

			return new SemanticVersion(major, minor, patch, buildBlocks: new List<string> { build });
		}

		/// <summary>Change the comparer used to compare two <see cref="SemanticVersion"/> objects.</summary>
		/// <param name="versionComparer">An instance of the comparer to use in future comparisons.</param>
		public static void ChangeComparer(IComparer<SemanticVersion> versionComparer) => comparer = versionComparer;

		public static SemanticVersion FluxVersion() => new SemanticVersion(0);

		/// <summary>Describes the first public api version.</summary>
		/// <returns>A <see cref="SemanticVersion"/> with version 1.0.0 as version number.</returns>
		public static SemanticVersion PublicVersion() => new SemanticVersion(1);

		/// <summary>Checks if a given string can be considered a valid <see cref="SemanticVersion"/>.</summary>
		/// <param name="inputString">The string to check for validity.</param>
		/// <returns>True, if the passed string is a valid <see cref="SemanticVersion"/>, otherwise false.</returns>
		public static bool IsVersion(string inputString) => VersionExpression.IsMatch(inputString);

		public static SemanticVersion NewVersion(int? major = null, int? minor = null, int? patch = null, String releaseStr = null, String buildStr = null) {
			if(major.HasValue && major.Value < 0) {
				throw new ArgumentException("The provided major version element is less than zero.", nameof(major));
			}
			if(minor.HasValue && minor.Value < 0) {
				throw new ArgumentException("The provided minor version element is less than zero.", nameof(minor));
			}
			if(patch.HasValue && patch.Value < 0) {
				throw new ArgumentException("The provided patch version element is less than zero.", nameof(patch));
			}

			UInt32 majorVal = Convert.ToUInt32(major.GetValueOrDefault(0));
			UInt32 minorVal = Convert.ToUInt32(minor.GetValueOrDefault(0));
			UInt32 patchVal = Convert.ToUInt32(patch.GetValueOrDefault(0));
			List<string> releaseList = string.IsNullOrWhiteSpace(releaseStr) ? new List<string>() : new List<string> { releaseStr.Trim() };
			List<string> buildList = string.IsNullOrWhiteSpace(buildStr) ? new List<string>() : new List<string> { buildStr.Trim() };

			return new SemanticVersion(majorVal, minorVal, patchVal, releaseBlocks: releaseList, buildBlocks: buildList);
		}

		/// <summary>Parses the specified string to a semantic version.</summary>
		/// <param name="versionString">The version string.</param>
		/// <returns>A new <see cref="SemanticVersion"/> object that has the specified values.</returns>
		/// <exception cref="ArgumentNullException">Raised when the input string is null.</exception>
		/// <exception cref="ArgumentException">Raised when the the input string is in an invalid format.</exception>
		public static SemanticVersion Parse(string versionString) {
			if(string.IsNullOrWhiteSpace(versionString)) {
				throw new ArgumentException("The provided version string is either null, empty or only consits of whitespace.", nameof(versionString));
			}

			if(!TryParse(versionString, out var version)) {
				throw new ArgumentException("The provided version string is invalid.", nameof(versionString));
			}

			return version;
		}

		/// <summary>Tries to parse the specified string into a semantic version.</summary>
		/// <param name="versionString">The version string.</param>
		/// <param name="version">When the method returns, contains a SemVersion instance equivalent
		/// to the version string passed in, if the version string was valid, or <c>null</c> if the
		/// version string was not valid.</param>
		/// <returns><c>False</c> when a invalid version string is passed, otherwise <c>true</c>.</returns>
		public static bool TryParse(string versionString, out SemanticVersion? version) {
			version = null;

			if(string.IsNullOrEmpty(versionString)) {
				return false;
			}

			var versionMatch = VersionExpression.Match(versionString);

			if(!versionMatch.Success) {
				return false;
			}

			var majorMatch = versionMatch.Groups["major"];
			var minorMatch = versionMatch.Groups["minor"];
			var patchMatch = versionMatch.Groups["patch"];
			var releaseMatch = versionMatch.Groups["release"];
			var buildMatch = versionMatch.Groups["build"];

			if(!majorMatch.Success) {
				return false;
			}
			UInt32 major;
			if(!UInt32.TryParse(majorMatch.Value, out major)) {
				return false;
			}

			if(!minorMatch.Success) {
				return false;
			}

			UInt32 minor;
			if(!UInt32.TryParse(minorMatch.Value, out minor)) {
				return false;
			}

			if(!patchMatch.Success) {
				return false;
			}
			UInt32 patch;
			if(!UInt32.TryParse(patchMatch.Value, out patch)) {
				return false;
			}

			var release = releaseMatch.Value;
			var build = buildMatch.Value != "*" ? buildMatch.Value : string.Empty;

			version = new SemanticVersion(major, minor, patch, releaseBlocks: new List<string> { release }, buildBlocks: new List<string> { build });
			return true;
		}

		/// <inheritdoc />
		public bool Equals(SemanticVersion other) => comparer.Compare(this, other) == 0;

		/// <inheritdoc />
		public int CompareTo(object obj) => comparer.Compare(this, obj as SemanticVersion);

		/// <inheritdoc />
		public int CompareTo(SemanticVersion other) => comparer.Compare(this, other);

		/// <inheritdoc />
		public override bool Equals(object obj) => this.Equals(obj as SemanticVersion);

		/// <inheritdoc />
		public override int GetHashCode() {
			unchecked {
				int hashCode = Convert.ToInt32(this.Major);
				hashCode = (hashCode * 397) ^ Convert.ToInt32(this.Minor);
				hashCode = (hashCode * 397) ^ Convert.ToInt32(this.Patch);
				hashCode = (hashCode * 397) ^ (!string.IsNullOrWhiteSpace(this.Release) ? StringComparer.OrdinalIgnoreCase.GetHashCode(this.Release) : 0);
				hashCode = (hashCode * 397) ^ (!string.IsNullOrWhiteSpace(this.Build) ? StringComparer.OrdinalIgnoreCase.GetHashCode(this.Build) : 0);
				return hashCode;
			}
		}

		public string ToString(bool trimVersionElements) {
			var builder = new StringBuilder();

			builder.Append(this.Major);

			if(this.Minor == 0
				&& this.Patch == 0
				&& String.IsNullOrWhiteSpace(this.Release)
				&& String.IsNullOrWhiteSpace(this.Build)
				&& trimVersionElements
				) {
				return builder.ToString();
			}
			builder.Append(DOT).Append(this.Minor);

			if(this.Patch == 0
				&& String.IsNullOrWhiteSpace(this.Release)
				&& String.IsNullOrWhiteSpace(this.Build)
				&& trimVersionElements
				) {
				return builder.ToString();
			}
			builder.Append(DOT).Append(Patch);

			if(String.IsNullOrWhiteSpace(this.Release)
				&& String.IsNullOrWhiteSpace(this.Build)
				) {
				return builder.ToString();
			}

			builder.Append(ReleasePrefix).Append(Release);

			if(String.IsNullOrWhiteSpace(this.Build)) {
				return builder.ToString();
			}

			builder.Append(BuildPrefix).Append(Build);

			return builder.ToString();
		}
		/// <inheritdoc />
		public override string ToString() {
			return this.ToString(false);
		}
	}
}
