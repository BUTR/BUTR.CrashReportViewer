﻿using Quartz;

using System;
using System.Threading;

namespace BUTR.Site.NexusMods.Server.Models.Quartz;

public class TriggerEventArgs : EventArgs
{
    public ITrigger Trigger { get; init; }
    public IJobExecutionContext JobExecutionContext { get; init; }
    public CancellationToken CancelToken { get; init; }

    public TriggerEventArgs(ITrigger trigger, IJobExecutionContext context, CancellationToken cancelToken = default)
    {
        Trigger = trigger;
        JobExecutionContext = context;
        CancelToken = cancelToken;
    }
}