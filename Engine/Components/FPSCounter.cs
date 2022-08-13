using System;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

namespace RedOwl.Engine
{
    public class FPSCounter : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text text;
        
        private void Start()
        {
            if (text == null) text = GetComponent<TMP_Text>();
        }

        private void Update()
        {
            var fps = math.clamp((int)(1f / Time.unscaledDeltaTime), 0, 61);
            text.text = StringsFrom00To60[fps];
        }
        
        private static readonly string[] StringsFrom00To60 = {
            "00", "01", "02", "03", "04", "05", "06", "07", "08", "09",
            "10", "11", "12", "13", "14", "15", "16", "17", "18", "19",
            "20", "21", "22", "23", "24", "25", "26", "27", "28", "29",
            "30", "31", "32", "33", "34", "35", "36", "37", "38", "39",
            "40", "41", "42", "43", "44", "45", "46", "47", "48", "49",
            "50", "51", "52", "53", "54", "55", "56", "57", "58", "59",
            "60", ">60"
        };
    }
}