using GameToFunLab.Configs;
using UnityEngine;

namespace GameToFunLab.Maps.Objects
{
    public class ObjectWarp : MonoBehaviour
    {
        public WarpData warpData;
        public int toMapUnum; // 워프할 unum
        public Vector3 toMapPlayerSpawnPosition; // 워프될 곳에 플레이어가 스폰될 위치
        private BoxCollider2D boxCollider2D;

        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            if (warpData == null) return;
            boxCollider2D = GetComponent<BoxCollider2D>();
            
            toMapUnum = warpData.ToMapUnum;
            toMapPlayerSpawnPosition = new Vector3(warpData.ToX, warpData.ToY, warpData.ToZ);
            transform.position = new Vector3(warpData.x, warpData.y, warpData.z);
            transform.eulerAngles = new Vector3(warpData.RotationX, warpData.RotationY, warpData.RotationZ);
            if (boxCollider2D != null)
            {
                boxCollider2D.size = new Vector2(warpData.BoxColliderSizeX, warpData.BoxColliderSizeY);
            }
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (toMapUnum <= 0) return;
            if (collision.gameObject.CompareTag(ConfigTags.Player) != true) return;
            
            // SceneGame.Instance.mapManager.SetPlaySpawnPosition(toMapPlayerSpawnPosition);
            // SceneGame.Instance.mapManager.LoadMap(toMapUnum);
        }
    }
}