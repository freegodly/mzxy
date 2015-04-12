using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Globalization;
using 梦之西游.Controls;

namespace 梦之西游.Controls
{

    public partial class TextShow : UserControl, GameObject
    {

        /// <summary>
        /// 描边文字控件
        /// </summary>
        public TextShow() {
            InitializeComponent();
        }

        Geometry TextGeometry; //文字路径

        /// <summary>
        /// 生成描边文字
        /// </summary>
        public void CreateText() {
            FormattedText formattedText = new FormattedText(
                Text,
                CultureInfo.GetCultureInfo("zh-cn"),
                FlowDirection.LeftToRight,
                new Typeface(Font, Italic ? FontStyles.Italic : FontStyles.Normal, Bold ? FontWeights.Bold : FontWeights.Normal, FontStretches.Normal),
                FontSize,
                Brushes.Black
                );
            //根据文字创建路径
            TextGeometry = formattedText.BuildGeometry(new Point(0, 0));
            this.MinWidth = formattedText.Width; this.MinHeight = formattedText.Height;
        }

        /// <summary>
        /// 回调重绘
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void QXTextInvalidated(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((TextShow)d).CreateText();
        }

        /// <summary>
        /// 重载控件OnRender
        /// </summary>
        protected override void OnRender(DrawingContext drawingContext) {
            CreateText();
            drawingContext.DrawGeometry(Fill, new Pen(Stroke, StrokeThickness), TextGeometry);
        }

        /// <summary>
        /// 获取或设置中心距离左边距离
        /// </summary>
        public double CenterX { get; set; }

        /// <summary>
        /// 获取或设置中心距离顶边距离
        /// </summary>
        public double CenterY { get; set; }

        /// 获取或设置X值
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// 获取或设置Y值
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// 获取或设置与父画布容器左边距离
        /// </summary>
        public double Left {
            get { return (double)this.GetValue(Canvas.LeftProperty); }
            set { this.SetValue(Canvas.LeftProperty, value); }
        }

        /// <summary>
        /// 获取或设置与父画布容器顶边距离
        /// </summary>
        public double Top {
            get { return (double)this.GetValue(Canvas.TopProperty); }
            set { this.SetValue(Canvas.TopProperty, value); }
        }

        /// <summary>
        /// 获取或设置层次
        /// </summary>
        public int ZIndex {
            get { return (int)this.GetValue(Canvas.ZIndexProperty); }
            set { this.SetValue(Canvas.ZIndexProperty, value); }
        }

        /// <summary>
        /// 获取或设置宽
        /// </summary>
        public double Width_ {
            get { return this.MinWidth; }
            set { this.FontSize *= value / MinWidth; this.MinWidth *= value / MinWidth; }
        }

        /// <summary>
        /// 获取或设置高
        /// </summary>
        public double Height_ {
            get { return this.MinHeight; }
            set { this.MinHeight *= value / MinHeight; }
        }

        /// <summary>
        /// 获取或设置文字是否加粗
        /// </summary>
        public bool Bold {
            get { return (bool)GetValue(BoldProperty); }
            set { SetValue(BoldProperty, value); }
        }

        public static readonly DependencyProperty BoldProperty = DependencyProperty.Register(
            "Bold",
            typeof(bool),
            typeof(TextShow),
            new FrameworkPropertyMetadata(
                false,
                FrameworkPropertyMetadataOptions.AffectsRender,
                new PropertyChangedCallback(QXTextInvalidated),
                null
                )
            );

        /// <summary>
        /// 获取或设置文字是否倾斜
        /// </summary>
        public bool Italic {
            get { return (bool)GetValue(ItalicProperty); }
            set { SetValue(ItalicProperty, value); }
        }

        public static readonly DependencyProperty ItalicProperty = DependencyProperty.Register(
            "Italic",
            typeof(bool),
            typeof(TextShow),
            new FrameworkPropertyMetadata(
                 false,
                 FrameworkPropertyMetadataOptions.AffectsRender,
                 new PropertyChangedCallback(QXTextInvalidated),
                 null
                 )
            );

        /// <summary>
        /// 获取或设置文字颜色
        /// </summary>
        public Brush Fill {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        public static readonly DependencyProperty FillProperty = DependencyProperty.Register(
            "Fill",
            typeof(Brush),
            typeof(TextShow),
            new FrameworkPropertyMetadata(
                new SolidColorBrush(Colors.Black),
                FrameworkPropertyMetadataOptions.AffectsRender,
                new PropertyChangedCallback(QXTextInvalidated),
                null
                )
            );

        /// <summary>
        /// 获取或设置字体类型
        /// </summary>
        public FontFamily Font {
            get { return (FontFamily)GetValue(FontProperty); }
            set { SetValue(FontProperty, value); }
        }

        public static readonly DependencyProperty FontProperty = DependencyProperty.Register(
            "Font",
            typeof(FontFamily),
            typeof(TextShow),
            new FrameworkPropertyMetadata(
                new FontFamily("微软雅黑"),
                FrameworkPropertyMetadataOptions.AffectsRender,
                new PropertyChangedCallback(QXTextInvalidated),
                null
                )
            );

        /// <summary>
        /// 获取或设置文字大小
        /// </summary>
        public double Size {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
            "Size",
            typeof(double),
            typeof(TextShow),
            new FrameworkPropertyMetadata(
                 (double)12,
                 FrameworkPropertyMetadataOptions.AffectsRender,
                 new PropertyChangedCallback(QXTextInvalidated),
                 null
                 )
            );

        /// <summary>
        /// 描边笔刷
        /// </summary>
        public Brush Stroke {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(
            "Stroke",
            typeof(Brush),
            typeof(TextShow),
            new FrameworkPropertyMetadata(
                 new SolidColorBrush(Colors.White),
                 FrameworkPropertyMetadataOptions.AffectsRender,
                 new PropertyChangedCallback(QXTextInvalidated),
                 null
                 )
            );

        /// <summary>
        /// 描边宽度
        /// </summary>
        public double StrokeThickness {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(
            "StrokeThickness",
            typeof(double),
            typeof(TextShow),
            new FrameworkPropertyMetadata(
                 (double)0,
                 FrameworkPropertyMetadataOptions.AffectsRender,
                 new PropertyChangedCallback(QXTextInvalidated),
                 null
                 )
            );

        /// <summary>
        /// 文字内容
        /// </summary>
        public string Text {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text",
            typeof(string),
            typeof(TextShow),
            new FrameworkPropertyMetadata(
                 "",
                 FrameworkPropertyMetadataOptions.AffectsRender,
                 new PropertyChangedCallback(QXTextInvalidated),
                 null
                 )
            );
    }
}
