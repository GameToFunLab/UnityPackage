using UnityEngine;
using System.Collections;
using GameToFunLab.Scenes;
using UnityEditor;

namespace GameToFunLab.Core
{
    public class CameraManager : MonoBehaviour
    {
        private Transform playerTransform;
        [SerializeField] Vector3 cameraPosition;
        [SerializeField] Vector2 center;
        [SerializeField] Vector2 mapSize;
        [SerializeField] public Vector2 monsterSpawnPositionBoxSize;
        
        [SerializeField] float cameraMoveSpeed;
        [SerializeField] float width;
        [SerializeField] float height;

        // 흔들림 효과 관련 변수
        private Vector3 originalPos;
        private bool isShaking;

        private void Start()
        {
            height = SceneGame.Instance.mainCamera.orthographicSize;
            width = height * Screen.width / Screen.height;
            originalPos = transform.localPosition;
            isShaking = false;
        }

        public void SetPlayer(Transform player)
        {
            playerTransform = player;
        }

        private void Update()
        {
            if (playerTransform == null) return;
            LimitCameraArea();
        }
        private void LimitCameraArea()
        {
            // 플레이어를 따라가는 카메라 위치 계산
            Vector3 targetPos = playerTransform.position + cameraPosition;
            targetPos = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * cameraMoveSpeed);
            
            // 맵의 좌측 상단을 기준으로 경계 내로 제한
            float clampX = Mathf.Clamp(targetPos.x, width, mapSize.x - width); // 좌측 상단 기준 X 좌표 제한
            float clampY = Mathf.Clamp(targetPos.y, height, mapSize.y - height); // 좌측 상단 기준 Y 좌표 제한

            // 맵의 가운데 기준으로 경계 내로 제한
            // float lx = mapSize.x - width;
            // float clampX = Mathf.Clamp(targetPos.x, -lx + center.x, lx + center.x);
            //
            // float ly = mapSize.y - height;
            // float clampY = Mathf.Clamp(targetPos.y, -ly + center.y, ly + center.y);

            // 흔들림이 적용된 최종 위치 설정
            if (isShaking)
            {
                transform.position = new Vector3(clampX, clampY, -10f) + (Vector3)Random.insideUnitCircle * 0.1f;
            }
            else
            {
                transform.position = new Vector3(clampX, clampY, -10f);
                originalPos = transform.position;
            }
        }

        private IEnumerator Shake(float duration, float magnitude)
        {
            isShaking = true;
            float elapsed = 0.0f;

            while (elapsed < duration)
            {
                float x = Random.Range(-1f, 1f) * magnitude;
                float y = Random.Range(-1f, 1f) * magnitude;

                transform.localPosition = originalPos + new Vector3(x, y, 0);

                elapsed += Time.deltaTime;
                yield return null;
            }

            isShaking = false;
            transform.localPosition = originalPos;
        }
        /// <summary>
        /// 카메라 흔들림 효과 주기
        /// </summary>
        /// <param name="shakeDuration"></param>
        /// <param name="shakeMagnitude"></param>
        public void StartShake(float shakeDuration, float shakeMagnitude)
        {
            if (shakeDuration <= 0 || shakeMagnitude <= 0) return;
            StartCoroutine(Shake(shakeDuration, shakeMagnitude));
        }
        /// <summary>
        /// 맵 경계선 사이즈 변경하기
        /// </summary>
        /// <param name="pWidth"></param>
        /// <param name="pHeight"></param>
        public void ChangeMapSize(float pWidth, float pHeight)
        {
            mapSize.x = pWidth;
            mapSize.y = pHeight;
        }
        /// <summary>
        /// 카메라 강제로 이동시키기
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void MoveCameraPosition(float x, float y)
        {
            transform.position = new Vector3(x, y, -10f) + cameraPosition;
            originalPos = transform.position;
        }
        /// <summary>
        /// 플레이어 기준에서의 카메라 위치 값 바꾸기 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void ChangeCameraPositionValue(float x, float y)
        {
            cameraPosition.x = x;
            cameraPosition.y = y;
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(center, mapSize * 2);

            // Gizmos.color = Color.blue;
            // Gizmos.DrawWireCube(center, monsterSpawnPositionBoxSize * 2);
        }
    }
}
