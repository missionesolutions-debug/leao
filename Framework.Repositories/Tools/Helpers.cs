using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Linq;

namespace Framework.Tools
{
    public static class Helpers
    {
        public static int ToInt(this string texto)
        {
            int numero = 0;
            int.TryParse(texto, out numero);

            return numero;
        }

        public static int? GreaterThanZeroOrNull(this int value)
        {
            return value > 0 ? value : null;
        }

        public static int? GreaterThanZeroOrNull(this decimal value)
        {
            return value > 0 ? Convert.ToInt32(value) : null;
        }

        public static int ToInt(this object texto)
        {
            return texto.ToString().ToInt();
        }

        public static Int16 ToInt16(this string value)
        {
            Int16 result = 0;
            Int16.TryParse(value, out result);

            return result;
        }

        public static Int32 ToInt32(this string value)
        {
            Int32 result = 0;
            Int32.TryParse(value, out result);

            return result;
        }

        public static DateTime ToTimeZone(this DateTime datetime, string timezone = "E. South America Standard Time")
        {
            TimeZoneInfo esatZone = TimeZoneInfo.FindSystemTimeZoneById(timezone);
            var localeDateTime = TimeZoneInfo.ConvertTimeFromUtc(datetime, esatZone);

            return localeDateTime;
        }

        public static bool IsNullOrEmpty(this string texto)
        {
            return texto == "" || texto == null ? true : false;
        }

        public static bool IsNullOrZero(this int? number)
        {
            return number is null || number == 0;
        }
        public static decimal ToDecimal(this string texto)
        {
            decimal valor = 0m;
            decimal.TryParse(texto, out valor);

            return valor;
        }
        public static string ToDateTime(this DateTime texto)
        {
            return texto.ToString("dd/MM/yyyy HH:mm");
        }

        public static string SeparateLines(string[] content)
        {
            String result = "";


            return result;
        }

        public static string FormatCategories(string[] categoryId)
        {
            String result = "";

            for (int i = 0; i < categoryId.Length; i++)
            {
                result += categoryId[i] + ";";
            }

            return result;
        }
        public static string ToDateTime(this DateTime texto, string mascara)
        {
            return texto.ToString(mascara);
        }
        public static string LowercaseFirstLetter(this string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToLower(s[0]) + s.Substring(1);
        }

        public static string UppercaseFirstLetter(this string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }
        public static string RemoverAcentos(this string inputString)
        {
            if ((inputString == "") || (inputString == null))
                return "";

            string normalizedString = inputString.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < normalizedString.Length; i++)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(normalizedString[i]);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(normalizedString[i]);
                }
            }
            inputString = sb.ToString().Normalize(NormalizationForm.FormC);
            return inputString;
        }

        public static string LimparUrl(this string texto)
        {
            if (!String.IsNullOrEmpty(texto))
            {
                string s = texto.Normalize(NormalizationForm.FormD);

                StringBuilder sb = new StringBuilder();

                for (int k = 0; k < s.Length; k++)
                {
                    UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(s[k]);
                    if (uc != UnicodeCategory.NonSpacingMark)
                    {
                        sb.Append(s[k]);
                    }
                }

                return sb.ToString().ToLower().Replace(" ", "-")
                    .Replace(".", "-")
                    .Replace(",", "-")
                    .Replace(";", "-")
                    .Replace(":", "-")
                    .Replace("~", "-")
                    .Replace("]", "-")
                    .Replace("[", "-")
                    .Replace("+", "-")
                    .Replace("-", "-")
                    .Replace("=", "-")
                    .Replace("-", "-")
                    .Replace("!", "-")
                    .Replace("@", "-")
                    .Replace("#", "-")
                    .Replace("$", "-")
                    .Replace("%", "-")
                    .Replace("&", "-")
                    .Replace("*", "-")
                    .Replace("(", "-")
                    .Replace("?", "-")
                    .Replace(")", "-")
                    .Replace("\\", "-")
                    .Replace("/", "-")
                    .Replace("|", "-")
                    .Replace("'", "-")
                    .Replace("\"", "-");
            }
            else
            {
                return "";
            }
        }

        #region IEnumerable
        public static IEnumerable<T> Random<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(x => Guid.NewGuid());
        }
        #endregion

        #region DateTime
        public static DateTime ToDateTime(this string texto)
        {
            DateTime data = new DateTime();
            DateTime.TryParse(texto, out data);

            return data;
        }
        public static DateTime ToDateTime(this string texto, string mascara)
        {
            DateTime data = new DateTime();
            DateTime.TryParseExact(texto, mascara, CultureInfo.InvariantCulture, DateTimeStyles.None, out data);

            return data;
        }
        private static string BuildUrl(string Nome)
        {
            String Url = Nome.RemoverAcentos().LimparUrl();

            return Url;
        }

        public static string GenerateGUID()
        {
            Guid guid = Guid.NewGuid();

            return guid.ToString();
        }

        public static string GenerateGuid()
        {
            Guid obj = Guid.NewGuid();

            return obj.ToString();
        }

        #endregion
    }
}