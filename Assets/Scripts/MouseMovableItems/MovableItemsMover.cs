using UnityEngine;

namespace FeedTheFish
{
    public class MovableItemsMover : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [Space]
        [SerializeField] private Vector3 _movingOffset;

        private MovableItem _pickedItem = null;

        #region DEBUG
        //[TabGroup("Tabs", "Debug")]
        //[ShowInInspector, ReadOnly] private bool _itemRaycasted;
        #endregion

        private void Update()
        {
            if (!_pickedItem && Input.GetMouseButtonDown(0))
            {
                if (TryPickItem(out var item))
                {
                    _pickedItem = item;

                    _pickedItem.SetPicked(true);

                }
            }

            if (_pickedItem && Input.GetMouseButton(0))
            {
                var targetPosition = GetWorldProjectedMousePosition() + _movingOffset;

                _pickedItem.transform.position = Vector3.Lerp(_pickedItem.transform.position, targetPosition, 0.1f);
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (_pickedItem == null)
                    return;

                _pickedItem.SetPicked(false);

                _pickedItem = null;
            }
        }

        private bool TryPickItem(out MovableItem item)
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