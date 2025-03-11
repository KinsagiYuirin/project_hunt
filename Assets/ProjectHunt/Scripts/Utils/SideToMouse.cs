using UnityEngine;

namespace MadDuck.Scripts.Utils
{
    /// <summary>
    /// Flip the object to direction the mouse cursor.
    /// </summary>
    public class SideToMouse : MonoBehaviour
    {
        void Update()
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.transform.position.y));
            
            if (worldMousePos.x > transform.position.x)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (worldMousePos.x < transform.position.x)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }
}
