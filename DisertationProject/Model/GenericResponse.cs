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

namespace DisertationProject.Model
{
    public class GenericResponse
    {
        public GenericStatus Status;

        public string message;
    }

    public class GenericResponse<T>
    {
        public GenericStatus Status;

        public string Message;

        public T Result;
    }
}