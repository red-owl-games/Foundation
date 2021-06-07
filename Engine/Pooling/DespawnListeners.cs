using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace RedOwl.Engine
{
    [HideMonoScript]
    public class DespawnListener : MonoBehaviour
    {
        [HideInInspector]
        public Spawnable spawnable;

        protected void DoDespawn(GameObject other)
        {
            if (!spawnable.useFilter) spawnable.Despawn();
            if (spawnable.Filter.ShouldDespawn(other)) spawnable.Despawn();
        }
    }
    
    #region OnCollision
    public class SpawnableOnCollisionEnterListener : DespawnListener
    {
        private void OnCollisionEnter(Collision other) { DoDespawn(other.gameObject); }
    }
    public class SpawnableOnCollisionStayListener : DespawnListener
    {
        private void OnCollisionStay(Collision other) { DoDespawn(other.gameObject); }
    }
    public class SpawnableOnCollisionExitListener : DespawnListener
    {
        private void OnCollisionExit(Collision other) { DoDespawn(other.gameObject); }
    }
    #endregion
    
    #region OnTrigger
    public class SpawnableOnTriggerEnterListener : DespawnListener
    {
        private void OnTriggerEnter(Collider other) { DoDespawn(other.gameObject); }
    }
    public class SpawnableOnTriggerStayListener : DespawnListener
    {
        private void OnTriggerStay(Collider other) { DoDespawn(other.gameObject); }
    }
    public class SpawnableOnTriggerExitListener : DespawnListener
    {
        private void OnTriggerExit(Collider other) { DoDespawn(other.gameObject); }
    }
    #endregion
    
    #region OnCollision2D
    public class SpawnableOnCollisionEnter2DListener : DespawnListener
    {
        private void OnCollisionEnter2D(Collision2D other) { DoDespawn(other.gameObject); }
    }
    public class SpawnableOnCollisionStay2DListener : DespawnListener
    {
        private void OnCollisionStay2D(Collision2D other) { DoDespawn(other.gameObject); }
    }
    public class SpawnableOnCollisionExit2DListener : DespawnListener
    {
        private void OnCollisionExit2D(Collision2D other) { DoDespawn(other.gameObject); }
    }
    #endregion
    
    #region OnTrigger2D
    public class SpawnableOnTriggerEnter2DListener : DespawnListener
    {
        private void OnTriggerEnter2D(Collider2D other) { DoDespawn(other.gameObject); }
    }
    public class SpawnableOnTriggerStay2DListener : DespawnListener
    {
        private void OnTriggerStay2D(Collider2D other) { DoDespawn(other.gameObject); }
    }
    public class SpawnableOnTriggerExit2DListener : DespawnListener
    {
        private void OnTriggerExit2D(Collider2D other) { DoDespawn(other.gameObject); }
    }
    #endregion
}