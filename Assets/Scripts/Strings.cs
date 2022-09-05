using System;

public static class Strings {
	public static string ToOrdinal(int value) {
		int magnitude = Math.Abs(value);
		int remainder = magnitude % 100;

		return (value < 0 ? "-" : "")
			+ magnitude
			+ (remainder >= 11 && remainder <= 13
				? "th"
				: (magnitude % 10) switch { 1 => "st", 2 => "nd", 3 => "rd", _ => "th" }
		);
	}

	public static string TimestampFromSeconds(float seconds) {
		return TimeSpan.FromSeconds(seconds).ToString("m\\:ss\\.fff");
	}
}