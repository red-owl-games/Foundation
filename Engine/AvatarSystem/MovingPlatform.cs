using KinematicCharacterController;
using UnityEngine;

namespace RedOwl.Engine
{
   public class MovingPlatform : MonoBehaviour, IMoverController
   {
       public Transform target;
       
       private PhysicsMover _mover;
   
       private void Awake()
       {
           _mover = GetComponent<PhysicsMover>();
       }
   
       private void Start()
       {
           _mover.MoverController = this;
       }
           
       public void UpdateMovement(out Vector3 goalPosition, out Quaternion goalRotation, float deltaTime)
       {
           goalPosition = target.position;
           goalRotation = target.rotation;
       }
   } 
}

