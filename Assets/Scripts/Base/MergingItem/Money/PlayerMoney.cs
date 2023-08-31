using System;
using TMPro;
using UnityEngine;

namespace FeedTheFish
{
    public class PlayerMoney : Singleton<PlayerMoney>
    {
        public event Action AmountChanged;

        [SerializeField] private TextMeshProUGUI _text;

        private int _amount;

        public int Amount
        {
            get
            {
                return _amount;
            }

            set
            {
                _text.text = value.ToString();

                _amount = value;

                AmountChanged?.Invoke();
            }
        }

        private void Start()
        {
            Amount = 0;
        }
    }
}
