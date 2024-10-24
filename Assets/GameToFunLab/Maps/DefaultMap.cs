using UnityEngine;

namespace GameToFunLab.Maps
{
    public class DefaultMap : MonoBehaviour, IMap
    {
        public int Unum { get; set; }
        public float FadeDuration { get; set; }
        public Vector3 PlaySpawnPosition { get; set; }
        public MapConstants.Type Type { get; set; }
        public MapConstants.SubType SubType { get; set; }
        
        private int chapterNumber;
        private string chapterName;

        /// <summary>
        /// 맵 사이즈 구하기
        /// </summary>
        /// <returns></returns>
        protected virtual (float width, float height) GetMapSize()
        {
            float totalWidth = 0;
            float totalHeight = 0;
            return (totalWidth, totalHeight);
        }

        protected virtual void InitializeByEditor()
        {
            
        }
        
        public virtual void CreateMap()
        {
            // Logger.Log("타일맵 프리팹 로드 완료");
        }
    }
}
