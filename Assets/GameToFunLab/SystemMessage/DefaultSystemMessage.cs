using TMPro;
using UnityEngine;

namespace GameToFunLab.Core.SystemMessage
{
    public class DefaultSystemMessage : MonoBehaviour
    {
        private TextMeshProUGUI textMessage;
        private int mDelay;
        private int mDuration;
        private Vector3 mMovePosition;

        private void Awake()
        {
            textMessage = GetComponent<TextMeshProUGUI>();
        }

        public void Show(string message, int delay, int duration, Vector3 movePosition)
        {
            FgLogger.Log("showNotice " + delay + " / duration : " + duration + " / movePositionY : " + movePosition.y);
            //this.delay = delay;
            //this.duration = duration;
            //this.movePosition = movePosition;

            textMessage.text = message;
            //txtMessage.DOFade(0, duration).SetDelay(delay).SetEase(Ease.OutCubic);
            //float y = this.transform.position.y + 100;
            //txtMessage.transform.DOMoveY(y, duration).SetDelay(delay).SetEase(Ease.OutCubic).OnComplete(MyCallback);
        }
    }
}
