using System;
using System.Collections.Generic;
using System.Text;

namespace SemVersion.VersionExtras {
	/// <summary>
	/// A list of development stages.
	/// Referes to pre-release identifier in SemVer.
	/// </summary>
	public enum ReleaseStageEnum {
		/// <summary>
		/// This is the default or unspecified value.
		/// </summary>
		None = 0,
		/// <summary>
		/// Usually akin to a prototype, this has either very little
		/// functionality, is a mock-up, or is still otherwise in the
		/// design stages.
		/// </summary>
		PreAlpha = 10,
		/// <summary>
		/// This typically means that work on major features is still ongoing.
		/// </summary>
		Alpha = 20,
		/// <summary>
		/// This typically means that major features are complete, though
		/// not necessarily bug-free or tested, and may or may not mean
		/// that minor features are done or tested.
		/// </summary>
		Beta = 30,
		/// <summary>
		/// This means that we are needing some tests before Release Candidate.
		/// </summary>
		Gamma = 40,
		/// <summary>
		/// Is Release Candidate.
		/// This typically means that all planned features or changes are
		/// either done or cut, as well as tested and mostly ready.  Code
		/// should be mainly stable.
		/// </summary>
		RC = 50,
		/// <summary>
		/// This typically means a shipping or production version.
		/// </summary>
		Final = 100
	}
}
