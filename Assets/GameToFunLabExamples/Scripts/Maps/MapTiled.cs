using System.Collections.Generic;
using GameToFunLab.Core;
using GameToFunLab.Maps;
using GameToFunLab.Scenes;
using Scripts.TableLoader;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Scripts.Maps
{
    public class MapTiled : DefaultMap
    {
        public Camera mainCamera;
        public float extraTileCount = 2f;
        private TilemapRenderer tilemapRenderer;
        private Tilemap tilemap;

        public List<GameObject> monsters = new List<GameObject>();
        public List<GameObject> npcs = new List<GameObject>();

        private Bounds cullingBounds; // 현재 컬링 범위를 저장할 변수
        private float mainCameraZ;

        protected void Awake()
        {
            tilemapRenderer = GetComponent<TilemapRenderer>();
            tilemap = GetComponent<Tilemap>();
        }
        public void InitializeByEditor(int unum, string name, MapConstants.Type type, MapConstants.SubType subType)
        {
            Unum = unum;
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            var size = GetMapSize();
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, size.height, 0);
        }

        void Start()
        {
            if (mainCamera == null)
            {
                mainCamera = SceneGame.Instance.mainCamera;
            }
            mainCameraZ = mainCamera.transform.position.z;

            if (Unum <= 0) return;
            
            StruckTableMap struckTableMap = TableLoaderManager.Instance.TableMap.GetMapData(Unum);
            // bgm 플레이
            if (struckTableMap.BgmUnum > 0)
            {
                SceneGame.Instance.soundManager?.PlayByUnum(struckTableMap.BgmUnum);
            }

            var result = GetMapSize();

            // 로드된 맵에 맞게 맵 영역 사이즈 갱신하기 
            mainCamera.GetComponent<CameraManager>()?.ChangeMapSize(result.width, result.height);
            
            UpdatePosition();
            CalculateCullingBounds();
        }

        protected override (float width, float height) GetMapSize()
        {
            if (tilemap == null) 
            {
                tilemap = GetComponent<Tilemap>();
            }

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

        void LateUpdate()
        {
            // 카메라가 이동할 때마다 컬링 범위 및 오브젝트 상태 갱신
            CalculateCullingBounds();
        }

        void CalculateCullingBounds()
        {
            if (mainCamera == null || tilemapRenderer == null) return;

            // 카메라 크기 계산
            float verticalSize = mainCamera.orthographicSize;
            float horizontalSize = verticalSize * mainCamera.aspect;

            // Culling Bounds 설정
            tilemapRenderer.chunkCullingBounds = new Vector3(
                horizontalSize + extraTileCount,
                verticalSize + extraTileCount,
                0
            );

            // 카메라의 현재 위치를 기준으로 컬링 영역을 갱신
            Vector3 cameraPosition = mainCamera.transform.position;
            cullingBounds = new Bounds(cameraPosition, new Vector3(
                (horizontalSize + extraTileCount) * 2,
                (verticalSize + extraTileCount) * 2,
                0
            ));

            // 오브젝트 활성화/비활성화 처리
            UpdateObjectActivation(monsters, cullingBounds);
            UpdateObjectActivation(npcs, cullingBounds);
        }

        void UpdateObjectActivation(List<GameObject> objects, Bounds bounds)
        {
            foreach (GameObject obj in objects)
            {
                if (obj == null) continue;

                // NPC의 Z 축도 고려하여 활성화 상태 확인
                Vector3 position = obj.transform.position;
                bool isActive = bounds.Contains(new Vector3(position.x, position.y, mainCameraZ));

                // 활성화 상태 설정
                if (obj.activeSelf != isActive)
                {
                    // if (isActive)
                    // {
                    //     obj.GetComponent<MyNpc>().StartFadeIn();
                    // }
                    // else
                    // {
                    //     obj.GetComponent<MyNpc>().StartFadeOut();
                    // }
                    obj.SetActive(isActive);
                }
            }
        }

        public void AddNpc(GameObject npc)
        {
            if (npc == null) return;
            npcs.Add(npc);
        }

        void OnDrawGizmos()
        {
            if (mainCamera == null) return;

            // 카메라의 가로, 세로 뷰 크기 계산
            float verticalSize = mainCamera.orthographicSize;
            float horizontalSize = verticalSize * mainCamera.aspect;

            // 카메라 뷰의 영역 시각화 (초록색)
            Vector3 cameraPosition = mainCamera.transform.position;
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(
                cameraPosition,
                new Vector3(horizontalSize * 2 + extraTileCount * 2, verticalSize * 2 + extraTileCount * 2, 0)
            );

            // 컬링 영역 시각화 (빨간색)
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(cullingBounds.center, cullingBounds.size);
        }
    }
}
