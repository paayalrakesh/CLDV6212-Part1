using ABC_Retailers.Models;
using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

public class AzureFileShareService
{
    private readonly ShareClient _shareClient;
    private readonly ShareDirectoryClient _directoryClient;

    public AzureFileShareService(string connectionString, string shareName, string directoryName)
    {
        _shareClient = new ShareClient(connectionString, shareName);
        _directoryClient = _shareClient.GetDirectoryClient(directoryName);

        // Ensure the share exists
        _shareClient.CreateIfNotExists();

        // Ensure the directory exists
        _directoryClient.CreateIfNotExists();
    }

    public async Task<List<FileModel>> ListFilesAsync(string directoryName)
    {
        var directoryClient = _shareClient.GetDirectoryClient(directoryName);

        // Ensure the directory exists before listing files
        if (!await directoryClient.ExistsAsync())
        {
            await directoryClient.CreateAsync();
        }

        List<FileModel> files = new List<FileModel>();

        await foreach (ShareFileItem fileItem in directoryClient.GetFilesAndDirectoriesAsync())
        {
            if (fileItem.IsDirectory) continue;

            var fileClient = directoryClient.GetFileClient(fileItem.Name);
            var properties = await fileClient.GetPropertiesAsync();

            files.Add(new FileModel
            {
                Name = fileItem.Name,
                Size = properties.Value.ContentLength, // Ensure the correct property is used
                LastModified = properties.Value.LastModified // Correct the use of LastModified
            });
        }

        return files;
    }

    public async Task UploadFileAsync(string directoryName, string fileName, Stream fileStream)
    {
        var directoryClient = _shareClient.GetDirectoryClient(directoryName);

        // Ensure the directory exists before uploading
        if (!await directoryClient.ExistsAsync())
        {
            await directoryClient.CreateAsync();
        }

        var fileClient = directoryClient.GetFileClient(fileName);

        await fileClient.CreateAsync(fileStream.Length);
        await fileClient.UploadAsync(fileStream);
    }

    public async Task<Stream> DownloadFileAsync(string directoryName, string fileName)
    {
        var directoryClient = _shareClient.GetDirectoryClient(directoryName);

        // Ensure the directory exists before downloading
        if (!await directoryClient.ExistsAsync())
        {
            throw new DirectoryNotFoundException($"Directory '{directoryName}' does not exist.");
        }

        var fileClient = directoryClient.GetFileClient(fileName);

        var downloadInfo = await fileClient.DownloadAsync();
        return downloadInfo.Value.Content;
    }
}
