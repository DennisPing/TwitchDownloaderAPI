using System.Runtime.CompilerServices;
using TwitchDownloaderAPI.Controllers;
using TwitchDownloaderCore.Interfaces;

namespace TwitchDownloaderAPI.Tools;

/// <summary>
/// Task Progress for web API usage. CLI related methods are no-op.
/// </summary>
/// <param name="logger"></param>
public class ApiTaskProgress(ILogger<ChatLogController> logger) : ITaskProgress
{
    public void LogVerbose(string logMessage)
    {
        logger.LogDebug("{message}", logMessage);
    }

    public void LogVerbose(DefaultInterpolatedStringHandler logMessage)
    {
        logger.LogDebug("{message}", logMessage.ToString());
    }

    public void LogInfo(string logMessage)
    {
        logger.LogInformation("{message}", logMessage);
    }

    public void LogInfo(DefaultInterpolatedStringHandler logMessage)
    {
        logger.LogInformation("{message}", logMessage.ToString());
    }

    public void LogWarning(string logMessage)
    {
        logger.LogWarning("{message}", logMessage);
    }

    public void LogWarning(DefaultInterpolatedStringHandler logMessage)
    {
        logger.LogWarning("{message}", logMessage.ToString());
    }

    public void LogError(string logMessage)
    {
        logger.LogError("{message}", logMessage);
    }

    public void LogError(DefaultInterpolatedStringHandler logMessage)
    {
        logger.LogError("{message}", logMessage.ToString());
    }

    public void LogFfmpeg(string logMessage)
    {
        throw new NotImplementedException();
    }

    public void SetStatus(string status)
    {
        // no-op
    }

    public void SetTemplateStatus(string status, int initialPercent)
    {
        // no-op
    }

    public void SetTemplateStatus(string status, int initialPercent, TimeSpan initialTime1, TimeSpan initialTime2)
    {
        // no-op
    }

    public void ReportProgress(int percent)
    {
        // no-op
    }

    public void ReportProgress(int percent, TimeSpan time1, TimeSpan time2)
    {
        // no-op
    }
}