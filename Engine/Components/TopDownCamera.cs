using System;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

namespace RedOwl.Engine
{
    public class TopDownCamera : MonoBehaviour
    {
        [SerializeField] private Transform angleTransform;
        [SerializeField] private Camera targetCamera;
        [SerializeField] private Transform follow;

        [Title("Move")]
        [SerializeField] private float moveSpeed = 1f;
        [SerializeField] private float moveDamping = 12f;
        [SerializeField] private float moveVelocityDamping = 3f;
        [SerializeField] private float moveVelocityDragThreshold = 0.2f;
        
        [Title("Edge Screen Move")]
        [SerializeField] private bool enableScreenEdgeMove = false;
        [Range(0f, 0.1f)]
        [SerializeField] private float screenEdgeTolerance = 0.02f;

        [SerializeField] private float screenEdgeMoveSpeed = 8f;

        [Title("Rotation")] 
        [SerializeField] private float rotationSpeed = 1f;
        [SerializeField] private float rotationDamping = 10f;
        [SerializeField] private float rotationMouseFactor = 5f;
        [SerializeField] private float rotationYMin = -30;
        [SerializeField] private float rotationYMax = 35;
        
        [Title("Zoom")] 
        [SerializeField] private float zoomSpeed = 1f;
        [SerializeField] private float zoomDamping = 7f;
        [SerializeField] private float zoomMouseFactor = 0.3f;
        [SerializeField] private float zoomMin = 50f;
        [SerializeField] private float zoomMax = 450f;

        private InputHandler input;

        private Transform targetCameraTransform;
        private Vector3 targetPosition;
        private Vector3 targetDragVelocity;
        private float targetRotationX;
        private float targetRotationY;
        private float targetZoom;

        private Plane dragPlane;
        
        private Vector3 dragMoveStartPosition;
        private Vector3 dragMoveCurrentPosition;

        private Vector3 dragRotationStartPosition;
        private Vector3 dragRotationCurrentPosition;

        private void OnEnable()
        {
            input = World.Default.Get<PlayerInputSystem>().Player1;
            
            targetCameraTransform = targetCamera.transform;
            targetPosition = transform.position;
            targetDragVelocity = Vector3.zero;
            targetRotationX = transform.rotation.eulerAngles.y;
            targetRotationY = angleTransform.localRotation.eulerAngles.x;
            targetZoom = math.abs(targetCameraTransform.localPosition.z);

            dragPlane = new Plane(Vector3.up, Vector3.zero);
        }

        private void Update()
        {
            if (input.Mouse != null)
            {
                CalculateMouseTargetRotation();
                CalculateMouseTargetZoom();
                // TODO: Move setting follow transform to a function so we can set a private bool to make this more effecient
                if (follow == null) CalculateMouseTargetPosition();
            }
            CalculateTargetRotation();
            CalculateTargetZoom();
            CalculateTargetPosition();

            var dt = Time.deltaTime;
            UpdatePosition(dt);
            UpdateRotation(dt);
            UpdateZoom(dt);
        }

        private Vector3 GetCameraRight()
        {
            var output = targetCameraTransform.right;
            output.y = 0;
            return output;
        }

        private Vector3 GetCameraForward()
        {
            var output = targetCameraTransform.forward;
            output.y = 0;
            return output;
        }

        private void CalculateMouseTargetPosition()
        {
            var mousePosition = input.MousePosition.ReadValue<Vector2>();
            if (input.MiddleMouseButton.WasPressedThisFrame())
            {
                var ray = targetCamera.ScreenPointToRay(mousePosition);

                if (dragPlane.Raycast(ray, out var hitPoint))
                {
                    dragMoveStartPosition = ray.GetPoint(hitPoint);
                }
            }

            if (input.MiddleMouseButton.IsPressed())
            {
                targetPosition = transform.position;
                var ray = targetCamera.ScreenPointToRay(mousePosition);

                if (dragPlane.Raycast(ray, out var hitPoint))
                {
                    dragMoveCurrentPosition = ray.GetPoint(hitPoint);
                    targetPosition = transform.position + (dragMoveStartPosition - dragMoveCurrentPosition);
                }
            }

            if (input.MiddleMouseButton.WasReleasedThisFrame())
            {
                var difference = targetPosition - transform.position;
                if (difference.sqrMagnitude > moveVelocityDragThreshold)
                {
                    targetDragVelocity = new Vector3(difference.x, 0, difference.z) * moveVelocityDamping;
                }
            }

            // TODO: If split screen disable?
            if (!enableScreenEdgeMove) return;

            var screenEdgeMove = Vector3.zero;
            if (mousePosition.x < screenEdgeTolerance * Screen.width)
            {
                screenEdgeMove += -GetCameraRight();
            }
            else if (mousePosition.x > (1f - screenEdgeTolerance) * Screen.width)
            {
                screenEdgeMove += GetCameraRight();
            }

            if (mousePosition.y < screenEdgeTolerance * Screen.height)
            {
                screenEdgeMove += -GetCameraForward();
            }
            else if (mousePosition.y > (1f - screenEdgeTolerance) * Screen.height)
            {
                screenEdgeMove += GetCameraForward();
            }

            targetDragVelocity += screenEdgeMove * ((targetZoom / zoomMax) * moveSpeed * screenEdgeMoveSpeed);
        }

        private void CalculateMouseTargetRotation()
        {
            if (input.RightMouseButton.WasPressedThisFrame())
            {
                dragRotationStartPosition = input.MousePosition.ReadValue<Vector2>();
            }

            if (input.RightMouseButton.IsPressed())
            {
                dragRotationCurrentPosition = input.MousePosition.ReadValue<Vector2>();

                var difference = dragRotationStartPosition - dragRotationCurrentPosition;

                dragRotationStartPosition = dragRotationCurrentPosition;
                
                targetRotationX += -difference.x / rotationMouseFactor;
                targetRotationY += difference.y / rotationMouseFactor;
            }
        }

        private void CalculateMouseTargetZoom()
        {
            var inputValue = input.ScrollWheel.ReadValue<float>();
            
            if (inputValue != 0)
                targetZoom += -inputValue * zoomMouseFactor;
        }

        private void CalculateTargetPosition()
        {
            if (follow != null)
            {
                targetPosition = follow.position;
                return;
            }

            var inputValue = input.LeftStick.ReadValue<Vector2>();
            targetPosition += (inputValue.x * GetCameraRight() + inputValue.y * GetCameraForward()).normalized *
                              ((targetZoom / zoomMax) * moveSpeed);
        }

        private void CalculateTargetRotation()
        {
            var inputValue = input.RightStick.ReadValue<Vector2>();
            targetRotationX += inputValue.x * rotationSpeed;
            targetRotationY += -inputValue.y * rotationSpeed;
        }

        private void CalculateTargetZoom()
        {
            var inputValue = (input.RightShoulder.IsPressed() ? 1 : 0) + (input.LeftShoulder.IsPressed() ? -1 : 0);
            targetZoom +=  inputValue * zoomSpeed;
        }

        private void UpdatePosition(float dt)
        {
            if (targetDragVelocity.sqrMagnitude > 0.01f)
            {
                targetDragVelocity = Vector3.Lerp(targetDragVelocity, Vector3.zero, dt * moveDamping * 0.5f);
                targetPosition += targetDragVelocity * dt;
            }
            transform.position = Vector3.Lerp(transform.position, targetPosition, dt * moveDamping);
        }

        private void UpdateRotation(float dt)
        {
            targetRotationY = math.clamp(targetRotationY, rotationYMin, rotationYMax);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(Vector3.up * targetRotationX), dt * rotationDamping);
            angleTransform.localRotation = Quaternion.Slerp(angleTransform.localRotation, Quaternion.Euler(Vector3.right * targetRotationY), dt * rotationDamping);
        }

        private void UpdateZoom(float dt)
        {
            targetZoom = math.clamp(targetZoom, zoomMin, zoomMax);
            targetCameraTransform.localPosition = Vector3.Lerp(targetCameraTransform.localPosition, new Vector3(0, targetZoom, -targetZoom), dt * zoomDamping);
        }
    }
}