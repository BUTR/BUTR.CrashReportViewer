﻿using System;

namespace BUTR.Site.NexusMods.Server.Models.Database;

public sealed record NexusModsExposedModsEntity : IEntity
{
    public required int NexusModsModId { get; init; }

    public required string[] ModuleIds { get; init; }

    public required DateTime LastCheckedDate { get; init; }
}