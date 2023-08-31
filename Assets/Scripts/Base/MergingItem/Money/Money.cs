using DG.Tweening;
using UnityEngine;

namespace FeedTheFish
{
    public class Money : MonoBehaviour
    {
        [SerializeField] private Collider _collider;
        [SerializeField] private Rigidbody _rigidbody;

        public Rigidbody Rigidbody => _rigidbody;

        public int Amount { get; set; } = 10;

        public void OnPick()
        {
            _collider.enabled = false;
            _rigidbody.isKinematic = true;

            transform.DOScale(0, 0.15f).SetEase(Ease.InBack);
        }
    }
}
