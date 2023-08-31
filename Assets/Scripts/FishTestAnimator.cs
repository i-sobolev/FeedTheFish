using DG.Tweening;
using UnityEngine;

namespace FeedTheFish
{
    public class FishTestAnimator : ItemRecyclerAnimator
    {
        [SerializeField] private Transform _mouth;
        [Space]
        [SerializeField] private Vector3 _openedMouthTransform;
        [SerializeField] private Vector3 _closedMouthTransform;

        public override Tween PlayRecycleAnimation()
        {
            return DOTween.Sequence()
                .Append(_mouth.transform.DOLocalRotate(_closedMouthTransform, 0.15f))
                .Append(_mouth.transform.DOLocalRotate(_openedMouthTransform, 0.15f))
                .SetLoops(2, LoopType.Yoyo)
                .OnComplete(() =>
                {
                    SetOpened(false);
                });
        }

        public override void SetOpened(bool state)
        {
            _mouth.transform.DOLocalRotate(state ? _openedMouthTransform : _closedMouthTransform, 0.15f);
        }
    }
}