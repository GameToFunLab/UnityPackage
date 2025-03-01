using GameToFunLab.Core;
using GameToFunLab.Scenes;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameToFunLab.UI
{
    public class UIWindow : MonoBehaviour, IDropHandler
    {
        public UIWindowManager.WindowUnum unum;
        public string id;

        public GameObject[] slots;
        public GameObject[] icons;

        private UIWindowFade uiWindowFade;

        protected virtual void Awake()
        {
            this.gameObject.AddComponent<CanvasGroup>();
            uiWindowFade = gameObject.AddComponent<UIWindowFade>();
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
        
        }
        /// <summary>
        /// 아이콘 가져오기
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public GameObject GetIconByIndex(int index)
        {
            if (icons.Length == 0 || icons[index] == null)
            {
                FgLogger.LogError("no icon. index: " +index);
                return null;
            }
        
            return icons[index];
        }
        /// <summary>
        /// 아이콘 가져오기
        /// </summary>
        /// <param name="unum"></param>
        /// <returns></returns>
        public GameObject GetIconByUnum(int unum)
        {
            if (icons.Length == 0)
            {
                FgLogger.LogError("icons.Length is zero");
                return null;
            }

            foreach (var icon in icons)
            {
                if (icon == null) continue;
                UIIcon uiIcon = icon.GetComponent<UIIcon>();
                if (uiIcon == null) continue;
                if (uiIcon.unum == unum)
                {
                    return icon;
                }
            }

            return null;
        }
        /// <summary>
        /// 아이콘 정보 지워주기 
        /// </summary>
        /// <param name="slotIndex"></param>
        public void DetachIcon(int slotIndex)
        {
            if (icons.Length <= 0) return;
            GameObject icon = icons[slotIndex];
            if (icon == null || icon.GetComponent<UIIcon>() == null)
            {
                FgLogger.LogError("dont exist icon. slot index: " +slotIndex);
                return;
            }
            icon.GetComponent<UIIcon>().ClearIconInfos();
            OnDetachIcon(slotIndex);
        }

        protected virtual void OnDetachIcon(int slotIndex)
        {
            
        }
        /// <summary>
        /// 아이콘 붙여주기 
        /// </summary>
        /// <param name="icon"></param>
        /// <param name="slotIndex"></param>
        public void SetIcon(GameObject icon, int slotIndex)
        {
            GameObject slot = slots[slotIndex];
            if (slot == null)
            {
                FgLogger.LogError("dont exist slot. slot index: " +slotIndex);
                return;
            }
            icon.transform.SetParent(slot.transform);
            icon.transform.position = slot.transform.position;
            icon.GetComponent<UIIcon>().iconIndex = slotIndex;
            icon.GetComponent<UIIcon>().iconSlotIndex = slotIndex;
            icons[slotIndex] = icon;
            
            OnSetIcon(icon, slotIndex);
        }
        protected virtual void OnSetIcon(GameObject icon, int slotIndex)
        {
            
        }
        public virtual void OnClickIcon(UIIcon icon)
        {
        
        }
        public virtual void OnDrop(PointerEventData eventData)
        {
        }
        /// <summary>
        /// 아이콘 위에서 드래그가 끝났을때 처리 
        /// </summary>
        /// <param name="droppedIcon">드랍한 한 아이콘</param>
        /// <param name="originalIcon">드랍되는 곳에 있는 아이콘</param>
        public virtual void OnEndDragInIcon(GameObject droppedIcon, GameObject originalIcon)
        {
            FgLogger.Log("OnEndDragInIcon");
        }
        /// <summary>
        /// 아이템 구매
        /// </summary>
        /// <param name="item"></param>
        public virtual void PurchaseItem(GameObject item)
        {
        }
        /// <summary>
        /// 아이콘 등급 이미지 path 가져오기
        /// </summary>
        /// <param name="valueGrade"></param>
        /// <param name="valueGradeLevel"></param>
        /// <returns></returns>
        public static string GetIconGradePath(UIIcon.Grade valueGrade, int valueGradeLevel)
        {
            return $"Images/UI/{UIIcon.IconGradeImagePath[valueGrade]}{valueGradeLevel}";
        }
        /// <summary>
        /// 윈도우가 show 가 된 후 처리 
        /// </summary>
        public virtual void OnShow(bool show)
        {
            
        }
        public virtual void OnClickClose()
        {
            SceneGame.Instance.uIWindowManager.ShowWindow(unum, false);
        }

        public void Show(bool show)
        {
            if (uiWindowFade == null) return;
            if (gameObject.activeSelf == show) return;
            if (show)
            {
                uiWindowFade.ShowPanel();
            }
            else
            {
                uiWindowFade.HidePanel();
            }
        }
    }
}
