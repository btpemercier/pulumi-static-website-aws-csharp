using System.IO;
using Pulumi;
using Aws = Pulumi.Aws;

class WebsiteStack : Stack
{
    [Output] public Output<string> BucketName { get; set; }
    [Output] public Output<string> BucketEndpoint { get; set; }
    [Output] public Output<string> Readme { get; set; }

    public WebsiteStack()
    {

        var config = new Config();

        var bucket = new Aws.S3.Bucket("bucket", new()
        {
            Acl = "public-read",
            Website = new Aws.S3.Inputs.BucketWebsiteArgs
            {
                IndexDocument = "index.html",
                ErrorDocument = "error.html",
            },
            Tags = new InputMap<string> {
                {"user:Project", "test"},            
                {"user:Stack", "test"}
            }
        });
        

        var files = Directory.GetFiles("www");

        foreach (var file in files)
        {
            var name = Path.GetFileName(file);
            var bucketObject = new Aws.S3.BucketObject(name, new Aws.S3.BucketObjectArgs
            {
                Acl = "public-read",
                Bucket = bucket.BucketName,
                ContentType = "text/html",
                Source = new FileAsset(file)
            }, new CustomResourceOptions {Parent = bucket});
        }

        BucketName = bucket.BucketName;
        BucketEndpoint = Output.Format($"http://{bucket.WebsiteEndpoint}");
        Readme = Output.Create(File.ReadAllText("./Pulumi.README.md"));
    }
}