/*
 * 
 */

using UnityEngine;

namespace Revy.Framework.Localization
{
    public class FSTextSetting : FScriptableObject
    {
        #region Fields
#pragma warning disable 649
        [SerializeField]
        private Font _font;
#pragma warning restore 649
        [SerializeField]
        private FontStyle _fontStyle = FontStyle.Normal;
        [SerializeField]
        private int _fontSize = 30;
        [SerializeField]
        private int _lineSpacing = 1;
        [SerializeField]
        private bool _richText = false;
        [SerializeField]
        private TextAnchor _alignment = TextAnchor.MiddleCenter;
        [SerializeField]
        private bool _alignByGeometry = false;
        [SerializeField]
        private HorizontalWrapMode _horizontalOverflow = HorizontalWrapMode.Wrap;
        [SerializeField]
        private VerticalWrapMode _verticalOverflow = VerticalWrapMode.Truncate;
        [SerializeField]
        private bool _bestFit = false;
        [SerializeField]
        private Color _color = Color.white;
        [SerializeField]
        private bool _raycastTarget = false;
        #endregion

        #region Properties
        public Font Font { get { return _font; } }
        public FontStyle FontStyle { get { return _fontStyle; } }
        public int FontSize { get { return _fontSize; } }
        public int LineSpacing { get { return _lineSpacing; } }
        public bool RichText { get { return _richText; } }
        public TextAnchor Alignment { get { return _alignment; } }
        public bool AlignByGeometry { get { return _alignByGeometry; } }
        public HorizontalWrapMode HorizontalOverflow { get { return _horizontalOverflow; } }
        public VerticalWrapMode VerticalOverflow { get { return _verticalOverflow; } }
        public bool BestFit { get { return _bestFit; } }
        public Color Color { get { return _color; } }
        public bool RaycastTarget { get { return _raycastTarget; } }
        #endregion
    }
}