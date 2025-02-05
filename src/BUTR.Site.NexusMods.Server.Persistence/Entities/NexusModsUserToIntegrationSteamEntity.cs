using System;
using System.Collections.Generic;

namespace BUTR.Site.NexusMods.Server.Models.Database;

public sealed record NexusModsUserToIntegrationSteamEntity : IEntity
{
    public required NexusModsUserId NexusModsUserId { get; init; }
    public required NexusModsUserEntity NexusModsUser { get; init; }
    public IntegrationSteamTokensEntity? ToTokens { get; init; }

    public required SteamUserId SteamUserId { get; init; }
    public ICollection<IntegrationSteamToOwnedTenantEntity> ToOwnedTenants { get; init; } = new List<IntegrationSteamToOwnedTenantEntity>();


    public override int GetHashCode() => HashCode.Combine(NexusModsUserId, SteamUserId);
}