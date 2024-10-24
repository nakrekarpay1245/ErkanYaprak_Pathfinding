using UnityEngine;

namespace SpaceObjects
{
    /// <summary>
    /// Controls the rotation of an asteroid along the X, Y, and Z axes.
    /// Each axis can be toggled on or off independently, and the speed of rotation can be adjusted for each axis.
    /// </summary>
    public class Asteroid : MonoBehaviour
    {
        [Header("Rotation Toggles")]
        [Tooltip("Enable or disable rotation around the X-axis.")]
        [SerializeField] private bool _rotateX = true;

        [Tooltip("Enable or disable rotation around the Y-axis.")]
        [SerializeField] private bool _rotateY = true;

        [Tooltip("Enable or disable rotation around the Z-axis.")]
        [SerializeField] private bool _rotateZ = true;

        [Header("Rotation Speeds")]
        [Tooltip("Rotation speed around the X-axis.")]
        [SerializeField, Range(0f, 100f)] private float _speedX = 10f;

        [Tooltip("Rotation speed around the Y-axis.")]
        [SerializeField, Range(0f, 100f)] private float _speedY = 10f;

        [Tooltip("Rotation speed around the Z-axis.")]
        [SerializeField, Range(0f, 100f)] private float _speedZ = 10f;

        /// <summary>
        /// Updates the rotation of the asteroid each frame.
        /// The asteroid rotates around the enabled axes at the specified speeds.
        /// </summary>
        private void Update()
        {
            // Calculate the rotation for each axis based on whether it's enabled and the assigned speed.
            float rotationX = _rotateX ? _speedX * Time.deltaTime : 0f;
            float rotationY = _rotateY ? _speedY * Time.deltaTime : 0f;
            float rotationZ = _rotateZ ? _speedZ * Time.deltaTime : 0f;

            // Apply the rotation to the asteroid.
            transform.Rotate(rotationX, rotationY, rotationZ);
        }
    }
}