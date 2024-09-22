using BUTR.Site.NexusMods.Server.Models.Database;

namespace BUTR.Site.NexusMods.Server.Repositories;

public interface IIntegrationGOGToOwnedTenantEntityRepositoryRead : IRepositoryRead<IntegrationGOGToOwnedTenantEntity>;
public interface IIntegrationGOGToOwnedTenantEntityRepositoryWrite : IRepositoryWrite<IntegrationGOGToOwnedTenantEntity>, IIntegrationGOGToOwnedTenantEntityRepositoryRead;