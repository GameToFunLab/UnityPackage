using UnityEngine;

namespace GameToFunLab.Maps
{
    public interface IMap
    {
        MapConstants.Type Type { get; set; }
        MapConstants.SubType SubType { get; set; }
        int Unum { get; set; } // 현재 맵 unum
        float FadeDuration { get; set; }  // 페이드 인 지속 시간
        Vector3 PlaySpawnPosition { get; set; }
        
        void CreateMap();
    }
}