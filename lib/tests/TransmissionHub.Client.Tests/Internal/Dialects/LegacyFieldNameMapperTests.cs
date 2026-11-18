using TransmissionHub.Client.Internal.Dialects;

namespace TransmissionHub.Client.Tests.Internal.Dialects;

public class LegacyFieldNameMapperTests
{
    // -------------------------------------------------------------------------
    // ToTorrentGetFieldName — torrent-get field table (camelCase + exceptions)
    // -------------------------------------------------------------------------

    [Test]
    public async Task ToTorrentGetFieldName_RegularField_ReturnsCamelCase()
    {
        await Assert.That(LegacyFieldNameMapper.ToTorrentGetFieldName("ActivityDate")).IsEqualTo("activityDate");
        await Assert.That(LegacyFieldNameMapper.ToTorrentGetFieldName("SeedRatioLimit")).IsEqualTo("seedRatioLimit");
        await Assert.That(LegacyFieldNameMapper.ToTorrentGetFieldName("HashString")).IsEqualTo("hashString");
    }

    [Test]
    public async Task ToTorrentGetFieldName_KebabException_ReturnsKebab()
    {
        // The only kebab-case keys of the torrent-get field table
        await Assert.That(LegacyFieldNameMapper.ToTorrentGetFieldName("FileCount")).IsEqualTo("file-count");
        await Assert.That(LegacyFieldNameMapper.ToTorrentGetFieldName("PeerLimit")).IsEqualTo("peer-limit");
        await Assert.That(LegacyFieldNameMapper.ToTorrentGetFieldName("PrimaryMimeType")).IsEqualTo("primary-mime-type");
    }

    [Test]
    public async Task ToTorrentGetFieldName_DownloadDir_ReturnsCamelCase()
    {
        // torrent-get is the only scope where the download directory is camelCase;
        // torrent-add arguments and session fields use "download-dir".
        await Assert.That(LegacyFieldNameMapper.ToTorrentGetFieldName("DownloadDir")).IsEqualTo("downloadDir");
    }

    // -------------------------------------------------------------------------
    // ToSessionFieldName — session-get fields / session-set arguments
    // -------------------------------------------------------------------------

    [Test]
    public async Task ToSessionFieldName_KebabField_ReturnsKebab()
    {
        // Session fields are almost entirely kebab-case
        await Assert.That(LegacyFieldNameMapper.ToSessionFieldName("PeerPort")).IsEqualTo("peer-port");
        await Assert.That(LegacyFieldNameMapper.ToSessionFieldName("DhtEnabled")).IsEqualTo("dht-enabled");
        await Assert.That(LegacyFieldNameMapper.ToSessionFieldName("SpeedLimitDown")).IsEqualTo("speed-limit-down");
    }

    [Test]
    public async Task ToSessionFieldName_DownloadDir_ReturnsKebab()
    {
        // The same PascalCase name maps to kebab-case in the session scope
        await Assert.That(LegacyFieldNameMapper.ToSessionFieldName("DownloadDir")).IsEqualTo("download-dir");
    }

    [Test]
    public async Task ToSessionFieldName_RegularField_ReturnsCamelCase()
    {
        // encryption, seedRatioLimit, seedRatioLimited are camelCase in the session table
        await Assert.That(LegacyFieldNameMapper.ToSessionFieldName("SeedRatioLimit")).IsEqualTo("seedRatioLimit");
        await Assert.That(LegacyFieldNameMapper.ToSessionFieldName("Encryption")).IsEqualTo("encryption");
    }

    // -------------------------------------------------------------------------
    // ToArgumentName — request argument bodies (torrent-add, torrent-set, ...)
    // -------------------------------------------------------------------------

    [Test]
    public async Task ToArgumentName_TorrentAddFields_ReturnKebab()
    {
        await Assert.That(LegacyFieldNameMapper.ToArgumentName("DownloadDir")).IsEqualTo("download-dir");
        await Assert.That(LegacyFieldNameMapper.ToArgumentName("PeerLimit")).IsEqualTo("peer-limit");
        await Assert.That(LegacyFieldNameMapper.ToArgumentName("FilesWanted")).IsEqualTo("files-wanted");
        await Assert.That(LegacyFieldNameMapper.ToArgumentName("FilesUnwanted")).IsEqualTo("files-unwanted");
        await Assert.That(LegacyFieldNameMapper.ToArgumentName("PriorityHigh")).IsEqualTo("priority-high");
        await Assert.That(LegacyFieldNameMapper.ToArgumentName("PriorityLow")).IsEqualTo("priority-low");
        await Assert.That(LegacyFieldNameMapper.ToArgumentName("PriorityNormal")).IsEqualTo("priority-normal");
    }

    [Test]
    public async Task ToArgumentName_TorrentRemoveField_ReturnsKebab()
    {
        // Critical: without this override the server silently ignores the flag
        // and keeps the local data
        await Assert.That(LegacyFieldNameMapper.ToArgumentName("DeleteLocalData")).IsEqualTo("delete-local-data");
    }

    [Test]
    public async Task ToArgumentName_SessionSetField_FallsBackToSessionOverrides()
    {
        // session-set arguments share the session field wire names
        await Assert.That(LegacyFieldNameMapper.ToArgumentName("AltSpeedDown")).IsEqualTo("alt-speed-down");
        await Assert.That(LegacyFieldNameMapper.ToArgumentName("PeerPort")).IsEqualTo("peer-port");
    }

    [Test]
    public async Task ToArgumentName_RegularField_ReturnsCamelCase()
    {
        await Assert.That(LegacyFieldNameMapper.ToArgumentName("Ids")).IsEqualTo("ids");
        await Assert.That(LegacyFieldNameMapper.ToArgumentName("Location")).IsEqualTo("location");
        await Assert.That(LegacyFieldNameMapper.ToArgumentName("Metainfo")).IsEqualTo("metainfo");
    }
}