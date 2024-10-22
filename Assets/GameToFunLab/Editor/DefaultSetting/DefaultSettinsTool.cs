using UnityEditor;
using UnityEngine;

namespace GameToFunLab.Editor.DefaultSetting
{
    public class DefaultSettinsTool : EditorWindow
    {
        private int resolutionWidth = 0;
        private int resolutionHeight = 0;
        private SettingTags settingTags = new SettingTags();
        private SettingResolution settingResolution = new SettingResolution();
        
        [MenuItem("GameToFunLab/기본 셋팅하기")]
        public static void ShowWindow()
        {
            GetWindow<DefaultSettinsTool>("기본 셋팅하기");
        }

        private void OnGUI()
        {
            GUILayout.Label("해상도 정하기", EditorStyles.boldLabel);

            resolutionWidth = EditorGUILayout.IntField("width", resolutionWidth);
            resolutionHeight = EditorGUILayout.IntField("height", resolutionHeight);
            if (GUILayout.Button("적용하기"))
            {
                settingResolution.SetResolution(resolutionWidth, resolutionHeight);
            }
            
            GUILayout.Label("태그 추가하기", EditorStyles.boldLabel);

            if (GUILayout.Button("태그 추가하기"))
            {
                settingTags.AddTags();
            }
        }

        private void SetResolution()
        {
            
        }
    }
}