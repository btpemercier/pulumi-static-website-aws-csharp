using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Pulumi.Aws.S3;
using Xunit;

public class WebserverStackTests
{
    [Theory]
    [InlineData("user:Project")]
    [InlineData("user:Stack")]
    public async Task WebsiteHasTag(string tag)
    {
        var resources = await Testing.RunAsync<WebsiteStack>();

        var bucket = resources.OfType<Bucket>().FirstOrDefault();
        bucket.Should().NotBeNull("Bucket not found");

        var tags = await bucket.Tags.GetValueAsync();
        tags.Should().NotBeNull("Tags are not defined");
        tags.Should().ContainKey(tag);
    }
}