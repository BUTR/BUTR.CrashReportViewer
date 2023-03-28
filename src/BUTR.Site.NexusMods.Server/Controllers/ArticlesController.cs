﻿using BUTR.Authentication.NexusMods.Authentication;
using BUTR.Site.NexusMods.Server.Contexts;
using BUTR.Site.NexusMods.Server.Extensions;
using BUTR.Site.NexusMods.Server.Models;
using BUTR.Site.NexusMods.Server.Models.API;
using BUTR.Site.NexusMods.Server.Models.Database;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;

using Npgsql;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BUTR.Site.NexusMods.Server.Controllers
{
    [ApiController, Route("api/v1/[controller]"), Authorize(AuthenticationSchemes = ButrNexusModsAuthSchemeConstants.AuthScheme)]
    public class ArticlesController : ControllerBase
    {
        public sealed record ArticlesQuery(uint Page, uint PageSize, List<Filtering>? Filters, List<Sorting>? Sotings);

        private readonly ILogger _logger;
        private readonly AppDbContext _dbContext;

        public ArticlesController(ILogger<ArticlesController> logger, AppDbContext dbContext)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        [HttpPost("Paginated")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PagingResponse<ArticleModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Paginated([FromBody] ArticlesQuery query, CancellationToken ct)
        {
            var page = query.Page;
            var pageSize = Math.Max(Math.Min(query.PageSize, 100), 5);
            var filters = query.Filters ?? Enumerable.Empty<Filtering>();
            var sortings = query.Sotings is null || query.Sotings.Count == 0
                ? new List<Sorting> { new() { Property = nameof(NexusModsArticleEntity.ArticleId), Type = SortingType.Ascending } }
                : query.Sotings;

            var paginated = await _dbContext.Set<NexusModsArticleEntity>()
                .WithFilter(filters)
                .WithSort(sortings)
                .PaginatedAsync(page, pageSize, ct);

            return StatusCode(StatusCodes.Status200OK, new PagingResponse<ArticleModel>
            {
                Items = paginated.Items.Select(x => new ArticleModel(x.ArticleId, x.Title, x.AuthorId, x.AuthorName, x.CreateDate)).ToAsyncEnumerable(),
                Metadata = paginated.Metadata
            });
        }

        [HttpGet("Autocomplete")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string[]), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Autocomplete([FromQuery] string authorName)
        {
            var mapping = _dbContext.Model.FindEntityType(typeof(NexusModsArticleEntity));
            if (mapping?.GetTableName() is not { } table)
                return StatusCode(StatusCodes.Status200OK, Array.Empty<string>());

            var schema = mapping.GetSchema();
            var tableName = mapping.GetSchemaQualifiedTableName();
            var storeObjectIdentifier = StoreObjectIdentifier.Table(table, schema);
            var authorNameName = mapping.GetProperty(nameof(NexusModsArticleEntity.AuthorName)).GetColumnName(storeObjectIdentifier);

            var valPram = new NpgsqlParameter<string>("val", authorName);
            var values = await _dbContext.Set<NexusModsArticleEntity>()
                .FromSqlRaw(@$"
SELECT DISTINCT
     {authorNameName}
FROM
    {tableName}
WHERE
    {authorNameName} ILIKE @val || '%'
ORDER BY
    {authorNameName}", valPram)
                .AsNoTracking()
                .Select(x => x.AuthorName)
                .ToArrayAsync();

            return StatusCode(StatusCodes.Status200OK, values);
        }
    }
}