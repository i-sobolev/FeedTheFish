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
        [SerializeField] private List<MergingItem> _itemsTemplates;

        private MergingItem PickedItem { get; set; }
        private MergingItem PotentialMergingItem { get; set; }

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

            if (PotentialMergingItem != null)
            {
                MergeItems();
            }

            PickedItem = null;
            PotentialMergingItem = null;
        }

        private void OnSomeItemPicked(MergingItem item)
        {
            PickedItem = item;

            StartCoroutine(FindMergingItem());
        }

        private void MergeItems()
        {
            var newItemTargetPosition = (PotentialMergingItem.transform.position + PickedItem.transform.position) / 2;

            PickedItem.PlayMergeAnimationAndDestroy(newItemTargetPosition);
            PotentialMergingItem.PlayMergeAnimationAndDestroy(newItemTargetPosition);

            var newItem = Instantiate(GetNextMergingItemType(PickedItem), newItemTargetPosition, Quaternion.identity);
            newItem.Type = PickedItem.Type + 1;

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
                var closestItem = FindClosestItem();

                if (closestItem != PotentialMergingItem)
                {
                    PotentialMergingItem?.SetHighlighted(false);

                    PotentialMergingItem = closestItem;

                    PotentialMergingItem?.SetHighlighted(true);
                }

                yield return new WaitForSeconds(0.1f);
            }

            MergingItem FindClosestItem()
            {
                var minDistance = Mathf.Infinity;

                MergingItem result = null;

                foreach (var item in allItems)
                {
                    if (item == PickedItem)
                        continue;

                    if (item.Type != PickedItem.Type)
                        continue;

                    if (item.Merging)
                        continue;

                    var distance = Vector3.Distance(item.transform.position, PickedItem.transform.position);

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
            if (PotentialMergingItem != null && PickedItem != null)
            {
                Debug.DrawLine(PickedItem.transform.position, PotentialMergingItem.transform.position, Color.yellow);
            }
        }
    }
}
