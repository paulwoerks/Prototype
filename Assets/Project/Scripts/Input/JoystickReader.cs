using UnityEngine;

namespace PocketHeroes.Input
{
    public enum Joysticks { PlayerMovement }
    [System.Serializable]
    public struct JoystickReader
    {
        [SerializeField] Joysticks joystick;
        UltimateJoystick ultimateJoystick;

        public void Initialize()
        {
            ultimateJoystick = UltimateJoystick.GetUltimateJoystick(joystick.ToString());
        }

        /// <summary>
        /// Get the current direction the joystick points at.
        /// </summary>
        /// <param name="rotationOffset"></param>
        /// <returns>Vector3 Direction</returns>
        public Vector3 GetDirection(float rotationOffset = 0f)
        {
            Vector3 direction = Rotate(new Vector3(ultimateJoystick.HorizontalAxis, 0, ultimateJoystick.VerticalAxis), rotationOffset);
            return direction;
        }

        /// <summary>
        /// Get the current distance the joystick is dragged.
        /// </summary>
        /// <returns></returns>
        public float GetDistance() => ultimateJoystick.GetDistance();

        Vector3 Rotate(Vector3 vector, float degrees) =>
            Quaternion.Euler(0, degrees, 0) * vector;

    }
}
