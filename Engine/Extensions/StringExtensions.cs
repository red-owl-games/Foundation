using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace RedOwl.Core
{
	public static class StringUtility
	{
		public static bool IsNullOrWhiteSpace(string s)
		{
			return s == null || s.Trim() == string.Empty;
		}

		public static string FallbackEmpty(string s, string fallback)
		{
			if (string.IsNullOrEmpty(s))
			{
				s = fallback;
			}

			return s;
		}

		public static string FallbackWhitespace(string s, string fallback)
		{
			if (IsNullOrWhiteSpace(s))
			{
				s = fallback;
			}

			return s;
		}

		public static void AppendLineFormat(this StringBuilder sb, string format, params object[] args)
		{
			sb.AppendFormat(format, args);
			sb.AppendLine();
		}

		public static string ToSeparatedString(this IEnumerable enumerable, string separator)
		{
			return string.Join(separator, enumerable.Cast<object>().Select(o => o?.ToString() ?? "(null)").ToArray());
		}

		public static string ToCommaSeparatedString(this IEnumerable enumerable)
		{
			return ToSeparatedString(enumerable, ", ");
		}

		public static string ToLineSeparatedString(this IEnumerable enumerable)
		{
			return ToSeparatedString(enumerable, Environment.NewLine);
		}

		public static bool ContainsInsensitive(this string haystack, string needle)
		{
			return haystack.IndexOf(needle, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		public static IEnumerable<int> AllIndexesOf(this string haystack, string needle)
		{
			if (string.IsNullOrEmpty(needle))
			{
				yield break;
			}

			for (var index = 0;; index += needle.Length)
			{
				index = haystack.IndexOf(needle, index, StringComparison.OrdinalIgnoreCase);

				if (index == -1)
				{
					break;
				}

				yield return index;
			}
		}

		public static string Filter(this string s, bool letters = true, bool numbers = true, bool whitespace = true, bool symbols = true, bool punctuation = true)
		{
			StringBuilder sb = new StringBuilder();

			foreach (char c in s)
			{
				if ((!letters && char.IsLetter(c)) ||
				    (!numbers && char.IsNumber(c)) ||
				    (!whitespace && char.IsWhiteSpace(c)) ||
				    (!symbols && char.IsSymbol(c)) ||
				    (!punctuation && char.IsPunctuation(c)))
				{
					continue;
				}

				sb.Append(c);
			}

			return sb.ToString();
		}

		public static string FilterReplace(this string s, char replacement, bool merge, bool letters = true, bool numbers = true, bool whitespace = true, bool symbols = true, bool punctuation = true)
		{
			StringBuilder sb = new StringBuilder();

			bool wasFiltered = true;

			foreach (char c in s)
			{
				if ((!letters && char.IsLetter(c)) ||
				    (!numbers && char.IsNumber(c)) ||
				    (!whitespace && char.IsWhiteSpace(c)) ||
				    (!symbols && char.IsSymbol(c)) ||
				    (!punctuation && char.IsPunctuation(c)))
				{
					if (!merge || !wasFiltered)
					{
						sb.Append(replacement);
					}

					wasFiltered = true;
				}
				else
				{
					sb.Append(c);

					wasFiltered = false;
				}
			}

			return sb.ToString();
		}

		public static string RemoveNonAlphanumeric(this string s)
		{
			return s.Filter(symbols: false, whitespace: false, punctuation: false);
		}

		public static string Prettify(this string s)
		{
			return s.FirstCharacterToUpper().SplitWords(' ');
		}

		public static bool IsWordDelimiter(char c)
		{
			return char.IsWhiteSpace(c) || char.IsSymbol(c) || char.IsPunctuation(c);
		}

		public static bool IsWordBeginning(char? previous, char current, char? next)
		{
			bool isFirst = previous == null;
			bool isLast = next == null;

			bool isLetter = char.IsLetter(current);
			bool wasLetter = previous != null && char.IsLetter(previous.Value);

			bool isNumber = char.IsNumber(current);
			bool wasNumber = previous != null && char.IsNumber(previous.Value);

			bool isUpper = char.IsUpper(current);
			bool wasUpper = previous != null && char.IsUpper(previous.Value);

			bool isDelimiter = IsWordDelimiter(current);
			bool wasDelimiter = previous != null && IsWordDelimiter(previous.Value);

			bool willBeLower = next != null && char.IsLower(next.Value);

			return
				(!isDelimiter && isFirst) ||
				(!isDelimiter && wasDelimiter) ||
				(isLetter && wasLetter && isUpper && !wasUpper) || // camelCase => camel_Case
				(isLetter && wasLetter && isUpper && !isLast && willBeLower) || // => ABBRWord => ABBR_Word
				(isNumber && wasLetter) || // Vector3 => Vector_3
				(isLetter && wasNumber && isUpper && willBeLower); // Word1Word => Word_1_Word, Word1word => Word_1word
		}

		public static bool IsWordBeginning(string s, int index)
		{
			Ensure.That(nameof(index)).IsGte(index, 0);
			Ensure.That(nameof(index)).IsLt(index, s.Length);

			char? previous = index > 0 ? s[index - 1] : (char?)null;
			char current = s[index];
			char? next = index < s.Length - 1 ? s[index + 1] : (char?)null;

			return IsWordBeginning(previous, current, next);
		}

		public static string SplitWords(this string s, char separator)
		{
			StringBuilder sb = new StringBuilder();

			for (int i = 0; i < s.Length; i++)
			{
				char c = s[i];

				if (i > 0 && IsWordBeginning(s, i))
				{
					sb.Append(separator);
				}

				sb.Append(c);
			}

			return sb.ToString();
		}

		public static string RemoveConsecutiveCharacters(this string s, char c)
		{
			StringBuilder sb = new StringBuilder();

			char previous = '\0';

			foreach (char current in s)
			{
				if (current == c && current == previous) continue;
				sb.Append(current);
				previous = current;
			}

			return sb.ToString();
		}

		public static string ReplaceMultiple(this string s, HashSet<char> haystacks, char replacement)
		{
			Ensure.That(nameof(haystacks)).IsNotNull(haystacks);

			StringBuilder sb = new StringBuilder();

			foreach (char current in s)
			{
				sb.Append(haystacks.Contains(current) ? replacement : current);
			}

			return sb.ToString();
		}

		public static string Truncate(this string value, int maxLength, string suffix = "...")
		{
			return value.Length <= maxLength ? value : value.Substring(0, maxLength) + suffix;
		}

		public static string TrimEnd(this string source, string value)
		{
			return !source.EndsWith(value) ? source : source.Remove(source.LastIndexOf(value, StringComparison.Ordinal));
		}

		public static string TrimStart(this string source, string value)
		{
			return !source.StartsWith(value) ? source : source.Substring(value.Length);
		}

		public static string FirstCharacterToLower(this string s)
		{
			if (string.IsNullOrEmpty(s) || char.IsLower(s, 0))
			{
				return s;
			}

			return char.ToLowerInvariant(s[0]) + s.Substring(1);
		}

		public static string FirstCharacterToUpper(this string s)
		{
			if (string.IsNullOrEmpty(s) || char.IsUpper(s, 0))
			{
				return s;
			}

			return char.ToUpperInvariant(s[0]) + s.Substring(1);
		}

		public static string PartBefore(this string s, char c)
		{
			Ensure.That(nameof(s)).IsNotNull(s);

			int index = s.IndexOf(c);

			return index > 0 ? s.Substring(0, index) : s;
		}

		public static string PartAfter(this string s, char c)
		{
			Ensure.That(nameof(s)).IsNotNull(s);

			int index = s.IndexOf(c);

			return index > 0 ? s.Substring(index + 1) : s;
		}

		public static void PartsAround(this string s, char c, out string before, out string after)
		{
			Ensure.That(nameof(s)).IsNotNull(s);

			int index = s.IndexOf(c);

			if (index > 0)
			{
				before = s.Substring(0, index);
				after = s.Substring(index + 1);
			}
			else
			{
				before = s;
				after = null;
			}
		}

		// Faster equivalents for chars

		public static bool EndsWith(this string s, char c)
		{
			Ensure.That(nameof(s)).IsNotNull(s);

			if (s.Length == 0)
			{
				return false;
			}

			return s[s.Length - 1] == c;
		}

		public static bool StartsWith(this string s, char c)
		{
			Ensure.That(nameof(s)).IsNotNull(s);

			if (s.Length == 0)
			{
				return false;
			}

			return s[0] == c;
		}

		public static bool Contains(this string s, char c)
		{
			Ensure.That(nameof(s)).IsNotNull(s);

			return s.Any(t => t == c);
		}

		public static string NullIfEmpty(this string s)
		{
			return s != string.Empty ? s : null;
		}

		public static string ToBinaryString(this int value)
		{
			return Convert.ToString(value, 2).PadLeft(8, '0');
		}

		public static string ToBinaryString(this long value)
		{
			return Convert.ToString(value, 2).PadLeft(16, '0');
		}

		public static string ToBinaryString(this Enum value)
		{
			return Convert.ToString(Convert.ToInt64(value), 2).PadLeft(16, '0');
		}

		public static int CountIndices(this string s, char c)
		{
			int count = 0;

			foreach (char item in s)
			{
				if (c == item)
				{
					count++;
				}
			}

			return count;
		}

		public static bool IsGuid(string value)
		{
			return GuidRegex.IsMatch(value);
		}

		private static readonly Regex GuidRegex = new Regex(@"[a-fA-F0-9]{8}(\-[a-fA-F0-9]{4}){3}\-[a-fA-F0-9]{12}");
		private const string Ellipsis = "...";

		public static string PathEllipsis(string s, int maxLength)
		{
			if (s.Length < maxLength)
			{
				return s;
			}

			string fileName = Path.GetFileName(s);
			string directory = Path.GetDirectoryName(s);

			int maxDirectoryLength = maxLength - fileName.Length - Ellipsis.Length;

			if (maxDirectoryLength > 0)
			{
				return directory?.Substring(0, maxDirectoryLength) + Ellipsis + Path.DirectorySeparatorChar + fileName;
			}

			return Ellipsis + Path.DirectorySeparatorChar + fileName;
		}

		public static string UniqueName(string originalName, ICollection<string> existingNames)
		{
			string uniqueName = originalName;

			if (!existingNames.Contains(uniqueName)) return uniqueName;
			int suffix = 2;

			do
			{
				uniqueName = originalName + " " + suffix;
				suffix++;
			} while (existingNames.Contains(uniqueName));

			return uniqueName;
		}

		// https://stackoverflow.com/a/3961365/154502
		public static string WordWrap(string s, int width)
		{
			int pos, next;
			StringBuilder sb = new StringBuilder();
			string newline = Environment.NewLine;

			// Lucidity check
			if (width < 1)
			{
				return s;
			}
			
			// Parse each line of text
			for (pos = 0; pos < s.Length; pos = next)
			{
				// Find end of line
				int eol = s.IndexOf(newline, pos, StringComparison.Ordinal);

				if (eol == -1)
					next = eol = s.Length;
				else
					next = eol + newline.Length;

				// Copy this line of text, breaking into smaller lines as needed
				if (eol > pos)
				{
					do
					{
						int len = eol - pos;

						if (len > width)
							len = BreakLine(s, pos, width);

						sb.Append(s, pos, len);
						sb.Append(newline);

						// Trim whitespace following break
						pos += len;

						while (pos < eol && char.IsWhiteSpace(s[pos]))
							pos++;
					} while (eol > pos);
				}
				else sb.Append(newline); // Empty line
			}

			return sb.ToString();
		}
		
		public static int BreakLine(string text, int pos, int max)
		{
			// Find last whitespace in line
			int i = max - 1;
			while (i >= 0 && !char.IsWhiteSpace(text[pos + i]))
				i--;
			if (i < 0)
				return max; // No whitespace found; break at maximum length
			// Find start of whitespace
			while (i >= 0 && char.IsWhiteSpace(text[pos + i]))
				i--;
			// Return length of text before whitespace
			return i + 1;
		}

		public static string Pluralize(string s)
		{
			// Create a dictionary of exceptions that have to be checked first
			// This is very much not an exhaustive list!
			Dictionary<string, string> exceptions = new Dictionary<string, string>() {
				{ "man", "men" },
				{ "woman", "women" },
				{ "child", "children" },
				{ "tooth", "teeth" },
				{ "foot", "feet" },
				{ "mouse", "mice" },
				{ "belief", "beliefs" } };

			if (exceptions.ContainsKey(s.ToLowerInvariant()))
			{
				return exceptions[s.ToLowerInvariant()];
			}
			else if (s.EndsWith("y", StringComparison.OrdinalIgnoreCase) &&
			         !s.EndsWith("ay", StringComparison.OrdinalIgnoreCase) &&
			         !s.EndsWith("ey", StringComparison.OrdinalIgnoreCase) &&
			         !s.EndsWith("iy", StringComparison.OrdinalIgnoreCase) &&
			         !s.EndsWith("oy", StringComparison.OrdinalIgnoreCase) &&
			         !s.EndsWith("uy", StringComparison.OrdinalIgnoreCase))
			{
				return s.Substring(0, s.Length - 1) + "ies";
			}
			else if (s.EndsWith("us", StringComparison.InvariantCultureIgnoreCase))
			{
				// http://en.wikipedia.org/wiki/Plural_form_of_words_ending_in_-us
				return s + "es";
			}
			else if (s.EndsWith("ss", StringComparison.InvariantCultureIgnoreCase))
			{
				return s + "es";
			}
			else if (s.EndsWith("s", StringComparison.InvariantCultureIgnoreCase))
			{
				return s;
			}
			else if (s.EndsWith("x", StringComparison.InvariantCultureIgnoreCase) ||
			         s.EndsWith("ch", StringComparison.InvariantCultureIgnoreCase) ||
			         s.EndsWith("sh", StringComparison.InvariantCultureIgnoreCase))
			{
				return s + "es";
			}
			else if (s.EndsWith("f", StringComparison.InvariantCultureIgnoreCase) && s.Length > 1)
			{
				return s.Substring(0, s.Length - 1) + "ves";
			}
			else if (s.EndsWith("fe", StringComparison.InvariantCultureIgnoreCase) && s.Length > 2)
			{
				return s.Substring(0, s.Length - 2) + "ves";
			}
			else
			{
				return s + "s";
			}
		}
		
		// https://stackoverflow.com/questions/25259/how-does-stack-overflow-generate-its-seo-friendly-urls
        // Modified to perform C# camelCasing
        public static string camelCase(string name)
        {
            if (name == null) return "";

            const int maxlen = 80;
            int len = name.Length;
            bool prevdash = true;
            var sb = new StringBuilder(len);
            char c;

            for (int i = 0; i < len; i++)
            {
                c = name[i];
                if (i == 0 && (c >= '0' && c <= '9'))
                    sb.Append("_");
                if ((c >= 'a' && c <= 'z'))
                {
                    if (prevdash)
                    {
                        // Tricky way to Capitialize
                        sb.Append(char.ToUpper(c));
                    }
                    else
                    {
                        sb.Append(c);
                    }
                    prevdash = false;
                }
                else if (c >= 'A' && c <= 'Z' || (c >= '0' && c <= '9'))
                {
                    sb.Append(c);
                    prevdash = false;
                }
                else if (c == ' ' || c == ',' || c == '.' || c == '/' || 
                         c == '\\' || c == '-' || c == '_' || c == '=')
                {
                    if (!prevdash && sb.Length > 0)
                    {
                        prevdash = true;
                    }
                }
                else if ((int)c >= 128)
                {
                    int prevlen = sb.Length;
                    sb.Append(RemapInternationalCharToAscii(c));
                    if (prevlen != sb.Length) prevdash = false;
                }
                if (i == maxlen) break;
            }

            return prevdash ? sb.ToString().Substring(0, sb.Length - 1) : sb.ToString();
        }
        
        public static string RemapInternationalCharToAscii(char c)
        {
            string s = c.ToString().ToLowerInvariant();
            if ("àåáâäãåą".Contains(s))
            {
                return "a";
            }
            else if ("èéêëę".Contains(s))
            {
                return "e";
            }
            else if ("ìíîïı".Contains(s))
            {
                return "i";
            }
            else if ("òóôõöøőð".Contains(s))
            {
                return "o";
            }
            else if ("ùúûüŭů".Contains(s))
            {
                return "u";
            }
            else if ("çćčĉ".Contains(s))
            {
                return "c";
            }
            else if ("żźž".Contains(s))
            {
                return "z";
            }
            else if ("śşšŝ".Contains(s))
            {
                return "s";
            }
            else if ("ñń".Contains(s))
            {
                return "n";
            }
            else if ("ýÿ".Contains(s))
            {
                return "y";
            }
            else if ("ğĝ".Contains(s))
            {
                return "g";
            }
            else if (c == 'ř')
            {
                return "r";
            }
            else if (c == 'ł')
            {
                return "l";
            }
            else if (c == 'đ')
            {
                return "d";
            }
            else if (c == 'ß')
            {
                return "ss";
            }
            else if (c == 'Þ')
            {
                return "th";
            }
            else if (c == 'ĥ')
            {
                return "h";
            }
            else if (c == 'ĵ')
            {
                return "j";
            }
            else
            {
                return "";
            }
        }
	}
}