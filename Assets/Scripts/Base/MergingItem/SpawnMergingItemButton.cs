using System;
using UnityEngine;
using UnityEngine.UI;

namespace FeedTheFish
{
    [RequireComponent(typeof(Button))]
    public class SpawnMergingItemButton : MonoBehaviour
    {
        [SerializeField] private int _price;

        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();

            _button.onClick.AddListener(OnButtonClicked);

            PlayerMoney.Instance.AmountChanged += OnAmountChanged;
        }

        private void OnButtonClicked()
        {
            PlayerMoney.Instance.Amount -= _price;

            MergingItemsSpawner.Instance.SpawnBaseItem();
        }

        private void OnAmountChanged()
        {
            SetAvailable(PlayerMoney.Instance.Amount >=  _price);
        }

        private void SetAvailable(bool state)
        {
            _button.interactable = state;
        }
    }
}