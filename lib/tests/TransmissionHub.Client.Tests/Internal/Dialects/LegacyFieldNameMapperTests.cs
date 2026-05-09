using TransmissionHub.Client.Internal.Dialects;

namespace TransmissionHub.Client.Tests.Internal.Dialects;

public class LegacyFieldNameMapperTests
{
    // -------------------------------------------------------------------------
    // camelCase fallback — lowerFirst(pascalCase)
    // -------------------------------------------------------------------------

    [Test]
    public async Task ToLegacyWireName_RegularField_ReturnsCamelCase()
    {
        await Assert.That(LegacyFieldNameMapper.ToLegacyWireName("ActivityDate")).IsEqualTo("activityDate");
        await Assert.That(LegacyFieldNameMapper.ToLegacyWireName("SeedRatioLimit")).IsEqualTo("seedRatioLimit");
        await Assert.That(LegacyFieldNameMapper.ToLegacyWireName("HashString")).IsEqualTo("hashString");
    }

    // -------------------------------------------------------------------------
    // kebab-case overrides — representative cases from each method group
    // -------------------------------------------------------------------------

    [Test]
    public async Task ToLegacyWireName_TorrentGetKebabException_ReturnsKebab()
    {
        // Fields that deviate from camelCase within torrent-get
        await Assert.That(LegacyFieldNameMapper.ToLegacyWireName("FileCount")).IsEqualTo("file-count");
        await Assert.That(LegacyFieldNameMapper.ToLegacyWireName("PeerLimit")).IsEqualTo("peer-limit");
        await Assert.That(LegacyFieldNameMapper.ToLegacyWireName("PrimaryMimeType")).IsEqualTo("primary-mime-type");
    }

    [Test]
    public async Task ToLegacyWireName_SessionField_ReturnsKebab()
    {
        // Session fields are almost entirely kebab-case
        await Assert.That(LegacyFieldNameMapper.ToLegacyWireName("PeerPort")).IsEqualTo("peer-port");
        await Assert.That(LegacyFieldNameMapper.ToLegacyWireName("DhtEnabled")).IsEqualTo("dht-enabled");
        await Assert.That(LegacyFieldNameMapper.ToLegacyWireName("SpeedLimitDown")).IsEqualTo("speed-limit-down");
    }

    [Test]
    public async Task ToLegacyWireName_SessionStatsNestedKeys_ReturnKebab()
    {
        // These are top-level keys in session-stats response
        await Assert.That(LegacyFieldNameMapper.ToLegacyWireName("CumulativeStats")).IsEqualTo("cumulative-stats");
        await Assert.That(LegacyFieldNameMapper.ToLegacyWireName("CurrentStats")).IsEqualTo("current-stats");
    }

    [Test]
    public async Task ToLegacyWireName_FreeSpaceField_ReturnsKebab()
    {
        await Assert.That(LegacyFieldNameMapper.ToLegacyWireName("SizeBytes")).IsEqualTo("size-bytes");
    }
}