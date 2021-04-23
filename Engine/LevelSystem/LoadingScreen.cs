using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedOwl.Engine
{
    public interface ILoadingScreen
    {
        IEnumerator DoShow();
        IEnumerator DoHide();
    }
    
    [HideMonoScript]
    [RequireComponent(typeof(VerandaView))]
    public class LoadingScreen : MonoBehaviour, ILoadingScreen
    {
        private VerandaView _view;
        
        private void Awake()
        {
            _view = GetComponent<VerandaView>();
            Game.Bind<ILoadingScreen>(this);
        }

        public IEnumerator DoShow()
        {
            return _view.DoShow();
        }

        public IEnumerator DoHide()
        {
            return _view.DoHide();
        }

        public static IEnumerator Show()
        {
            var screen = Game.Find<ILoadingScreen>();
            if (screen != null) yield return screen.DoShow();
        }
        
        public static IEnumerator Hide()
        {
            var screen = Game.Find<ILoadingScreen>();
            if (screen != null) yield return screen.DoHide();
        }
    }
}