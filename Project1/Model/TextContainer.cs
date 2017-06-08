using Android.Widget;
using DisertationProject.Controller;
using System;

namespace DisertationProject.Model
{
    /// <summary>
    /// Text container class
    /// </summary>
    public class TextContainer
    {
        /// <summary>
        /// Text to be displayed
        /// </summary>
        private string _text;

        /// <summary>
        /// Textview object which containes the text
        /// </summary>
        private TextView _textView;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mainActivity">Main activity context</param>
        /// <param name="textBoxId">Text box id</param>
        public TextContainer(MainActivity mainActivity, int textBoxId)
        {
            _text = "";
            _textView = (TextView)mainActivity.FindViewById(Globals.ErrorTextBox);
        }

        /// <summary>
        /// Method used to set the text in a text view
        /// </summary>
        /// <param name="text">Input text</param>
        public void SetText(string text)
        {
            _text = text;
            _textView.Text = text;
        }

        /// <summary>
        /// Method used to get the text from a textview
        /// </summary>
        /// <returns>Text from the textview</returns>
        public string GetText()
        {
            return _text;
        }
    }
}