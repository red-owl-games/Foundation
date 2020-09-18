using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RedOwl.Engine
{
    [HideMonoScript]
    public class BuildVersion : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text text;
        
        private void Start()
        {
            if (text == null) text = GetComponent<TMP_Text>();
            text.text = Application.version;
        }
    }
}