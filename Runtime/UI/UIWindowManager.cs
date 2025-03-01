using GameToFunLab.Core;
using GameToFunLab.Scenes;
using UnityEngine;
using UnityEngine.UI;

namespace GameToFunLab.UI
{
    public class UIWindowManager : MonoBehaviour
    {
        public enum WindowUnum 
        {
        }

        public UIWindow[] uiWindows;
        
        public GameObject prefabFloatingText;
        public VerticalLayoutGroup debugMessageVerticalLayoutGroup;
        public GameObject prefabDebugMessage;

        // Start is called before the first frame update
        private void Start()
        {
            InitializationShowDisable();
        }
        /// <summary>
        /// 초기화시 안보여야 하는 윈도우 처리 
        /// </summary>
        private void InitializationShowDisable()
        {
        }
        /// <summary>
        /// 윈도우 보임/안보임 처리 
        /// </summary>
        /// <param name="unum"></param>
        /// <param name="show"></param>
        public void ShowWindow(WindowUnum unum, bool show)
        {
            UIWindow uiWindow = GetUIWindowByUnum<UIWindow>(unum);
            if (uiWindow == null) {
                FgLogger.LogWarning("dont exist window. unum:"+unum);
                return;
            }
            if (uiWindow.gameObject.activeSelf == show) return;
            UIWindowFade uiWindowFade = uiWindow.GetComponent<UIWindowFade>();
            if (uiWindowFade == null) return;
            if (show) {
                uiWindowFade.ShowPanel();
            }
            else {
                uiWindowFade.HidePanel();
            }

            // uiWindow.OnShow(show);
            // window.SetActive(show);
        }
        /// <summary>
        /// 윈도우 간에 아이콘 이동시키기 
        /// </summary>
        /// <param name="fromWindowUnum"></param>
        /// <param name="fromIndex"></param>
        /// <param name="toWindowUnum"></param>
        /// <param name="toIndex"></param>
        public void MoveIcon(WindowUnum fromWindowUnum, int fromIndex, WindowUnum toWindowUnum, int toIndex)
        {
            UIWindow fromWindow = GetUIWindowByUnum<UIWindow>(fromWindowUnum);
            GameObject fromIcon = null;
            if (fromWindow != null)
            {
                fromIcon = fromWindow.GetIconByIndex(fromIndex);
            }

            UIWindow toWindow = GetUIWindowByUnum<UIWindow>(toWindowUnum);
            GameObject toIcon = null;
            if (toWindow != null)
            {
                toIcon = toWindow.GetIconByIndex(toIndex);
            }

            if (fromWindow == null || toWindow == null)
            {
                FgLogger.LogError("dont exist fromUIWindow or toUIWindow. fromWindow: "+fromWindowUnum+" / toWindow: "+toWindow);
                return;
            }
            if (fromIcon == null)
            {
                FgLogger.LogError("dont exist fromIcon. fromWindow: "+fromWindowUnum+" / fromIndex: "+fromIndex);
                return;
            }

            if (toIcon != null)
            {
                // fromWindow.DetachIcon(fromIndex);
                // toWindow.DetachIcon(toIndex);
            
                fromWindow.SetIcon(toIcon, fromIndex);
                toWindow.SetIcon(fromIcon, toIndex);
            }
            else
            {
                // fromWindow.DetachIcon(fromIndex);
                toWindow.SetIcon(fromIcon, toIndex);
            }
        }
        /// <summary>
        /// 특정 윈도우에서 아이콘 가져오기
        /// </summary>
        /// <param name="srcWindowUnum"></param>
        /// <param name="srcIndex"></param>
        /// <returns></returns>
        private GameObject GetIconByWindowUnum(WindowUnum srcWindowUnum, int srcIndex)
        {
            UIWindow uiWindow = GetUIWindowByUnum<UIWindow>(srcWindowUnum);
            if (uiWindow == null)
            {
                FgLogger.LogError("dont exist window. window unum: "+srcWindowUnum);
                return null;
            }
            return uiWindow.GetIconByIndex(srcIndex);
        }
        /// <summary>
        /// 윈도우에 아이콘 등록하기
        /// </summary>
        /// <param name="srcWindowUnum"></param>
        /// <param name="srcIndex"></param>
        /// <param name="toWindowUnum"></param>
        /// <param name="toIndex"></param>
        /// <returns></returns>
        public GameObject RegisterIcon(WindowUnum srcWindowUnum, int srcIndex, WindowUnum toWindowUnum, int toIndex)
        {
            GameObject srcIcon = this.GetIconByWindowUnum(srcWindowUnum, srcIndex);
            if(srcIcon == null) 
            {
                FgLogger.LogError("dont exist srcIcon. window unum: "+srcWindowUnum+ " / srcIndex: "+srcIndex);
                return null;
            }

            UIIcon uiIcon = srcIcon.GetComponent<UIIcon>();
            if(uiIcon == null) 
            {
                FgLogger.LogError("dont exist srcIcon FG_UIIcon. window unum: "+srcWindowUnum+ " / srcIndex: "+srcIndex);
                return null;
            }
            GameObject registerIcon = this.GetIconByWindowUnum(toWindowUnum, toIndex);
            if (registerIcon == null)
            {
                registerIcon = AddIcon(toWindowUnum, toIndex, UIIcon.Type.Skill, uiIcon.unum, 0);
            }
            else
            {
                registerIcon.GetComponent<UIIcon>().ChangeInfoByUnum(uiIcon.unum);
                UIWindow uiWindow = GetUIWindowByUnum<UIWindow>(toWindowUnum);
                if (uiWindow != null)
                {
                    uiWindow.SetIcon(registerIcon, toIndex);
                }
            }
            return registerIcon;
        }
        /// <summary>
        /// 아이콘 추가하기. 보류
        /// </summary>
        /// <param name="toWindowUnum"></param>
        /// <param name="toIndex"></param>
        /// <param name="type"></param>
        /// <param name="unum"></param>
        /// <param name="vid"></param>
        /// <returns></returns>
        private GameObject AddIcon(WindowUnum toWindowUnum, int toIndex, UIIcon.Type type, int unum, int vid)
        {
            // let icon = this.createIcon(type, unum, dragging, click, byClient, item_class, authority);
            // if(!icon) return null;
            //
            // icon.vid = vid;
            // if (byClient === true)
            //     icon.vid = unum;
            //
            // if(wnd === Def.WINDOW.SKILLTIER) {
            //     icon.createName();
            // }
            //
            // if (wnd === Def.WINDOW.QUICKSLOT_ITEM) {
            //     wnd = Def.WINDOW.HUDMANAGER;
            //     index = index + Def.QSLOT_SKILL_COUNT;
            // }
            //
            // if(wnd === Def.WINDOW.QUICKSLOT) {
            //     wnd = Def.WINDOW.HUDMANAGER;
            //     // index = index % Def.QSLOT_SKILL_COUNT;
            // }
            //
            // if(wnd == Def.WINDOW.REPURCHASE_ITEM){
            //     wnd = Def.WINDOW.SHOP;
            // }
            //
            // let wndObj = this.getWindow(wnd);
            // if(wndObj == undefined) {
            //     if(DEBUG) {
            //         if (Config.debug_icons) {
            //             console.log('ADD_ICON: UNDEFINED %d', wnd);
            //         }
            //     }
            //     return null;
            // }
            //
            //
            // wndObj.setIcon(icon, index);
            // return icon;
            return null;
        }
        /// <summary>
        /// UIWindow 찾기 
        /// </summary>
        /// <param name="windowUnum"></param>
        /// <returns></returns>
        public T GetUIWindowByUnum<T>(WindowUnum windowUnum) where T : UIWindow
        {
            UIWindow uiWindow = uiWindows[(int)windowUnum];
            if (uiWindow == null)
            {
                FgLogger.LogError("dont exist window. window unum: "+windowUnum);
                return null;
            }

            return uiWindow as T;
        }
        /// <summary>
        /// 특정 윈도우에서 아이콘 지우기
        /// </summary>
        /// <param name="windowUnum"></param>
        /// <param name="slotIndex"></param>
        public void RemoveIcon(WindowUnum windowUnum, int slotIndex)
        {
            UIWindow uiWindow = GetUIWindowByUnum<UIWindow>(windowUnum);
            if (uiWindow == null)
            {
                FgLogger.LogError("dont exist window. window unum: "+windowUnum);
                return;
            }
            uiWindow.DetachIcon(slotIndex);
        }
        public void ShowFloatingText(string text, Vector3 position)
        {
            ShowFloatingText(text, position, Color.yellow);
        }
        public void ShowFloatingText(string text, Vector3 position, float fontSize)
        {
            ShowFloatingText(text, position, Color.yellow);
        }
        public void ShowFloatingText(string text, Vector3 position, Color textColor, float fontSize = 55)
        {
            UIFloatingText uiFloatingText = Instantiate(prefabFloatingText, SceneGame.Instance.uIWindowManager.transform).GetComponent<UIFloatingText>();
            uiFloatingText.SetText(text);
            uiFloatingText.SetColor(textColor);
            uiFloatingText.SetFontSize(fontSize);
            uiFloatingText.transform.position = position;
        }
        public void AddDebugMessage(string text)
        {
            if (debugMessageVerticalLayoutGroup == null || prefabDebugMessage == null || GameObject.Find("ScrollViewDebugMessage") == null) return;
            Instantiate(prefabDebugMessage, debugMessageVerticalLayoutGroup.transform);
            LayoutRebuilder.ForceRebuildLayoutImmediate(debugMessageVerticalLayoutGroup.GetComponent<RectTransform >());
            GameObject.Find("ScrollViewDebugMessage").GetComponent<ScrollRect>().verticalNormalizedPosition = 0;
        }
        /// <summary>
        /// 썸네일, 칭호 아이콘 이미지 변경하기
        /// </summary>
        /// <param name="index"></param>
        public void ChangeHudCharacterInfo(string index)
        {
        }

        public bool IsShowByWindowUnum(WindowUnum windowUnum)
        {
            UIWindow uiWindow = GetUIWindowByUnum<UIWindow>(windowUnum);
            if (uiWindow == null) return false;
            return uiWindow.gameObject.activeSelf;
        }
    }
}
