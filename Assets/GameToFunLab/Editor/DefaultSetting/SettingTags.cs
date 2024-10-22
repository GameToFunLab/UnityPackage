using UnityEditor;
using UnityEngine;

namespace GameToFunLab.Editor.DefaultSetting
{
    public class SettingTags
    {
        private string[] tagsToAdd = { "Monster", "Npc", "Map", "ButtonNpcQuest", "MapObjectWarp" };
        
        public void AddTags()
        {
            // Tag 추가를 위해 Unity의 TagManager를 가져옴
            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty tagsProp = tagManager.FindProperty("tags");

            // 원하는 태그 목록
            foreach (string tag in tagsToAdd)
            {
                if (!TagExists(tagsProp, tag))
                {
                    tagsProp.InsertArrayElementAtIndex(tagsProp.arraySize);
                    tagsProp.GetArrayElementAtIndex(tagsProp.arraySize - 1).stringValue = tag;
                }
            }

            // 변경 사항 저장
            tagManager.ApplyModifiedProperties();
            AssetDatabase.SaveAssets(); // 변경 사항 저장
            
            // Inspector 갱신
            EditorUtility.SetDirty(tagManager.targetObject); // TargetObject를 '더럽힘' 상태로 만들어 갱신 유도
            AssetDatabase.Refresh(); // 에디터 갱신
            Debug.Log("태그 추가 완료");
            bool res1 = EditorUtility.DisplayDialog("태그 추가하기", "태그 추가 완료", "OK");
        }

        private bool TagExists(SerializedProperty tagsProp, string tag)
        {
            for (int i = 0; i < tagsProp.arraySize; i++)
            {
                if (tagsProp.GetArrayElementAtIndex(i).stringValue.Equals(tag))
                {
                    return true;
                }
            }
            return false;
        }
    }
}