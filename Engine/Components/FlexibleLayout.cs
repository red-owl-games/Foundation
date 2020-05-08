using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace RedOwl.Core
{
    [HideMonoScript]
    public class FlexibleLayout : LayoutGroup
    {
        public enum FitType
        {
            Uniform,
            Width,
            Height,
            FixedRows,
            FixedColumns
        }
        
        [HorizontalGroup("Fit", 0.70f)]
        public FitType fitType;
        [HorizontalGroup("Fit")]
        [DisableIf("isAutoLayout"), LabelWidth(30)]
        public bool fitX;
        [HorizontalGroup("Fit")]
        [DisableIf("isAutoLayout"), LabelWidth(30)]
        public bool fitY;
        
        [HorizontalGroup("Grid", 0.60f)]
        [DisableIf("isAutoLayout"), DisableIf("isFixedColumns")]
        public int rows;
        [HorizontalGroup("Grid")]
        [DisableIf("isAutoLayout"), DisableIf("isFixedRows"), LabelWidth(80)]
        public int columns;
        
        [DisableIf("isAutoLayout")]
        public Vector2 cellSize;
        public Vector2 spacing;


        private bool isAutoLayout => fitType == FitType.Width || fitType == FitType.Height || fitType == FitType.Uniform;
        private bool isFixedRows => fitType == FitType.FixedRows;
        private bool isFixedColumns => fitType == FitType.FixedColumns;
        
        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();

            int childCount = transform.childCount;
            if (isAutoLayout)
            {
                fitX = true;
                fitY = true;
                float sqrRt = Mathf.Sqrt(childCount);
                rows = Mathf.CeilToInt(sqrRt);
                columns = Mathf.CeilToInt(sqrRt);
            }
            if (fitType == FitType.Width || fitType == FitType.FixedColumns)
                rows = Mathf.CeilToInt(childCount / (float) columns);
            if (fitType == FitType.Height || fitType == FitType.FixedRows) 
                columns = Mathf.CeilToInt(childCount / (float) rows);

            var parent = rectTransform.rect;

            var pad = padding;
            float cellWidth = (parent.width - spacing.x * (columns - 1)) / columns - pad.left / (float) columns - pad.right / (float) columns;
            float cellHeight = (parent.height - spacing.y * (rows - 1)) / rows - pad.top / (float) rows - pad.bottom / (float) rows;

            cellSize.x = fitX ? cellWidth : cellSize.x;
            cellSize.y = fitY ? cellHeight : cellSize.y;
            
            int columnCount = 0;
            int rowCount = 0;

            for (int i = 0; i < childCount; i++)
            {
                rowCount = i / columns;
                columnCount = i % columns;

                RectTransform item = rectChildren[i];

                float xPos = cellSize.x * columnCount + pad.left + spacing.x * columnCount;
                float yPos = cellSize.y * rowCount + pad.top + spacing.y * rowCount;
                
                SetChildAlongAxis(item, 0, xPos, cellSize.x);
                SetChildAlongAxis(item, 1, yPos, cellSize.y);
            }
        }

        public override void CalculateLayoutInputVertical()
        {
        }

        public override void SetLayoutHorizontal()
        {
        }

        public override void SetLayoutVertical()
        {
        }
    }
}
