namespace Transcript.Service.Test
{
    using Microsoft.Extensions.Configuration;
    using Moq;
    using Transcript.Service.Helpers;
    using Transcript.Service.Processors;
    using Transcript.Service.Services;

    public class InvoxServiceTest
    {
        private readonly Mock<IConfiguration> configuration = new();
        
        private readonly Mock<ConfigurationHelper> configurationHelper;
        private InvoxService InvoxService;
        private readonly Mock<FileHelper> FileHelper = new();

        public InvoxServiceTest()
        {
            configurationHelper = new Mock<ConfigurationHelper>(configuration.Object);
            FileHelper.Setup(x => x.GetFilesAtDirectory(It.IsAny<string>(), It.IsAny<string>())).Returns(() => ["file1.txt", "file2.txt"]);

            InvoxService = new InvoxService(FileHelper.Object);
        }

        [Fact]
        public async Task TranscriptVoiceFile_SendLessQuantityThanFivePercent_ExecutesSucessfully()
        {
            for (int i = 0; i < 10; i++)
            {
                await InvoxService.TranscriptVoiceFile("");
            }
            Assert.Equal(10, InvoxService.SuccessRequests);
        }

        [Fact]
        public async Task TranscriptVoiceFile_SendMoreQuantityThanFivePercent_ReturnsError()
        {
            for (int i = 0; i < 20; i++)
            {
                await InvoxService.TranscriptVoiceFile("");
            }
                ;
            await Assert.ThrowsAsync<Exception>(() => InvoxService.TranscriptVoiceFile(""));
        }
    }
}