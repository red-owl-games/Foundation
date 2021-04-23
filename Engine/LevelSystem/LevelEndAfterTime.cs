using System;
using UnityEngine;

namespace RedOwl.Engine
{
    public class LevelEndAfterTime : MonoBehaviour
    {
        public Cooldown duration;

        private void Start()
        {
            duration.Use();
        }

        private void Update()
        {
            duration.Tick(Time.deltaTime);
            if (!duration.IsReady) return;
            Game.LoadNextLevel();
            Destroy(this);
        }
    }
}