using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FeedTheFish
{
    public class MergingItemsSpawner : Singleton<MergingItemsSpawner>
    {
        [SerializeField] private List<MergingItem> _itemsTemplates;
        [Space]
        [SerializeField] private Transform _spawnItemPosition;
        [SerializeField] private float _offset;

        private void Awake()
        {
            base.Awake();

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

        public MergingItem GetNextMergingItemType(MergingItem mergingItem)
        {
            if (_itemsTemplates.Count < mergingItem.Type + 1)
                return null;

            return _itemsTemplates[mergingItem.Type + 1];
        }

        public void SpawnBaseItem()
        {
            var offset = UnityEngine.Random.insideUnitCircle * _offset;
            var spawnPosition = _spawnItemPosition.transform.position + new Vector3() { x = offset.x, z = offset.y };

            var newItem = Instantiate(_itemsTemplates[0], spawnPosition, Quaternion.Euler(UnityEngine.Random.insideUnitSphere));

            newItem.transform.DOScale(1, 0.25f).From(0).SetEase(Ease.OutBack);
        }
    }
}
