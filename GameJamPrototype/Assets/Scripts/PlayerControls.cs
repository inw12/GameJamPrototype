using UnityEngine;

public class PlayerControls : MonoBehaviour
{
        private PlayerInputs inputs;
        public static PlayerControls Instance;
        public Vector2 MovePressed => inputs.General.Move.ReadValue<Vector2>();
        public bool JumpPressed => inputs.General.Jump.IsPressed();
        public Vector2 Mouse => inputs.General.Mouse.ReadValue<Vector2>();
        public bool Mouse1 => inputs.General.Mouse1.IsPressed();
        public bool InteractPressed => inputs.General.Interact.IsPressed();
        public bool SprintPressed => inputs.General.Sprint.IsPressed();
        public bool StatusMenu => inputs.General.StatusMenu.IsPressed();

        void Awake()
        {
                Instance = this;
                inputs = new PlayerInputs();
                inputs.Enable();
        }

        // returns mouse position as world position
        public static Vector2 GetMouseWorldPosition()
        {
                Vector3 screenPos = Instance.Mouse;
                screenPos.z = -Camera.main.transform.position.z;
                return Camera.main.ScreenToWorldPoint(screenPos);
        }
}