using UnityEngine;

namespace PocketHeroes
{
    // References:
    // 1. DrowsyFoxDev: 'You are using Debug.Log wrong, do THIS': https://youtu.be/lRqR4YF8iQs
    // 2. samyam: 'Introduction to the Console and Types of Logging in Unity': https://youtu.be/YqhMhSLbeuw

    /// <summary>
    /// Better alternative to the direct approach of Unity Logging.
    /// </summary>
    public static class CustomDebugLogger
    {
        /// <summary>
        /// Throws a customized log
        /// </summary>
        public static void Log(this Object myObj, object msg,
        bool debug = true)
        {
            if (!debug) { return; }

            Debug.Log($"[{myObj.name}]: {msg}", myObj);
        }

        /// <summary>
        /// Throws a customized warning
        /// </summary>
        public static void LogWarning(this Object myObj, object msg, 
            bool debug = true)
        {
            if (!debug) { return; }

            string obj = $"[{myObj.name}]".ColorizeString(Color.yellow);
            Debug.LogWarning($"{obj}: {msg}", myObj);
        }

        /// <summary>
        /// Throws a customized error
        /// </summary>
        public static void LogError(this Object myObj, object msg)
        {
            string obj = $"[{myObj.name}]".ColorizeString(Color.red);
            Debug.LogError($"{obj}: {msg}", myObj);
        }

        /// <summary>
        /// Throws a custom exception
        /// </summary>
        public static void LogException(this Object myObj, 
            System.Exception exception, object message = null)
        {
            message = message == null ? "" : ": " + message;
            string obj = $"[{myObj.name}]".ColorizeString(Color.red);
            Debug.LogError($"{obj}: {exception}{message}", myObj);
        }

        /// <summary>
        /// Only prints if condition is true.
        /// </summary>
        public static void LogAssert(this Object myObj, bool assertation,
            bool debug = false, object message = null)
        {
            if (!debug || assertation) return;

            message = message == null ? "!" : $": {message}";

            string obj = $"[{myObj.name}]".ColorizeString(Color.magenta);
            Debug.LogWarning($"{obj}: Assertation is false{message}", myObj);
        }

        static string ColorizeString(this string text, Color color) =>
            $"<color={color}>{text}</color>";
    }
}
