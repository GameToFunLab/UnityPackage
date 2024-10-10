#if DEVELOPMENT_BUILD || UNITY_EDITOR
namespace GameToFunLab.Core
{
    public static class FgLogger
    {
        public static void Log(string message)
        {
            UnityEngine.Debug.Log(message);
        }
        public static void Log(string message, params object[] parameters)
        {
            message = string.Format(message, parameters);
            UnityEngine.Debug.Log(message);
        }
        public static void Log(object message)
        {
            UnityEngine.Debug.Log(message);
        }
        public static void LogWarning(string message)
        {
            UnityEngine.Debug.LogWarning(message);
        }
        public static void LogWarning(string message, params object[] parameters)
        {
            message = string.Format(message, parameters);
            UnityEngine.Debug.LogWarning(message);
        }
        public static void LogWarning(object message)
        {
            UnityEngine.Debug.LogWarning(message);
        }
        public static void LogError(string message)
        {
            UnityEngine.Debug.LogError(message);
        }
        public static void LogError(string message, params object[] parameters)
        {
            message = string.Format(message, parameters);
            UnityEngine.Debug.LogError(message);
        }
        public static void LogError(object message)
        {
            UnityEngine.Debug.LogError(message);
        }
        public static void LogFormat(string format, params object[] args)
        {
            UnityEngine.Debug.LogFormat(format, args);
        }
    }
}
#endif