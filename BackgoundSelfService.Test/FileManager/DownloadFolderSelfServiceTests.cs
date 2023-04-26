using FluentAssertions;
using Cyphercrescent.SelfService.BuildBackgroundService.FileManager;

public class DownloadFolderSelfServiceTests
{
    private readonly DownloadFileManager _sut;

    public DownloadFolderSelfServiceTests()
    {
        _sut = new DownloadFileManager();
    }

    [Fact]
    public void IsFileBusy_ShouldReturnTrue_WhenFileIsInUse()
    {
        // Arrange
        string fileFullPath = Path.GetTempFileName();
        using (FileStream stream = new FileStream(fileFullPath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
        {
            // Act
            bool result = _sut.IsFileBusy(fileFullPath);

            // Assert
            result.Should().BeTrue();
        }
    }

    [Fact]
    public void IsFileBusy_ShouldReturnFalse_WhenFileIsNotInUse()
    {
        // Arrange
        string fileFullPath = Path.GetTempFileName();

        // Act
        bool result = _sut.IsFileBusy(fileFullPath);

        // Assert
        result.Should().BeFalse();
    }
}

