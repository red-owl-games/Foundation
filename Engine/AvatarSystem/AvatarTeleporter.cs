using UnityEngine;
using UnityEngine.Events;

namespace RedOwl.Engine
{
    public class AvatarTeleporter : MonoBehaviour
    {
        public AvatarTeleporter TeleportTo;

        public UnityAction<Avatar> OnCharacterTeleport;

        public bool isBeingTeleportedTo { get; set; }

        private void OnTriggerEnter(Collider other)
        {
            if (!isBeingTeleportedTo)
            {
                Avatar cc = other.GetComponent<Avatar>();
                if (cc)
                {
                    var teleportTransform = TeleportTo.transform;
                    cc.Motor.SetPositionAndRotation(teleportTransform.position, teleportTransform.rotation);
                    OnCharacterTeleport?.Invoke(cc);
                    TeleportTo.isBeingTeleportedTo = true;
                }
            }

            isBeingTeleportedTo = false;
        }
    }
}