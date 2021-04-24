using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Engine
{
    [ExecuteInEditMode]
    public class PlayerId : MonoBehaviour
    {
        private static Dictionary<int, GameObject> _players;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void OnSubsystemRegistration()
        {
            _players = null;
        }

        public static IEnumerable<(int, GameObject)> GetPlayers()
        {
            foreach (var kvp in _players)
            {
                if (kvp.Value != null)
                    yield return (kvp.Key, kvp.Value);
            }
        }

        public static GameObject GetPlayer(int i)
        {
            _players.TryGetValue(i - 1, out var player);
            return player;
        }

        [ShowInInspector, ReadOnly] 
        public int Id { get; private set; }

        [Range(-1, 3)]
        public int OverrideId = -1;

        public string Name => $"Player{Id}";

        [Button]
        private void OnEnable()
        {
            if (_players == null) _players = new Dictionary<int, GameObject>(4);
            if (OverrideId != -1)
            {
                SetPlayer(OverrideId);
                return;
            }
            for (int i = 1; i < 5; i++)
            {
                if (_players.TryGetValue(i, out var player))
                {
                    if (player == null)
                    {
                        SetPlayer(i);
                        return;
                    }
                }
                else
                {
                    SetPlayer(i);
                    return;
                }
            }
            transform.Destroy();
        }

        private void OnDisable()
        {
            if (_players == null) return;
            _players[Id] = null;
        }

        private void SetPlayer(int id)
        {
            _players[id] = gameObject;
            Id = id;
            gameObject.name = Name;
        }
    }
}