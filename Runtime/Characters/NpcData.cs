using UnityEngine;

namespace GameToFunLab.Characters
{
    public class NpcData
    {
        public int Unum;
        public int MapUnum;
        public float x, y, z;
        public bool IsFlip;
        public float ScaleX, ScaleY, ScaleZ;

        public NpcData(int unum, Vector3 position, bool isFlip, Vector3 scale, int mapUnum)
        {
            Unum = unum;
            MapUnum = mapUnum;
            x = position.x;
            y = position.y;
            z = position.z;
            IsFlip = isFlip;
            ScaleX = scale.x;
            ScaleY = scale.y;
            ScaleZ = scale.z;
        }
    }
}