using UnityEngine;

namespace Scripts.Maps
{
    public class NPCData
    {
        public int Vnum;
        public int MapVnum;
        public float x, y, z;
        public bool IsFlip;
        public float ScaleX, ScaleY, ScaleZ;

        public NPCData(int vnum, Vector3 position, bool isFlip, Vector3 scale, int mapVnum)
        {
            Vnum = vnum;
            MapVnum = mapVnum;
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