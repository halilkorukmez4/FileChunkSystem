namespace FileChunkSystem.Application.Abstractions.Storages;

public interface IStorageProviderDispatcher
{
    IStorageProvider Use(string providerKey);
    IStorageProvider Use();
}