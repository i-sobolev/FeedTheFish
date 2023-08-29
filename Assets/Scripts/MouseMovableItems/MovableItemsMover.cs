using UnityEngine;

namespace FeedTheFish
{
    public class MovableItemsMover : MonoBehaviour
    {
        enum MoverState {  }

        [SerializeField] private Camera _camera;
        [Space]
        [SerializeField] private Vector3 _movingOffset;

        private MovableItem _catchedItem = null;

        #region DEBUG
        //[TabGroup("Tabs", "Debug")]
        //[ShowInInspector, ReadOnly] private bool _itemRaycasted;
        #endregion

        private void Update()
        {
            if (!_catchedItem && Input.GetMouseButtonDown(0))
            {
                if (TryCatchItem(out var item))
                {
                    _catchedItem = item;

                    _catchedItem.SetEnabledMoving(true);
                }
            }

            if (_catchedItem && Input.GetMouseButton(0))
            {
                var targetPosition = GetWorldProjectedMousePosition() + _movingOffset;

                _catchedItem.transform.position = Vector3.Lerp(_catchedItem.transform.position, targetPosition, 0.1f);
            }

            if (Input.GetMouseButtonUp(0)) 
            {
                _catchedItem.SetEnabledMoving(false);

                _catchedItem = null;
            }
        }

        private bool TryCatchItem(out MovableItem item)
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);

            Debug.DrawRay(ray.origin, ray.direction * 100, Color.blue);

            var raycasted = Physics.Raycast(ray, out var hitInfo);

            hitInfo.collider.TryGetComponent(out item);

            return raycasted && item != null;
        }

        private Vector3 GetWorldProjectedMousePosition()
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);

            Debug.DrawRay(ray.origin, ray.direction * 100, Color.green);

            Physics.Raycast(ray, out var hitInfo);

            return hitInfo.point;
        }
    }
}