using UnityEngine;

namespace GraviZoo
{
    public class ClickEscToExit : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }
}
