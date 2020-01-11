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
    public partial class RoofView : UserControl
    {
        #region Private Members
        private List<PvModulView> _selectedModules;
        // to store the actual highest z-index; important for D&D; that the moved PvModul is always on top
        private int maxZindex = 1;
        #endregion

        /// <summary>
        /// Initializes a new instance of the RoofView class.
        /// </summary>
        public RoofView()
        {
            InitializeComponent();

            _selectedModules = new List<PvModulView>();
        }

        #region Methods
        public void FillRoof()
        {
            foreach (PvModulView modul in Helper.GetModulsFromService())
            {
                modul.MouseLeftButtonDown += new MouseButtonEventHandler(modul_MouseLeftButtonDown);
                modul.ModCoordinatesChanged += new PvModulView.ModulCoordinatesChanged(selModul_ModCoordinatesChanged);
                modul.ModDropped += new PvModulView.ModulDropped(modul_Dropped);
                RoofArea.Children.Add(modul);
            }
        }



        /// <summary>
        /// Add/removes selected modules into/from a list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void modul_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            PvModulView selModul = sender as PvModulView;
            // increase the zindex so that the clicked element is always on top
            IncreaseZIndex(selModul);

            // add the selected modul to list of selected modules to enable multiple D&D
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                if (!_selectedModules.Contains(selModul))
                {
                    _selectedModules.Add(selModul);
                    selModul.IsSelected = true;
                }
                else
                {
                    _selectedModules.Remove(selModul);
                    selModul.IsSelected = false;
                }
            }
            else
            {
                // user clicked on a not selected modul -> -> remove all selected modules from selectedList and add the clicked modul to selectedList
                if (!_selectedModules.Contains(selModul))
                {
                    foreach (var mod in _selectedModules)
                    {
                        mod.IsSelected = false;
                    }
                    _selectedModules.Clear();

                    _selectedModules.Add(selModul);
                    selModul.IsSelected = true;
                }
                else // start moving the selected controls -> before save the start position of each control
                {
                    foreach (var mod in _selectedModules)
                    {
                        mod.SaveActualPosition(); // before the move starts
                    }
                }

            }
        }

        /// <summary>
        /// Moves all the selected modules
        /// </summary>
        /// <param name="sender"></param>
        void selModul_ModCoordinatesChanged(PvModulView sender)
        {
            if (_selectedModules.Count > 1)
            {
                foreach (PvModulView mod in _selectedModules)
                {
                    if (sender != mod)  // not move the sender module again!
                    {
                        mod.MoveModul(sender);
                    }
                }
            }
        }

        void modul_Dropped()
        {
            if (_selectedModules.Count > 0)
            {
                foreach (var mod in _selectedModules)
                {
                    if (!DropPositionOk(mod))
                    {
                        // if position for one of the moved controls is not ok jump all modules back to start position before drag started
                        // let the modul jump back to the start position
                        SetModulesBackToStartPosition(_selectedModules);
                        break;
                    }
                }
            }
        }

        private bool DropPositionOk(PvModulView mod)
        {
            return true;
        }

        /// <summary>
        /// Set all selected Modules back to the position before the drag started
        /// </summary>
        /// <param name="_selectedModules"></param>
        private void SetModulesBackToStartPosition(List<PvModulView> _selectedModules)
        {
            foreach (var mod in _selectedModules)
            {
                mod.SetModulBackToStartPosition();
            }
        }

        public void ChangeRoofSize(double width, double height)
        {
            this.Width = width;
            this.Height = height;
        }
        #endregion

        #region helper
        internal void IncreaseZIndex(UIElement element)
        {
            maxZindex += 1;
            Canvas.SetZIndex(element, maxZindex);

        }
        #endregion

    }
}
