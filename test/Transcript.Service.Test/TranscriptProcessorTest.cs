namespace Transcript.Service.Test
{
    using Microsoft.Extensions.Configuration;
    using Moq;
    using Transcript.Service.Helpers;
    using Transcript.Service.Processors;
    using Transcript.Service.Services;

    public class TranscriptProcessorTest
    {
        private readonly Mock<IConfiguration> configuration = new();
        private readonly Mock<ConfigurationHelper> configurationHelper;
        private readonly Mock<FileHelper> FileHelper = new();
        private readonly Mock<InvoxService> InvoxService;

        private TranscriptProcessor transcriptProcessor;

        public TranscriptProcessorTest()
        {
            configurationHelper = new Mock<ConfigurationHelper>(configuration.Object);
            InvoxService = new Mock<InvoxService>(FileHelper.Object);
            transcriptProcessor = new TranscriptProcessor(configurationHelper.Object, InvoxService.Object, FileHelper.Object);
        }

        [Fact]
        public async Task ProcessVoiceFiles_WithoutFiles_ExecutesSucessfully()
        {
            FileHelper.Setup(x => x.GetFilesAtDirectory(It.IsAny<string>(), It.IsAny<string>())).Returns(() => []);

            await transcriptProcessor.ProcessVoiceFiles("");
        }

        [Fact]
        public async Task ProcessVoiceFiles_WhenGetFilesToProcessFails_NoCallToInternalProcessVoiceFilesAsyncMethod()
        {
            FileHelper.Setup(x => x.GetFilesAtDirectory(It.IsAny<string>(), It.IsAny<string>())).Throws(() => new Exception("Some exception"));

            await Assert.ThrowsAsync<Exception>(() => transcriptProcessor.ProcessVoiceFiles(""));
        }

        [Fact]
        public async Task ProcessAudioFile_WhenGetContentFromInvoxService_StoreNewFile()
        {
            configurationHelper.Setup(x => x.GetConfigValueWithDefault(It.IsAny<string>(), It.IsAny<int>())).Returns(3);
            FileHelper.Setup(x => x.GetFilesAtDirectory(It.IsAny<string>(), It.IsAny<string>())).Returns(() => ["file1.txt"]);
            FileHelper.Setup(x => x.Save(It.IsAny<string>(), It.IsAny<byte[]>()));
            FileHelper.Setup(x => x.GetFileSize(It.IsAny<string>())).Returns(100000);
            InvoxService.Setup(x => x.TranscriptVoiceFile(It.IsAny<string>())).Returns(Task.FromResult(new byte[1]));

            await transcriptProcessor.ProcessVoiceFiles("");
            FileHelper.Verify(x => x.Save(It.IsAny<string>(), It.IsAny<byte[]>()), Times.Once);
        }

        [Fact]
        public async Task ProcessAudioFile_WhenGetExceptionFromInvoxService_RetriesUpToThree()
        {

            configurationHelper.Setup(x => x.GetConfigValueWithDefault(It.IsAny<string>(), It.IsAny<int>())).Returns(3);
            FileHelper.Setup(x => x.GetFilesAtDirectory(It.IsAny<string>(), It.IsAny<string>())).Returns(() => ["file1.txt"]);
            FileHelper.Setup(x => x.Save(It.IsAny<string>(), It.IsAny<byte[]>()));
            FileHelper.Setup(x => x.GetFileSize(It.IsAny<string>())).Returns(100000);
            InvoxService.Setup(x => x.TranscriptVoiceFile(It.IsAny<string>())).Throws(() => new Exception("Some exception"));

            await transcriptProcessor.ProcessVoiceFiles("");
            configurationHelper.Verify(x => x.GetConfigValueWithDefault("TranscriptRetries", It.IsAny<int>()), Times.Exactly(3));
        }

        [Fact]
        public async Task ProcessAudioFile_WhenFileSizeIsWrong_TheProcessDontSendIt()
        {

            configurationHelper.Setup(x => x.GetConfigValueWithDefault(It.IsAny<string>(), It.IsAny<int>())).Returns(3);
            FileHelper.Setup(x => x.GetFilesAtDirectory(It.IsAny<string>(), It.IsAny<string>())).Returns(() => ["file1.mp3"]);
            FileHelper.Setup(x => x.GetFileSize(It.IsAny<string>())).Returns(10);
            InvoxService.Setup(x => x.TranscriptVoiceFile(It.IsAny<string>())).Returns(Task.FromResult(new byte[1]));

            await transcriptProcessor.ProcessVoiceFiles("");
            InvoxService.Verify(x => x.TranscriptVoiceFile(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task ProcessAudioFile_WhenFileSizeIsCorrect_TheProcessSendIt()
        {

            configurationHelper.Setup(x => x.GetConfigValueWithDefault(It.IsAny<string>(), It.IsAny<int>())).Returns(3);
            FileHelper.Setup(x => x.GetFilesAtDirectory(It.IsAny<string>(), It.IsAny<string>())).Returns(() => ["file1.mp3"]);
            FileHelper.Setup(x => x.GetFileSize(It.IsAny<string>())).Returns(100000);
            InvoxService.Setup(x => x.TranscriptVoiceFile(It.IsAny<string>())).Returns(Task.FromResult(new byte[1]));

            await transcriptProcessor.ProcessVoiceFiles("");
            InvoxService.Verify(x => x.TranscriptVoiceFile(It.IsAny<string>()), Times.Once);
        }
    }
}