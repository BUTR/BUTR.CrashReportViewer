using BUTR.Site.NexusMods.Server.Models.Database;

namespace BUTR.Site.NexusMods.Server.Repositories;

public interface INexusModsModToModuleInfoHistoryEntityRepositoryRead : IRepositoryRead<NexusModsModToModuleInfoHistoryEntity>;
public interface INexusModsModToModuleInfoHistoryEntityRepositoryWrite : IRepositoryWrite<NexusModsModToModuleInfoHistoryEntity>, INexusModsModToModuleInfoHistoryEntityRepositoryRead;