using System.IO;
using Transcript.Service.Interfaces;

namespace Transcript.Service.Helpers
{
    public class FileHelper: IFileHelper
    {
        /// <inheritdoc />
        public virtual string[] GetFilesAtDirectory(string path, string searchCriteria)
        {
            try
            {
                string[] files = Directory.GetFiles(path, searchCriteria);
                Console.WriteLine($"There are {files.Length} files with this search criteria {searchCriteria} at path {path}.");
                return files;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error trying to read fields ot path {path}. SearchCriteria: {searchCriteria}. Error details: {e.Message}");
                throw;
            }
        }

        /// <inheritdoc />
        public virtual async Task Save(string newFileName, byte[] fileContent)
        {
            await File.WriteAllBytesAsync(newFileName, fileContent);
        }

        /// <inheritdoc />
        public virtual byte[] GetFileContent(string fileName)
        {
            return File.ReadAllBytes(fileName);
        }
    }
}
