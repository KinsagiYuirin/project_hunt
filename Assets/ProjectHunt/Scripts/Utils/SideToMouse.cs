using MadDuck.Scripts.Character.Module;
using TriInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace MadDuck.Scripts.Utils
{
    /// <summary>
    /// Flip the object to direction the mouse cursor.
    /// </summary>
    public class SideToMouse : MonoBehaviour
    {
        [Title("Modules")]
        [SerializeField] private CharacterMovementModule movementModule;
        [SerializeField] private CharacterCheckCreatureModule checkCreatureModule;
        [SerializeField] private bool fullKeyboardControl;
        [Title("Debug")]
        [SerializeField, DisplayAsString] private bool isUsingController;
        
        private void DetectController()
        {
            bool hasController = Gamepad.current != null;

            if (hasController && !isUsingController)
            {
                isUsingController = true;
                Debug.Log("🎮 Controller detected! Switching to Controller input.");
            }
            else if (!hasController && isUsingController)
            {
                isUsingController = false;
                Debug.Log("⌨️ No controller detected. Switching to Keyboard input.");
            }
        }
        
        void Update()
        {
            DetectController();

            if (fullKeyboardControl)
            {
                FullControlWithKeyboard();
            }
            else
            {
                ControlWithMouseOrJoy();
            }
        }
        
        private void FullControlWithKeyboard()
        {
            switch (checkCreatureModule.NearestEnemy)
            {
                case null:
                    if (movementModule.lastMoveDirection.x > 0f)
                    {
                        transform.rotation = Quaternion.Euler(0, 0, 0);
                    }
                    else if (movementModule.lastMoveDirection.x < 0f)
                    {
                        transform.rotation = Quaternion.Euler(0, 180, 0);
                    }
                    break;
                
                case not null:
                    if (checkCreatureModule.NearestEnemy.position.x > transform.position.x)
                    {
                        transform.rotation = Quaternion.Euler(0, 0, 0);
                    }
                    else if (checkCreatureModule.NearestEnemy.position.x < transform.position.x)
                    {
                        transform.rotation = Quaternion.Euler(0, 180, 0);
                    }
                    break;
            }
        }

        private void ControlWithMouseOrJoy()
        {
            if (isUsingController)
            {
                
                float joystickX = Gamepad.current.leftStick.ReadValue().x; // อ่านค่าจากจอย

                if (joystickX > 0.1f)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0); // หันไปขวา
                }
                else if (joystickX < -0.1f)
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0); // หันไปซ้าย
                }
            }
            else
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
}
