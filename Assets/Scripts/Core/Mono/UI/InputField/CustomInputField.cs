namespace Game
{
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class CustomInputField : MonoBehaviour
    {
        private Image mBG;
        [HideInInspector]
        public InputField inputField;
        void Awake()
        {
            mBG = transform.GetChild(0).GetComponent<Image>();
            mBG.gameObject.SetActive(false);
            inputField = transform.GetChild(1).GetComponent<InputField>();
            (inputField.transform as RectTransform).AddEventTrigger(EventTriggerType.PointerClick, (eventData) =>
            {
                mBG.gameObject.SetActive(true);
            });
            inputField.onEndEdit.AddListener((text) =>
            {
                mBG.gameObject.SetActive(false);
            });
        }
    }

}