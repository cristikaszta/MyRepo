using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using DisertationProject.Model;

namespace DisertationProject.Controller
{
    public class VisualFeedbackController
    {
        /// <summary>
        /// The main activity
        /// </summary>
        private MainActivity _mainActivity;

        /// <summary>
        /// Message displayed below buttons
        /// </summary>
        public TextContainer TextContainer { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mainActivity"></param>
        public VisualFeedbackController(MainActivity mainActivity)
        {
            _mainActivity = mainActivity;
            TextContainer = new TextContainer(_mainActivity, Globals.ErrorTextBox);
        }

        /// <summary>
        /// Initialization
        /// </summary>
        public void Initialize()
        {
           //
        }


    }
}