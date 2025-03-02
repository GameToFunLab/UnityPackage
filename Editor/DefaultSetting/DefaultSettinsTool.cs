using UnityEditor;

namespace GameToFunLab.Editor.DefaultSetting
{
    public class DefaultSettinsTool : EditorWindow
    {
        private readonly SettingTags settingTags = new SettingTags();
        private readonly SettingManagers settingManagers = new SettingManagers();
        private readonly SettingResolution settingResolution = new SettingResolution();
        private readonly SettingAddressable settingAddressable = new SettingAddressable();
        
        [MenuItem("GameToFunLab/기본 셋팅하기")]
        public static void ShowWindow()
        {
            GetWindow<DefaultSettinsTool>("기본 셋팅하기");
        }

        private void OnGUI()
        {
            settingResolution.OnGUI();
            EditorGUILayout.Space(10);
            
            settingTags.OnGUI();
            EditorGUILayout.Space(10);
            
            // todo 툴로 추가할 수 있는 방법 찾기. 현재는 class 이름으로 addComponent 를 하지 못 한다.
            // settingManagers.OnGUI();
            EditorGUILayout.Space(10);
            settingAddressable.OnGUI();
        }
    }
}