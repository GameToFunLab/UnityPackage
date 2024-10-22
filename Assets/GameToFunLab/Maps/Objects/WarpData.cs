using UnityEngine;

namespace GameToFunLab.Maps.Objects
{
    public class WarpData
    {
        public int MapUnum;
        public int ToMapUnum; // 이동할 map unum
        public float x, y, z;
        public float RotationX, RotationY, RotationZ;
        public float ToX, ToY, ToZ; // 이동했을때 플레이어 스폰되는 위치
        public float BoxColliderSizeX, BoxColliderSizeY;

        public WarpData(int mapUnum, Vector3 position, int toMapUnum, Vector3 toSpawnPosition, Vector3 rotation, Vector2 boxColliderSize)
        {
            MapUnum = mapUnum;
            ToMapUnum = toMapUnum;
            x = position.x;
            y = position.y;
            z = position.z;
            ToX = toSpawnPosition.x;
            ToY = toSpawnPosition.y;
            ToZ = toSpawnPosition.z;
            RotationX = rotation.x;
            RotationY = rotation.y;
            RotationZ = rotation.z;
            BoxColliderSizeX = boxColliderSize.x;
            BoxColliderSizeY = boxColliderSize.y;
        }
    }
}