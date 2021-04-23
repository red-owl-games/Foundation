using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RedOwl.Engine
{
    public interface ITooltipService : IService
    {
        void Show(Vector3 position, string content, string header = "", float hideDelay = 1f);
        void Show(Func<Vector3> position, string content, string header = "");
        void Hide();
    }
    
    public class TooltipService : Service, ITooltipService
    {
        private static ITooltipView _tooltipView;
        
        public override void Init()
        {
            //Log.Always("Tooltips Initialization!");
            if (Game.TooltipSettings.prefab != null)
            {
                var go = Object.Instantiate(Game.TooltipSettings.prefab);
                _tooltipView = go.GetComponent<ITooltipView>();
                Object.DontDestroyOnLoad(go);
            }
        }
        
        public void Show(Vector3 position, string content, string header = "", float hideDelay = 1f)
        {
            _tooltipView.Show(() => position, content, header);
            Delayed.Run(Hide, hideDelay);
        }

        public void Show(Func<Vector3> position, string content, string header = "") 
            => _tooltipView.Show(position, content, header);

        public void Hide() => _tooltipView.Hide();
    }

    public partial class Game
    {
        public static ITooltipService Tooltip => Get<ITooltipService>();

        public static void AddTooltipService() => Add<ITooltipService>(new TooltipService());
    }
}