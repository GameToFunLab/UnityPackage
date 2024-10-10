using GameToFunLab.Maps;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GameToFunLab.Core.Maps
{
    public class DefaultMap : MonoBehaviour
    {
        private int chapterNumber;
        private string chapterName;
        private MapManager.Type mapType;
        private MapManager.SubType mapSubType;
        public Tilemap tilemap;

        public void Initialize(int vnum, string name, MapManager.Type type, MapManager.SubType subType)
        {
            chapterNumber = vnum;
            chapterName = name;
            mapType = type;
            mapSubType = subType;
        }
        /// <summary>
        /// 같은 장의 맵인지 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool IsSameChapter(int value)
        {
            return chapterNumber == value;
        }
        /// <summary>
        /// 현재 맵에 연결된 장 가져오기
        /// </summary>
        /// <returns></returns>
        public int GetChapterNumber()
        {
            return chapterNumber;
        }
        /// <summary>
        /// 맵 사이즈 구하기
        /// </summary>
        /// <returns></returns>
        public (float width, float height) GetMapSize()
        {
            // 실제 타일이 배치된 경계를 추적하기 위한 변수들
            Vector3Int min = new Vector3Int(int.MaxValue, int.MaxValue, int.MaxValue);
            Vector3Int max = new Vector3Int(int.MinValue, int.MinValue, int.MinValue);

            // 타일맵의 모든 셀을 순회하며 타일이 있는 위치를 확인
            foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
            {
                if (tilemap.HasTile(pos))
                {
                    min = Vector3Int.Min(min, pos);
                    max = Vector3Int.Max(max, pos);
                }
            }

            // 타일이 있는 범위의 크기를 계산
            Vector3Int size = max - min + Vector3Int.one;

            // 셀 크기를 고려하여 실제 월드 공간에서의 크기 계산
            Vector3 cellSize = tilemap.cellSize;
            float totalWidth = size.x * cellSize.x;
            float totalHeight = size.y * cellSize.y;

            // Logger.Log("Total Tilemap Width: " + totalWidth + ", Total Tilemap Height: " + totalHeight);

            return (totalWidth, totalHeight);
        }
    }
}
