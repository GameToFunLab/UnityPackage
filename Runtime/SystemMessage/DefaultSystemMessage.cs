using GameToFunLab.Core;
using UnityEngine;

namespace GameToFunLab.SystemMessage
{
    public class DefaultSystemMessage : MonoBehaviour
    {
        private int mDelay;
        private int mDuration;
        private Vector3 mMovePosition;

        private void Awake()
        {
        }

        public void Show(string message, int delay, int duration, Vector3 movePosition)
        {
            FgLogger.Log("showNotice " + delay + " / duration : " + duration + " / movePositionY : " + movePosition.y);
        }
    }
}
