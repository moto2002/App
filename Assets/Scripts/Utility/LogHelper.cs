using System;
using UnityEngine;

public sealed class LogHelper {
    private LogHelper(){

    }

    public static void Log(object message){
        if (!DebugHelper.DEBUG){
            return;
        }
        Debug.Log(TimeHelper.FormattedTimeNow() + " " + message.ToString());
    }
    
    public static void LogError(object message){
        if (!DebugHelper.DEBUG){
            return;
        }
        Debug.LogError(TimeHelper.FormattedTimeNow() + " " + message.ToString());
    }

    public static void LogError(object message, object content){
        if (!DebugHelper.DEBUG){
            return;
        }
        Debug.LogError(TimeHelper.FormattedTimeNow() + " " + message.ToString());
    }

    public static void LogWarning(object message){
        if (!DebugHelper.DEBUG){
            return;
        }
        Debug.LogWarning(TimeHelper.FormattedTimeNow() + " " + message.ToString());
    }

    public static void LogWarning(object message, object content){
        if (!DebugHelper.DEBUG){
            return;
        }
        Debug.LogWarning(TimeHelper.FormattedTimeNow() + " " + message.ToString());
    }

    public static void LogException(Exception exception){
        if (!DebugHelper.DEBUG){
            return;
        }
        LogHelper.Log(TimeHelper.FormattedTimeNow());
        LogHelper.LogException(exception);
    }
}
