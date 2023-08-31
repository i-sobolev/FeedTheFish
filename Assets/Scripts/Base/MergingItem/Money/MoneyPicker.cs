using UnityEngine;

namespace FeedTheFish
{
    public class MoneyPicker : MonoBehaviour
    {
        [SerializeField] private Camera _camera;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
                TryPickMoney();
        }

        private void TryPickMoney()
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);

            Physics.Raycast(ray, out var hitInfo);

            hitInfo.collider.TryGetComponent<Money>(out var money);

            if (money != null)
            {
                PlayerMoney.Instance.Amount += money.Amount;
                money.OnPick();
            }
        }
    }
}