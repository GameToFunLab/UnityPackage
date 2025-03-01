using UnityEditor;
using UnityEngine;

namespace GameToFunLab.Editor
{
    public static class Common
    {
        public static void OnGUITitle(string title)
        {
            GUILayout.Label($"[ {title} ]", EditorStyles.whiteLargeLabel);
        }
    }
}