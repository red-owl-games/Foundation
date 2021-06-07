using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RedOwl.Engine
{
    public interface ITooltipService
    {
        void Show(Vector3 position, string content, string header = "", float hideDelay = 1f);
        void Show(Func<Vector3> position, string content, string header = "");
        void Hide();
    }
    
    public class TooltipService : IServiceStart, ITooltipService
    {
        private static ITooltipView _tooltipView;

        public void Start()
        {
            if (GameSettings.TooltipSettings.prefab != null)
            {
                var go = Object.Instantiate(GameSettings.TooltipSettings.prefab);
                _tooltipView = go.GetComponent<ITooltipView>();
                Object.DontDestroyOnLoad(go);
            }
        }
        
        public void Show(Vector3 position, string content, string header = "", float hideDelay = 1f)
        {
            _tooltipView.Show(() => position, content, header);
            Game.DelayedCall(Hide, hideDelay);
        }

        public void Show(Func<Vector3> position, string content, string header = "") 
            => _tooltipView.Show(position, content, header);

        public void Hide() => _tooltipView.Hide();
    }

    public partial class Game
    {
        public static ITooltipService Tooltip => Find<ITooltipService>();

        public static void BindTooltipService() => Bind<ITooltipService>(new TooltipService());
    }
}