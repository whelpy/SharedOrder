using System;

public static class Raise
{
    public static void Event(EventHandler eventHandler, object sender, EventArgs e)
    {
        if (eventHandler != null)
        {
            eventHandler(sender, e);
        }
    }
}