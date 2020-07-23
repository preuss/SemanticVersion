using System;

namespace SemVersion {
	internal static class UtilitiesExtensions {
		/// <summary>Compares two boxed integers and returns an indication of their relative values.</summary>
		/// <param name="left">The leftside integer to compare.</param>
		/// <param name="right">The rightside integer to compare.</param>
		/// <returns>
		/// A signed number indicating the relative values of this instance and value.
		/// Return Value Description: 
		/// Less than zero: This instance is less than value. 
		/// Zero: This instance is equal to value. 
		/// Greater than zero: This instance is greater than value.
		///</returns>
		public static int CompareToBoxed(this int? left, int? right) {
			if(!left.HasValue) {
				return !right.HasValue ? 0 : -1;
			}
			return !right.HasValue ? 1 : left.Value.CompareTo(right.Value);
		}
		/// <summary>Compares two unsigned integers and returns an indication of their relative values.</summary>
		/// <param name="left">The leftside unsigned integer to compare.</param>
		/// <param name="right">The rightside unsigned integer to compare.</param>
		/// <returns>
		/// A signed number indicating the relative values of this instance and value.
		/// Return Value Description: 
		/// Less than zero: This instance is less than value. 
		/// Zero: This instance is equal to value. 
		/// Greater than zero: This instance is greater than value.
		///</returns>
		public static int CompareToBoxed(this UInt32 left, UInt32 right) {
			return left.CompareTo(right);
		}
	}
}
