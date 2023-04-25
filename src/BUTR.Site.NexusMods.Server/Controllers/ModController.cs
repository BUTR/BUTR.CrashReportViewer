﻿using BUTR.Authentication.NexusMods.Authentication;
using BUTR.Site.NexusMods.Server.Contexts;
using BUTR.Site.NexusMods.Server.Extensions;
using BUTR.Site.NexusMods.Server.Models;
using BUTR.Site.NexusMods.Server.Models.API;
using BUTR.Site.NexusMods.Server.Models.Database;
using BUTR.Site.NexusMods.Server.Services;
using BUTR.Site.NexusMods.Shared.Helpers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BUTR.Site.NexusMods.Server.Controllers
{
    [ApiController, Route("api/v1/[controller]"), Authorize(AuthenticationSchemes = ButrNexusModsAuthSchemeConstants.AuthScheme)]
    public sealed class ModController : ControllerExtended
    {
        public sealed record ModQuery(int ModId);

        public sealed record ManualLinkQuery(string ModId, int NexusModsId);
        public sealed record ManualUnlinkQuery(string ModId);

        public sealed record AllowModQuery(int UserId, string ModId);
        public sealed record DisallowModQuery(int UserId, string ModId);


        private readonly ILogger _logger;
        private readonly NexusModsAPIClient _nexusModsAPIClient;
        private readonly AppDbContext _dbContext;
        private readonly NexusModsInfo _nexusModsInfo;

        public ModController(ILogger<ModController> logger, NexusModsAPIClient nexusModsAPIClient, AppDbContext dbContext, NexusModsInfo nexusModsInfo)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _nexusModsAPIClient = nexusModsAPIClient ?? throw new ArgumentNullException(nameof(nexusModsAPIClient));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _nexusModsInfo = nexusModsInfo ?? throw new ArgumentNullException(nameof(nexusModsInfo));
        }


        [HttpGet("Get/{gameDomain}/{modId:int}")]
        [Produces("application/json")]
        public async Task<ActionResult<APIResponse<ModModel?>>> Get(string gameDomain, int modId, CancellationToken ct)
        {
            var userId = HttpContext.GetUserId();
            var apiKey = HttpContext.GetAPIKey();

            if (await _nexusModsAPIClient.GetModAsync(gameDomain, modId, apiKey) is not { } modInfo)
                return APIResponseError<ModModel>("Mod not found!");

            if (userId != modInfo.User.Id)
                return APIResponseError<ModModel>("User does not have access to the mod!");

            /*
            var allowedUserIds = await _dbContext.Set<NexusModsModEntity>()
                .Where(x => x.NexusModsModId == modInfo.Id)
                .SelectMany(x => _dbContext.Set<UserAllowedModsEntity>(), (x, y) => new {x, y})
                .Join(_dbContext.Set<ModNexusModsManualLinkEntity>(), x => x.x.NexusModsModId, z => z.NexusModsId, (x, z) => new {x, z})
                .Where(x => x.x.x.UserIds.Contains(userId))
                .Where(x => x.x.y.AllowedModIds.Contains(x.z.ModId))
                .Select(x => x.x.y.UserId)
                .ToImmutableArrayAsync(ct);
            */

            return APIResponse(new ModModel(modInfo.Name, modInfo.Id, ImmutableArray<int>.Empty));
        }

        [HttpPost("Paginated")]
        [Produces("application/json")]
        public async Task<ActionResult<APIResponse<PagingData<ModModel>?>>> Paginated([FromBody] PaginatedQuery query, CancellationToken ct)
        {
            var userId = HttpContext.GetUserId();

            var paginated = await _dbContext.Set<NexusModsModEntity>()
                .Where(y => y.UserIds.Contains(userId))
                .PaginatedAsync(query, 20, new() { Property = nameof(NexusModsModEntity.NexusModsModId), Type = SortingType.Ascending }, ct);

            /*
            var allowedUserIds = await _dbContext.Set<NexusModsModEntity>()
                .SelectMany(x => _dbContext.Set<UserAllowedModsEntity>(), (x, y) => new {x, y})
                .Join(_dbContext.Set<ModNexusModsManualLinkEntity>(), x => x.x.NexusModsModId, z => z.NexusModsId, (x, z) => new {x, z})
                .Where(x => x.x.x.UserIds.Contains(userId))
                .Where(x => x.x.y.AllowedModIds.Contains(x.z.ModId))
                .Select(x => x.x.y.UserId)
                .ToImmutableArrayAsync(ct);
            */

            return APIResponse(new PagingData<ModModel>
            {
                Items = paginated.Items.Select(m => new ModModel(m.Name, m.NexusModsModId, ImmutableArray<int>.Empty)).ToAsyncEnumerable(),
                Metadata = paginated.Metadata
            });
        }

        [HttpPost("Update")]
        [Produces("application/json")]
        public async Task<ActionResult<APIResponse<string?>>> Update([FromBody] ModQuery query)
        {
            var userId = HttpContext.GetUserId();
            var apiKey = HttpContext.GetAPIKey();

            if (await _nexusModsAPIClient.GetModAsync("mountandblade2bannerlord", query.ModId, apiKey) is not { } modInfo)
                return APIResponseError<string>("Mod not found!");

            if (userId != modInfo.User.Id)
                return APIResponseError<string>("User does not have access to the mod!");

            NexusModsModEntity? ApplyChanges(NexusModsModEntity? existing) => existing switch
            {
                null => null,
                _ => existing with { Name = modInfo.Name }
            };
            if (await _dbContext.AddUpdateRemoveAndSaveAsync<NexusModsModEntity>(x => x.NexusModsModId == query.ModId, ApplyChanges))
                return APIResponse("Updated successful!");

            return APIResponseError<string>("Failed to link the mod!");
        }


        [HttpGet("Link")]
        [Produces("application/json")]
        public async Task<ActionResult<APIResponse<string?>>> Link([FromQuery] ModQuery query)
        {
            var userId = HttpContext.GetUserId();
            var apiKey = HttpContext.GetAPIKey();

            if (await _nexusModsAPIClient.GetModAsync("mountandblade2bannerlord", query.ModId, apiKey) is not { } modInfo)
                return APIResponseError<string>("Mod not found!");

            if (userId != modInfo.User.Id)
                return APIResponseError<string>("User does not have access to the mod!");

            if (HttpContext.GetIsPremium())
            {
                var exposedModIds = await _nexusModsInfo.GetModIdsAsync("mountandblade2bannerlord", modInfo.Id, apiKey).Distinct().ToImmutableArrayAsync();
                NexusModsExposedModsEntity? ApplyChanges2(NexusModsExposedModsEntity? existing) => existing switch
                {
                    null => new() { NexusModsModId = modInfo.Id, ModIds = exposedModIds.AsArray(), LastCheckedDate = DateTime.UtcNow },
                    _ => existing with { ModIds = existing.ModIds.AsImmutableArray().AddRange(exposedModIds.Except(existing.ModIds)).AsArray(), LastCheckedDate = DateTime.UtcNow }
                };
                if (!await _dbContext.AddUpdateRemoveAndSaveAsync<NexusModsExposedModsEntity>(x => x.NexusModsModId == query.ModId, ApplyChanges2))
                    return APIResponseError<string>("Failed to link!");
            }

            NexusModsModEntity? ApplyChanges(NexusModsModEntity? existing) => existing switch
            {
                null => new() { Name = modInfo.Name, NexusModsModId = modInfo.Id, UserIds = ImmutableArray.Create<int>(userId).AsArray() },
                _ when existing.UserIds.Contains(userId) => existing,
                _ => existing with { UserIds = ImmutableArray.Create<int>(userId).AsArray() }
            };
            if (await _dbContext.AddUpdateRemoveAndSaveAsync<NexusModsModEntity>(x => x.NexusModsModId == query.ModId, ApplyChanges))
                return APIResponse("Linked successful!");

            return APIResponseError<string>("Failed to link!");
        }

        [HttpGet("Unlink")]
        [Produces("application/json")]
        public async Task<ActionResult<APIResponse<string?>>> Unlink([FromQuery] ModQuery query)
        {
            var userId = HttpContext.GetUserId();

            NexusModsModEntity? ApplyChanges(NexusModsModEntity? existing) => existing switch
            {
                null => null,
                _ when existing.UserIds.Contains(userId) && existing.UserIds.Length == 1 => null,
                _ when !existing.UserIds.Contains(userId) => existing,
                _ => existing with { UserIds = existing.UserIds.AsImmutableArray().Remove(userId).AsArray() }
            };
            if (await _dbContext.AddUpdateRemoveAndSaveAsync<NexusModsModEntity>(x => x.NexusModsModId == query.ModId, ApplyChanges))
                return APIResponse("Unlinked successful!");

            return APIResponseError<string>("Failed to unlink!");
        }


        [HttpGet("ManualLink")]
        [Authorize(Roles = $"{ApplicationRoles.Administrator},{ApplicationRoles.Moderator}")]
        [Produces("application/json")]
        public async Task<ActionResult<APIResponse<string?>>> ManualLink([FromQuery] ManualLinkQuery query)
        {
            ModNexusModsManualLinkEntity? ApplyChanges(ModNexusModsManualLinkEntity? existing) => existing switch
            {
                null => new() { ModId = query.ModId, NexusModsId = query.NexusModsId },
                _ => existing with { NexusModsId = query.NexusModsId }
            };
            if (await _dbContext.AddUpdateRemoveAndSaveAsync<ModNexusModsManualLinkEntity>(x => x.ModId == query.ModId, ApplyChanges))
                return APIResponse("Linked successful!");

            return APIResponseError<string>("Failed to link!");
        }

        [HttpGet("ManualUnlink")]
        [Authorize(Roles = $"{ApplicationRoles.Administrator},{ApplicationRoles.Moderator}")]
        [Produces("application/json")]
        public async Task<ActionResult<APIResponse<string?>>> ManualUnlink([FromQuery] ManualUnlinkQuery query)
        {
            ModNexusModsManualLinkEntity? ApplyChanges(ModNexusModsManualLinkEntity? existing) => existing switch
            {
                _ => null
            };
            if (await _dbContext.AddUpdateRemoveAndSaveAsync<ModNexusModsManualLinkEntity>(x => x.ModId == query.ModId, ApplyChanges))
                return APIResponse("Unlinked successful!");

            return APIResponseError<string>("Failed to unlink!");
        }

        [HttpPost("ManualLinkPaginated")]
        [Produces("application/json")]
        public async Task<ActionResult<APIResponse<PagingData<ModNexusModsManualLinkModel>?>>> ManualLinkPaginated([FromBody] PaginatedQuery query, CancellationToken ct)
        {
            var paginated = await _dbContext.Set<ModNexusModsManualLinkEntity>()
                .PaginatedAsync(query, 20, new() { Property = nameof(ModNexusModsManualLinkEntity.ModId), Type = SortingType.Ascending }, ct);

            return APIResponse(new PagingData<ModNexusModsManualLinkModel>
            {
                Items = paginated.Items.Select(m => new ModNexusModsManualLinkModel(m.ModId, m.NexusModsId)).ToAsyncEnumerable(),
                Metadata = paginated.Metadata
            });
        }


        [HttpGet("AllowMod")]
        [Authorize(Roles = $"{ApplicationRoles.Administrator},{ApplicationRoles.Moderator}")]
        [Produces("application/json")]
        public async Task<ActionResult<APIResponse<string?>>> AllowMod([FromQuery] AllowModQuery query)
        {
            UserAllowedModsEntity? ApplyChanges(UserAllowedModsEntity? existing) => existing switch
            {
                null => new() { UserId = query.UserId, AllowedModIds = ImmutableArray.Create<string>(query.ModId).AsArray() },
                _ when existing.AllowedModIds.Contains(query.ModId) => existing,
                _ => existing with { AllowedModIds = existing.AllowedModIds.AsImmutableArray().Add(query.ModId).AsArray() }
            };
            if (await _dbContext.AddUpdateRemoveAndSaveAsync<UserAllowedModsEntity>(x => x.UserId == query.UserId, ApplyChanges))
                return APIResponse("Allowed successful!");

            return APIResponseError<string>("Failed to allowed!");
        }

        [HttpGet("DisallowMod")]
        [Authorize(Roles = $"{ApplicationRoles.Administrator},{ApplicationRoles.Moderator}")]
        [Produces("application/json")]
        public async Task<ActionResult<APIResponse<string?>>> DisallowMod([FromQuery] DisallowModQuery query)
        {
            UserAllowedModsEntity? ApplyChanges(UserAllowedModsEntity? existing) => existing switch
            {
                null => null,
                _ when existing.AllowedModIds.Contains(query.ModId) && existing.AllowedModIds.Length == 1 => null,
                _ when !existing.AllowedModIds.Contains(query.ModId) => existing,
                _ => existing with { AllowedModIds = existing.AllowedModIds.AsImmutableArray().Remove(query.ModId).AsArray() }
            };
            if (await _dbContext.AddUpdateRemoveAndSaveAsync<UserAllowedModsEntity>(x => x.UserId == query.UserId, ApplyChanges))
                return APIResponse("Disallowed successful!");

            return APIResponseError<string>("Failed to disallowed!");
        }

        [HttpPost("AllowModPaginated")]
        [Produces("application/json")]
        public async Task<ActionResult<APIResponse<PagingData<UserAllowedModsModel>?>>> AllowModPaginated([FromBody] PaginatedQuery query, CancellationToken ct)
        {
            var paginated = await _dbContext.Set<UserAllowedModsEntity>()
                .PaginatedAsync(query, 20, new() { Property = nameof(UserAllowedModsEntity.UserId), Type = SortingType.Ascending }, ct);

            return APIResponse(new PagingData<UserAllowedModsModel>
            {
                Items = paginated.Items.Select(m => new UserAllowedModsModel(m.UserId, m.AllowedModIds.AsImmutableArray())).ToAsyncEnumerable(),
                Metadata = paginated.Metadata
            });
        }
    }
}