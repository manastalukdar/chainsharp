using System;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace chainsharp.logging
{
    public static class LoggingUtils
    {
        #region Public Methods

        /// <summary>Receives a string array and transforms it into a valid CSV string.</summary>
        public static string ArrayToCsv(params string[] stringArray)
        {
            var stringBuilder = new StringBuilder();

            // We put quotes at the beggining
            stringBuilder.Append("\"");

            // We convert to CSV
            stringBuilder.Append(string.Join("\",\"", stringArray));

            // We add the final quotes
            stringBuilder.Append("\"");

            return stringBuilder.ToString();
        }

        /// <summary>Receives an object array, converts its to string array then transforms it into a valid CSV string.</summary>
        public static string ArrayToCsv(params object[] objectArray)
        {
            var stringArray = SafeCastToStringArray(objectArray);
            return ArrayToCsv(stringArray);
        }

        public static string ArrayToJson(params string[] stringArray)
        {
            JArray jsonArray = new JArray(stringArray);
            return jsonArray.ToString();
        }

        /// <summary>
        /// Conversts from object[] to string[], where null values become the empty string.
        /// </summary>
        public static string[] SafeCastToStringArray(params object[] objects)
        {
            Func<object, string> nullToStringFunc = obj => obj?.ToString() ?? string.Empty;
            return objects.Select(x => nullToStringFunc(x)).ToArray();
        }

        #endregion Public Methods
    }
}
