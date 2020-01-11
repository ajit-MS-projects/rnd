using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace PvScoutWeb.Views
{
    public partial class PvModulView : UserControl
    {
        #region Private Members

        private Boolean isLeftMouseButtonPressed = false;
        private bool _isSelected;
        private double xPos;
        private double yPos;
        public double ActualTopDistance { get; set; }
        public double ActualLeftDistance { get; set; }

        // public event
        public event ModulCoordinatesChanged ModCoordinatesChanged;
        public delegate void ModulCoordinatesChanged(PvModulView sender);
        // public event, fires when MouseButtonUp is fired
        public event ModulDropped ModDropped;
        public delegate void ModulDropped();

        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                // todo: impl. OnPropertyChange handler to change the color of whatever ;-)
                // OnPropertyChange("IsSelected");
                if (_isSelected)
                {
                    this.myBorder.Style = Application.Current.Resources["PvModulBorderSelectedStyle"] as Style;
                    this.myInnerBorder.Style = Application.Current.Resources["PvModulInnerBorderSelectedStyle"] as Style;
                }
                else
                {
                    this.myBorder.Style = Application.Current.Resources["PvModulBorderStyle"] as Style;
                    this.myInnerBorder.Style = Application.Current.Resources["PvModulInnerBorderStyle"] as Style;
                }
            }

        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the PvModulView class.
        /// </summary>
        public PvModulView()
        {
            InitializeComponent();
            this.MouseLeftButtonDown += new MouseButtonEventHandler(OnMouseLeftButtonDown);
        }

        #region ClickPosition Dependency Property

        /// <summary>
        /// Position of the click
        /// </summary>
        public Point ClickPosition
        {
            get { return (Point)GetValue(ClickPositionProperty); }
            set { SetValue(ClickPositionProperty, value); }
        }

        /// <summary>
        /// Position of the click
        /// // Using a DependencyProperty as the backing store for ClickPosition.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty ClickPositionProperty = DependencyProperty.Register("ClickPosition", typeof(Point), typeof(PvModulView), new PropertyMetadata(new Point(0.0, 0.0)));

        #endregion

        #region Dragging Methods
        /// <summary>
        /// PvModul start move
        /// </summary>
        /// <param name="sender">Clicked PvModul</param>
        /// <param name="e">MouseButtonEventArgs</param>
        void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isLeftMouseButtonPressed = true;
            ClickPosition = e.GetPosition(null); // get the click position

            // save the actual position before the move starts - important for DropPositionOk check
            this.ActualTopDistance = Canvas.GetTop(this);
            this.ActualLeftDistance = Canvas.GetLeft(this);

            this.CaptureMouse();
            this.Cursor = Cursors.Hand;

            // bind the eventhandler
            this.MouseLeftButtonUp += new MouseButtonEventHandler(this.OnMouseLeftButtonUp);
            this.MouseMove += new MouseEventHandler(this.OnMouseMove);
        }

        /// <summary>
        /// PvModul move
        /// </summary>
        /// <param name="sender">Clicked PvModul</param>
        /// <param name="e">MouseEventArgs</param>
        void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (isLeftMouseButtonPressed)
            {
                // Retrieving the item's current x and y position
                xPos = (e.GetPosition(null).X - ClickPosition.X); // / CanvasScaleTransform.ScaleX;
                yPos = (e.GetPosition(null).Y - ClickPosition.Y); // / CanvasScaleTransform.ScaleY;

                // Re-position the moved element
                this.SetValue(Canvas.TopProperty, yPos + (double)this.GetValue(Canvas.TopProperty));
                this.SetValue(Canvas.LeftProperty, xPos + (double)this.GetValue(Canvas.LeftProperty));

                // Reset the new position value
                ClickPosition = e.GetPosition(null);
                // event, that all selected will be moved too
                this.ModCoordinatesChanged(this);
            }
        }

        /// <summary>
        /// PvModul mouse up
        /// </summary>
        /// <param name="sender">PvModul</param>
        /// <param name="e">MouseButtonEventArgs</param>
        void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isLeftMouseButtonPressed)
            {
                isLeftMouseButtonPressed = false;
                // Removes Mouse Capture from Element being dragged
                this.ReleaseMouseCapture();
                this.MouseMove -= OnMouseMove;
                this.MouseLeftButtonUp -= OnMouseLeftButtonUp;

                this.ModDropped();  // fire public event so that all selectedModuls will be informed that the MouseButtonUp event was fired -> check the dropPosition can be fired                
            }
        }

        #endregion

        #region helper
        /// <summary>
        /// Moves a modules in relation to the moved module
        /// </summary>
        /// <param name="mod"></param>
        public void MoveModul(PvModulView mod)
        {
            // Re-position the moved element
            this.SetValue(Canvas.TopProperty, mod.yPos + (double)this.GetValue(Canvas.TopProperty));
            this.SetValue(Canvas.LeftProperty, mod.xPos + (double)this.GetValue(Canvas.LeftProperty));
        }

        internal void SetModulBackToStartPosition()
        {
            this.SetValue(Canvas.TopProperty, this.ActualTopDistance);
            this.SetValue(Canvas.LeftProperty, this.ActualLeftDistance);
        }

        /// <summary>
        /// Save the actual Top and Left distance
        /// </summary>
        internal void SaveActualPosition()
        {
            // save the actual position before the move starts
            this.ActualTopDistance = Canvas.GetTop(this);
            this.ActualLeftDistance = Canvas.GetLeft(this);
        }
        #endregion
    }
}
