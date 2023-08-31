using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace FeedTheFish
{
    [RequireComponent(typeof(MovableItem))]
    public class MergingItem : MonoBehaviour
    {
        public static event Action<MergingItem> SomeItemPicked;
        public static event Action<MergingItem> SomeItemDropped;

        private MovableItem _movableItem;

        private Tween _highlightTween;
        private bool _lastHighlightedState = false;

        public int Type { get; set; }
        public bool Merging { get; set; } = false;

        private void Awake()
        {
            _movableItem = GetComponent<MovableItem>();

            _movableItem.Picked += OnPicked;
            _movableItem.Dropped += OnDropped;
        }

        private void OnPicked()
        {
            SomeItemPicked?.Invoke(this);
        }

        private void OnDropped()
        {
            SomeItemDropped?.Invoke(this);
        }

        public void SetHighlighted(bool state)
        {
            if (_lastHighlightedState == state)
                return;

            _lastHighlightedState = state;

            if (state)
            {
                _movableItem.SetPhysics(true);

                _highlightTween = transform.DOShakeRotation(0.15f, 10, 20, 90, false).SetLoops(-1, LoopType.Incremental);
            }
            else
            {
                _highlightTween?.Kill();

                _movableItem.SetPhysics(false);
            }
        }

        public Tween PlayMergeAnimationAndDestroy(Vector3 targetPosition)
        {
            Merging = true;
            _movableItem.SetPhysics(false);

            Destroy(GetComponent<Collider>());

            return DOTween.Sequence()
                .Append(transform.DOMove(targetPosition, 0.3f).SetEase(Ease.InOutBack))
                .Join(transform.DOScale(0, 0.3f).SetEase(Ease.InOutBack))
                .OnComplete(() => Destroy(gameObject));
        }
    }
}