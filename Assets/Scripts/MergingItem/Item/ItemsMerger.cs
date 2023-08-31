using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FeedTheFish
{
    public class ItemsMerger : MonoBehaviour
    {
        [SerializeField] private float _minMergeDistance;
        [Space]
        [SerializeField] private ItemRecycler _itemRecycler;
        [SerializeField] private List<MergingItem> _itemsTemplates;

        private MergingItem _pickedItem;
        private MergingItem _potentialMergingItem;

        private bool CanRecycle => Vector3.Distance(_pickedItem.transform.position, _itemRecycler.transform.position) < _minMergeDistance * 2;

        private void Awake()
        {
            MergingItem.SomeItemPicked += OnSomeItemPicked;
            MergingItem.SomeItemDropped += OnSomeItemDropped;

            InitializeItems();
        }

        private void InitializeItems()
        {
            int counter = 0;

            foreach (var item in _itemsTemplates)
            {
                item.Type = counter++;
            }
        }

        private void OnSomeItemDropped(MergingItem item)
        {
            StopAllCoroutines();

            if (_potentialMergingItem != null)
            {
                MergeItems();
            }

            if (CanRecycle)
            {
                _itemRecycler.Recycle(item);
            }

            _pickedItem = null;
            _potentialMergingItem = null;
        }

        private void OnSomeItemPicked(MergingItem item)
        {
            _pickedItem = item;

            StartCoroutine(FindMergingItem());
        }

        private void MergeItems()
        {
            var newItemTargetPosition = (_potentialMergingItem.transform.position + _pickedItem.transform.position) / 2;

            _pickedItem.PlayMergeAnimationAndDestroy(newItemTargetPosition);
            _potentialMergingItem.PlayMergeAnimationAndDestroy(newItemTargetPosition);

            var newItem = Instantiate(GetNextMergingItemType(_pickedItem), newItemTargetPosition, Quaternion.identity);
            newItem.Type = _pickedItem.Type + 1;

            DOTween.Sequence()
                .Append(newItem.transform.DOScale(1, 0.25f).From(0).SetEase(Ease.OutBack))
                .Join(newItem.transform.DOJump(newItem.transform.position, 0.5f, 1, 0.3f))
                .SetDelay(0.15f);
        }

        private MergingItem GetNextMergingItemType(MergingItem mergingItem)
        {
            if (_itemsTemplates.Count < mergingItem.Type + 1)
                return null;

            return _itemsTemplates[mergingItem.Type + 1];
        }

        private IEnumerator FindMergingItem()
        {
            var allItems = FindObjectsOfType<MergingItem>();

            while (true)
            {
                var canRecycle = CanRecycle;

                _itemRecycler.SetReady(canRecycle);

                if (CanRecycle)
                {
                    _potentialMergingItem?.SetHighlighted(false);
                    _potentialMergingItem = null;
                }
                else
                {
                    var closestItem = FindClosestItem();

                    if (closestItem != _potentialMergingItem)
                    {
                        _potentialMergingItem?.SetHighlighted(false);

                        _potentialMergingItem = closestItem;

                        _potentialMergingItem?.SetHighlighted(true);
                    }
                }

                yield return new WaitForSeconds(0.1f);
            }

            MergingItem FindClosestItem()
            {
                var minDistance = Mathf.Infinity;

                MergingItem result = null;

                foreach (var item in allItems)
                {
                    if (item == _pickedItem)
                        continue;

                    if (item.Type != _pickedItem.Type)
                        continue;

                    if (item.Merging)
                        continue;

                    var distance = Vector3.Distance(item.transform.position, _pickedItem.transform.position);

                    if (distance < _minMergeDistance && distance < minDistance)
                    {
                        minDistance = distance;
                        result = item;
                    }
                }

                return result;
            }
        }

        private void OnDrawGizmos()
        {
            if (_potentialMergingItem != null && _pickedItem != null)
            {
                Debug.DrawLine(_pickedItem.transform.position, _potentialMergingItem.transform.position, Color.yellow);
            }
        }
    }
}
