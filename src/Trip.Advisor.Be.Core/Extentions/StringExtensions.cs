using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace  Trip.Advisor.Be.Core.Extentions;

public static class StringExtensions
{
    public static T AsEnum<T>(this string value) => (T)Enum.Parse(typeof(T), value);

    public static string TextNormalize(this string text)
    {
        var sbReturn = new StringBuilder();
        var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();
        foreach (char letter in arrayText)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                sbReturn.Append(letter);
        }
        return sbReturn.ToString();
    }
    public static string RemoveAccents(this string str)
    {
        StringBuilder sbReturn = new();
        var arrayText = str.Normalize(NormalizationForm.FormD).ToCharArray();
        foreach (char letter in arrayText)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                sbReturn.Append(letter);
        }
        return sbReturn.ToString();
    }

    public static string FormatString(this string input, bool withSpecialChars = false)
    {
        input ??= string.Empty;
        input = input.Normalize(NormalizationForm.FormD);
        StringBuilder sb = new();

        foreach (char c in input)
        {
            if (withSpecialChars)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark && !char.IsWhiteSpace(c))
                {
                    sb.Append(c);
                }
            }
            else
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark && !char.IsWhiteSpace(c) && char.IsLetterOrDigit(c))
                {
                    sb.Append(c);
                }
            }
        }

        return sb.ToString().ToLower();
    }

    public static string RemoveAccentsAndEspecialCharacters(this string input)
    {
        input ??= string.Empty;
        input = input.RemoveAccents() ?? string.Empty;
        string pattern = "[^a-zA-Z0-9\\s]";
        return Regex.Replace(input, pattern, "",
                                RegexOptions.None).Replace("  ", " ");
    }

    public static DateTime GetDate(this string date)
    {
        DateTime myDate;
        if (!DateTime.TryParse(date, out myDate))
        {
            // handle parse failure
            return new DateTime();
        }
        return myDate;
    }

    public static string ToTitleCase(this string input)
    {
        TextInfo textInfo = CultureInfo.InvariantCulture.TextInfo;
        return textInfo.ToTitleCase(input.ToLowerInvariant());
    }

    public static int ToInt(this string value)
    {
        try
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(value.ToNumber()))
                return 0;

            decimal convert = Decimal.Parse(value,
                 NumberStyles.AllowParentheses |
                 NumberStyles.AllowLeadingWhite |
                 NumberStyles.AllowTrailingWhite |
                 NumberStyles.AllowThousands |
                 NumberStyles.AllowDecimalPoint |
                 NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture);
            return Convert.ToInt32(Math.Floor(convert));
        }
        catch
        {
            return 0;
        }
    }

    public static string ToNumber(this String texto)
    {
        if (string.IsNullOrEmpty(texto))
            return "";

        return Regex.Replace(texto, @"[^0-9]", "");
    }

    public static string LimitStringSize(string input, int size = 324 * 324)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        try
        {
            Encoding encoding = Encoding.Unicode; // UTF-16 (2 bytes por character)
            byte[] bytes = encoding.GetBytes(input);

            if (bytes.Length <= size)
            {
                return input;
            }
            else
            {
                int max = size / 2; // 2 bytes por character
                string substring = encoding.GetString(bytes, 0, max);
                return substring;
            }
        }
        catch
        {
            return input;
        }
    }

    public static string FindTextByRegex(string content, string pattern)
    {
        MatchCollection matches = Regex.Matches(content, pattern);

        return matches?.FirstOrDefault()?.Groups[1]?.Value;
    }
    public static T? ParseEnum<T>(this string value, T? defaultValue) where T : struct
    {
        try
        {
            T enumValue;
            if (!System.Enum.TryParse(value, true, out enumValue))
            {
                return defaultValue;
            }
            return enumValue;
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    public static decimal GetDecimal(this string valueString)
    {
        var num = string.Concat(valueString?.Where(n => "0123456789,.".Contains(n)));
        if (string.IsNullOrEmpty(num))
            return decimal.Zero;

        return Convert.ToDecimal(num);
    }

    public static string GetDisplayName(this string value, Type enumType)
    {
        var member = enumType.GetField(value);
        if (member != null)
            if (Attribute.GetCustomAttribute(member, typeof(DisplayAttribute)) is DisplayAttribute attribute)
                return attribute.Name;

        return value;
    }

    public static string FormatCompIds(List<string> wonPositionIds)
    {
        if (wonPositionIds is null || !wonPositionIds.Any())
            return string.Empty;

        string formattedPositions = "";

        for (int i = 0; i < wonPositionIds.Count(); i++)
        {
            if (i + 1 == wonPositionIds.Count())
                formattedPositions += $"COMP{wonPositionIds[i]}";
            else
                formattedPositions += $"COMP{wonPositionIds[i]}, ";
        }

        return formattedPositions;
    }

    public static string FormatComment(string s)
    {
        string result = "";
        while (s.Length > 45)
        {
            if (Char.IsWhiteSpace(s[44]) ||
                Char.IsWhiteSpace(s[45]) ||
                s[44] == '-' || s[45] == '-')
            {
                result += s.Substring(0, 45) + "<br />";
            }
            else
            {
                result += s.Substring(0, 45) + "-<br />";
            }
            s = s.Substring(45);
        }
        result += s;
        return result;
    }
}