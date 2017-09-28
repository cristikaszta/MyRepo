using System.ComponentModel;

namespace DisertationProject.Model
{
    //Enumerations
    public enum ActionEvent
    {
        [Description("PLAY")]
        ActionPlay,
        [Description("PAUSE")]
        ActionPause,
        [Description("ActionSTOP")]
        ActionStop,
        [Description("PREVIOUS")]
        ActionPrevious,
        [Description("NEXT")]
        ActionNext,
        [Description("REPEAT_ON")]
        ActionRepeatOn,
        [Description("REPEAT_OFF")]
        ActionRepeatOff,
        [Description("SHUFFLE_ON")]
        ActionShuffleOn,
        [Description("SHUFFLE_OFF")]
        ActionShuffleOff
    }

    //public enum Errors
    //{
    //    [Description("GeneralException")]
    //    GeneralException,
    //    [Description("IllegalStateException")]
    //    IllegalStateException,
    //    [Description("NetworkOffline")]
    //    NetworkOffline,
    //    [Description("DatabaseError")]
    //    DatabaseError
    //}

    public enum Emotion
    {
        [Description("Sad")]
        Sad,
        [Description("Happy")]
        Happy,
        [Description("Neutral")]
        Neutral,
        [Description("Angry")]
        Angry
    }
    public enum TextType
    {
        [Description("Info")]
        Info,
        [Description("Warning")]
        Warning,
        [Description("Error")]
        Error
    }
    public enum ToggleState
    {
        [Description("On")]
        On,
        [Description("Off")]
        Off,
    }
    public enum PlayerState
    {
        [Description("Playing")]
        Playing,
        [Description("Stopped")]
        Stopped,
        [Description("Paused")]
        Paused
    }

}