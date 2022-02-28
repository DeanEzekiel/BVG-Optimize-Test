using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Ren.Misc
{

    /// <summary>
    /// A script that notifies its subscribers about the min and max range 
    /// (expressed in floating y position) of items viewable on the Content
    /// of a given ScrollRect. Updates are done:
    /// [1] after the first few frames (for initialization), and
    /// [2] after every scroll action performed on the ScrollRect.
    /// 
    /// -Renelie Salazar
    /// </summary>
    [RequireComponent(typeof(ScrollRect))]
    public class ScrollRectVerticalContentTracker : MonoBehaviour
    {

        #region Inspector Fields

        [SerializeField]
        [Range(0f, 500f)]
        [Tooltip("Additional value for the range of content doled out by this script to its subscribers.")]
        private float buffer = 500f;

        #endregion //Inspector Fields

        #region Private Fields

        private ScrollRect scrollRect;
        private float height;

        private Action<float, float> callbacks;

        #endregion //Private Fields

        #region Unity Callbacks

        private void Awake()
        {
            scrollRect = GetComponent<ScrollRect>();
            scrollRect.onValueChanged.AddListener(OnViewRangeChange);
            height = scrollRect.GetComponent<RectTransform>().rect.height;
        }

        private IEnumerator Start()
        {
            yield return StartCoroutine(C_AutoNotifyAllSubscribers());
        }

        #endregion //Unity Callbacks

        #region Public API

        public void SubscribeToOnViewRangeChange(Action<float, float> callback)
        {
            if (callback == null)
            {
                return;
            }

            callbacks += callback;
        }

        public void UnsubscribeToOnViewRangeChange(Action<float, float> callback)
        {
            if (callback == null)
            {
                return;
            }

            callbacks -= callback;
        }

        #endregion //Public API

        #region Class Implementation

        private IEnumerator C_AutoNotifyAllSubscribers()
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();

            OnViewRangeChange(Vector2.zero);
        }

        private void OnViewRangeChange(Vector2 position)
        {
            var beginning = scrollRect.content.localPosition.y;
            var minRange = beginning - buffer;
            var maxRange = beginning + height + buffer;

            callbacks?.Invoke(minRange, maxRange);
        }

        #endregion //Class Implementation

    }

}