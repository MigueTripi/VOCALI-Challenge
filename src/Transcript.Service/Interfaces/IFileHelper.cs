namespace Transcript.Service.Interfaces
{
    public interface IFileHelper
    {
        /// <summary>
        /// Retrieve the files located in the given directory
        /// </summary>
        /// <param name="path"> Directory where to find the files</param>
        /// <param name="searchCriteria">filter the files in the directory (mp3 in our case)</param>
        /// <returns>file names</returns>
        string[] GetFilesAtDirectory(string path, string searchCriteria);

        /// <summary>
        /// Save the content in the specified file
        /// </summary>
        /// <param name="newFileName">file name</param>
        /// <param name="fileContent">file content</param>
        Task Save(string newFileName, byte[] fileContent);

        /// <summary>
        /// Read and retrieve the file content as array of bytes
        /// </summary>
        /// <param name="fileName">file to read</param>
        /// <returns>file content</returns>
        byte[] GetFileContent(string fileName);

    }
}
