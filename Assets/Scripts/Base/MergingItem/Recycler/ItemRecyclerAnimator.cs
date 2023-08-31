using DG.Tweening;
using UnityEngine;

namespace FeedTheFish
{
    public abstract class ItemRecyclerAnimator : MonoBehaviour
    {
        public abstract void SetOpened(bool state);
        public abstract Tween PlayRecycleAnimation();
    }
}
