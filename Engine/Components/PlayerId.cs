using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Engine
{
    [ExecuteInEditMode]
    public class PlayerId : MonoBehaviour
    {
        [ShowInInspector]
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

        [Button]
        private void OnEnable()
        {
            if (_players == null) _players = new Dictionary<int, GameObject>(4);
            for (int i = 1; i < 5; i++)
            {
                if (_players.TryGetValue(i, out var player))
                {
                    if (player == null)
                    {
                        _players[i] = gameObject;
                        Id = i;
                        return;
                    }
                }
                else
                {
                    _players[i] = gameObject;
                    Id = i;
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
    }
}