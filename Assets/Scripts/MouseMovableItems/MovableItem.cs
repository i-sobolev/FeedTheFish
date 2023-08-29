using UnityEngine;

namespace FeedTheFish
{
    public class MovableItem : MonoBehaviour
    {
        [SerializeField] private Collider _collider;
        [SerializeField] private Rigidbody _rigidbody;

        public void SetEnabledMoving(bool state)
        {
            _collider.enabled = !state;
            _rigidbody.isKinematic = state;
        }
    }
}