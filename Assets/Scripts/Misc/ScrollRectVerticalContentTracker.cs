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
        private ScrollRect scrollRect;

        [SerializeField]
        [Range(0f, 500f)]
        [Tooltip("Additional value for the range of content doled out by this script to its subscribers.")]
        private float buffer = 500f;

        #endregion //Inspector Fields

        #region Private Fields

        private float scrollRectHeight;
        private Action<float, float> subscribers;

        #endregion //Private Fields

        #region Accessors

        public ScrollRect ScrollRect => scrollRect;

        #endregion //Accessors

        #region Unity Callbacks

        private void Awake()
        {
            Init();
        }

        #endregion //Unity Callbacks

        #region Public API

        /// <summary>
        /// Sets the ScrollRect's Content to its final dimensions and 
        /// not accept further changes before triggering a forced view range update.
        /// </summary>
        public void FinalizeScrollContentAndTriggerRangeUpdate()
        {
            StartCoroutine(C_FinalizeScrollContentTriggerRangeUpdateAfterFrame());
        }

        /// <summary>
        /// Register a delegate to be notified every time the scroll content
        /// view range is updated. 
        /// </summary>
        /// <param name="callback">The delegate to be called.
        /// Accepts a float minRange, and a float maxRange value.</param>
        public void SubscribeToOnViewRangeChange(Action<float, float> callback)
        {
            if (callback == null)
            {
                return;
            }

            subscribers += callback;
        }

        /// <summary>
        /// Unregister a previously registered delegate via SubscribeToOnViewRangeChange().
        /// </summary>
        /// <param name="callback">The delegate to be unregistered.</param>
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
            scrollRect.onValueChanged.AddListener(OnViewRangeChange);
            scrollRectHeight = scrollRect.GetComponent<RectTransform>().rect.height;
        }

        private void RemoveScrollContentDynamicitySetup()
        {
            if (scrollRect.content.gameObject.TryGetComponent<ContentSizeFitter>(
                out var fitter))
            {
                Destroy(fitter);
            }
            if (scrollRect.content.gameObject.TryGetComponent<VerticalLayoutGroup>(
                out var vLayoutGroup))
            {
                Destroy(vLayoutGroup);
            }
        }

        private IEnumerator C_FinalizeScrollContentTriggerRangeUpdateAfterFrame()
        {
            yield return new WaitForEndOfFrame();
            RemoveScrollContentDynamicitySetup();
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