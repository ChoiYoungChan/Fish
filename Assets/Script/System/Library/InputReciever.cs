using System.Collections.Generic;
using Fish;
using View;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace PoiKingyo
{
    public class InputReciever : MonoBehaviour
    {
        [SerializeField] private Camera gameCamera;
        [Inject] private IPoiPresenter _poiPresenter;

        private static List<RaycastResult> raycastResults = new();

        void Update()
        {
            if (IsPointerOverUIObject(Input.mousePosition)) return;
                        
            if (Input.GetMouseButtonDown(0)) //タップ開始
            {
                var point = GetTapAreaPoint();
                if (point != Vector3.zero)
                {
                    _poiPresenter.Move(PoiType.Player, point);
                }
            } 
            else if (Input.GetMouseButtonUp(0)) //タップ終了
            {
                _poiPresenter.Up(PoiType.Player);
            } 
            else if (Input.GetMouseButton(0)) //タップ中
            {
                var point = GetTapAreaPoint();
                if (point != Vector3.zero)
                {
                    _poiPresenter.Move(PoiType.Player, point);
                }
            }
        }


        /// <summary>
        /// タッチした座標を取得
        /// </summary>
        /// <returns></returns>
        private Vector3 GetTapAreaPoint()
        {
            Ray ray = gameCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            float distance = 60f;
            int layermask = 1 << LayerConst.TapArea;

            if (Physics.Raycast(ray, out hit, distance, layermask))
            {
                return hit.point;
            }

            return Vector3.zero;
        }

        public static bool IsPointerOverUIObject(Vector2 screenPosition) {
            // Referencing this code for GraphicRaycaster https://gist.github.com/stramit/ead7ca1f432f3c0f181f
            // the ray cast appears to require only eventData.position.
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = screenPosition;

            EventSystem.current.RaycastAll(eventDataCurrentPosition, raycastResults);
            var over = raycastResults.Count > 0;
            raycastResults.Clear();
            return over;
        }

    }

}