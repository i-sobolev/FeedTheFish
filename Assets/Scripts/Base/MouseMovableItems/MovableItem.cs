using DG.Tweening;
using System;
using UnityEngine;

namespace FeedTheFish
{
    public class MovableItem : MonoBehaviour
    {
        public event Action Picked;
        public event Action Dropped;

        [SerializeField] private Collider _collider;
        [SerializeField] private Rigidbody _rigidbody;

        public void SetPhysics(bool state)
        {
            _collider.enabled = !state;
            _rigidbody.isKinematic = state;
        }

        public void SetPicked(bool state)
        {
            SetPhysics(state);

            if (state)
                Picked?.Invoke();
            else
                Dropped?.Invoke();
        }
    }
}