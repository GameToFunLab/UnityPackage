using UnityEditor;
using UnityEngine;

namespace GameToFunLab.Editor.DefaultSetting
{
    public class SettingResolution
    {
        private int resolutionWidth;
        private int resolutionHeight;
        public void OnGUI()
        {
            Common.OnGUITitle("해상도 정하기");

            resolutionWidth = EditorGUILayout.IntField("width", resolutionWidth);
            resolutionHeight = EditorGUILayout.IntField("height", resolutionHeight);
            if (GUILayout.Button("적용하기"))
            {
                SetResolution(resolutionWidth, resolutionHeight);
            }
        }
        private void SetResolution(int width, int height)
        {
            
        }
    }
}