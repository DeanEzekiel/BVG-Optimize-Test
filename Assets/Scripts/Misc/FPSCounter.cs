using TMPro;
using UnityEngine;

namespace Ren.Misc
{

    /// <summary>
    /// A class that shows the FPS, given a UI text (TMP) 
    /// and a refresh interval value.
    /// 
    /// It shows the same FPS on the Game View's Stats panel
    /// Destroys itself if:
    /// [A] no TMP text is assigned, and
    /// [B] build is not DEBUG or played in Unity Editor
    /// 
    /// - Renelie Salazar
    /// </summary>
    public class FPSCounter : MonoBehaviour
    {

        #region Inspector Fields

        [SerializeField] 
        private TextMeshProUGUI textCounter;
        
        [SerializeField] 
        [Range(0.1f, 5f)]
        private float refreshInterval = 1f;

        #endregion //Inspector Fields

        #region Private Fields

        private const string PREFIX = "FPS: ";
        private float cachedTime;

        #endregion //Private Fields

        #region Unity Callbacks

        private void Awake()
        {
            if (textCounter == null)
            {
                Destroy(this);
            }
        }

        private void Start()
        {
            #if !DEBUG && !UNITY_EDITOR
                textCounter.text = string.Empty;
                Destroy(this);
            #endif
        }

        private void Update()
        {
            RefreshText();
        }

        #endregion //Unity Callbacks

        #region Class Implementation

        private void RefreshText()
        {
            if (Time.unscaledTime > cachedTime)
            {
                int fps = (int)(1f / Time.unscaledDeltaTime);
                textCounter.text = PREFIX + fps.ToString();
                cachedTime = Time.unscaledTime + refreshInterval;
            }
        }

        #endregion //Class Implementation

    }

}