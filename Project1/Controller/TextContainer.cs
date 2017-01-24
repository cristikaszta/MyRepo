using Android.Widget;
using DisertationProject.Model;

namespace DisertationProject.Controller
{
    public class TextContainer
    {
        private string _text;
        public TextView _textView;

        public TextContainer(MainActivity mainActivity,int textBoxId)
        {
            _text = "";
            _textView = (TextView)mainActivity.FindViewById(Globals.ErrorTextBox);
        }
        public void SetText(string text)
        {
            _text = text;
            _textView.Text = text;
        }

        public string GetText()
        {
            return _text;
        }


    }
}