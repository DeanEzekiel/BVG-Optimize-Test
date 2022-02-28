using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Ren.Misc
{

    /// <summary>
    /// A script that notifies its subscribers about the min and max range 
    /// (expressed in floating y position) of items viewable on the Content
    /// of a given ScrollRect.
    /// 
    /// Updates are done after every scroll action performed on the ScrollRect.
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
        private float scrollRectHeight;

        private Action<float, float> subscribers;

        #endregion //Private Fields

        #region Unity Callbacks

        private void Awake()
        {
            Init();
        }

        #endregion //Unity Callbacks

        #region Public API

        public void TriggerRangeUpdate()
        {
            StartCoroutine(C_TriggerRangeUpdateAfterFrame());
        }

        public void SubscribeToOnViewRangeChange(Action<float, float> callback)
        {
            if (callback == null)
            {
                return;
            }

            subscribers += callback;
        }

        public void UnsubscribeToOnViewRangeChange(Action<float, float> callback)
        {
            if (callback == null)
            {
                return;
            }

            subscribers -= callback;
        }

        #endregion //Public API

        #region Class Implementation

        private void Init()
        {
            scrollRect = GetComponent<ScrollRect>();
            scrollRect.onValueChanged.AddListener(OnViewRangeChange);
            scrollRectHeight = scrollRect.GetComponent<RectTransform>().rect.height;
        }

        private IEnumerator C_TriggerRangeUpdateAfterFrame()
        {
            yield return new WaitForEndOfFrame();
            OnViewRangeChange(Vector2.zero);
        }

        private void OnViewRangeChange(Vector2 position)
        {
            var beginning = scrollRect.content.localPosition.y;
            var minRange = beginning - buffer;
            var maxRange = beginning + scrollRectHeight + buffer;
            
            subscribers?.Invoke(minRange, maxRange);
        }

        #endregion //Class Implementation

    }

}