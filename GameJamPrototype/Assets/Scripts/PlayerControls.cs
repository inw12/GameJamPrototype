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

        void Awake()
        {
                Instance = this;
                inputs = new PlayerInputs();
                inputs.Enable();
        }
}