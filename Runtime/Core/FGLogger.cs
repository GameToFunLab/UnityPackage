using UnityEngine;

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
        /// <summary>
        /// value 값이 0 인지 체크한 후 에러 메세지 보여주기 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="errorLogMessage"></param>
        /// <returns></returns>
        public static bool IsNullInt(int value, string errorLogMessage)
        {
            if (value <= 0)
            {
                LogError(errorLogMessage);
                return true;
            }
            return false;
        }
        /// <summary>
        /// value 값이 null 인지 체크한 후 에러메시지 보여주기 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="errorLogMessage"></param>
        /// <returns></returns>
        public static bool IsNullGameObject(GameObject value, string errorLogMessage)
        {
            if (value == null)
            {
                LogError(errorLogMessage);
                return true;
            }
            return false;
        }
    }
}
#endif