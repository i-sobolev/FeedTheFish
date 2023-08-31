using DG.Tweening;
using UnityEngine;

namespace FeedTheFish
{
    public class ItemRecycler : MonoBehaviour
    {
        [SerializeField] private ItemRecyclerAnimator _animator;
        [SerializeField] private Money _moneyTemplate;
        [Space]
        [SerializeField] private Transform _itemsTargetPosition;

        public void SetReady(bool state)
        {
            _animator.SetOpened(state);
        }

        public void Recycle(MergingItem item)
        {
            item.PlayMergeAnimationAndDestroy(_itemsTargetPosition.position);

            DOTween.Sequence()
                .Append(_animator.PlayRecycleAnimation())
                .AppendCallback(() =>
                {
                    _animator.SetOpened(true);

                    SpawnMoney();
                })
                .AppendInterval(0.5f)
                .AppendCallback(() =>
                {
                    _animator.SetOpened(false);
                });
        }

        private void SpawnMoney()
        {
            var money = Instantiate(_moneyTemplate, _itemsTargetPosition.transform.position, Quaternion.identity);

            money.Rigidbody.AddForce(_itemsTargetPosition.forward * 5, ForceMode.Impulse);
            money.Rigidbody.AddTorque(Random.insideUnitSphere * 10, ForceMode.Impulse);
        }
    }
}