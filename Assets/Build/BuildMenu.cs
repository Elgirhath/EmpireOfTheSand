using UnityEngine;
using UnityEngine.UI;

namespace Build
{
    public class BuildMenu : MonoBehaviour
    {
        private Button[] buttons;
        private bool isOpen = false;

        private void Start()
        {
            buttons = GetComponentsInChildren<Button>();
            Close();
        }

        private void Update()
        {
            if (Input.GetButtonDown("Toggle Build Menu"))
            {
                if (isOpen)
                {
                    Close();
                }
                else
                {
                    Open();
                }
            }
        }

        public void Open()
        {
            foreach (var button in buttons)
            {
                button.gameObject.SetActive(true);
            }
            isOpen = true;
        }

        public void Close()
        {
            foreach (var button in buttons)
            {
                button.gameObject.SetActive(false);
            }
            isOpen = false;
        }
    }
}
