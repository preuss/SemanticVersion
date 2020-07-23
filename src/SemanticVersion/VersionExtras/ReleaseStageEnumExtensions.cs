using System;
using System.Collections.Generic;
using System.Text;

namespace SemVersion.VersionExtras {
	public static class ReleaseStageEnumExtensions {
		public static String ToReleaseStringOrDefault(this ReleaseStageEnum? releaseStage, string defaultStrValue) {
			return releaseStage switch
			{
				null => defaultStrValue,
				ReleaseStageEnum.None => "",
				_ => releaseStage.Value.ToString(),
			};
		}
	}
}
