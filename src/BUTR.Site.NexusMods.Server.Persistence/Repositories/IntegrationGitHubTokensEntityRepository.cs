using BUTR.Site.NexusMods.Server.Models.Database;

namespace BUTR.Site.NexusMods.Server.Repositories;

public interface IIntegrationGitHubTokensEntityRepositoryRead : IRepositoryRead<IntegrationGitHubTokensEntity>;
public interface IIntegrationGitHubTokensEntityRepositoryWrite : IRepositoryWrite<IntegrationGitHubTokensEntity>, IIntegrationGitHubTokensEntityRepositoryRead;