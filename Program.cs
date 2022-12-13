using System.Collections.Generic;
using Pulumi;
using Aws = Pulumi.Aws;
using SyncedFolder = Pulumi.SyncedFolder;

return await Deployment.RunAsync(() =>
{
    // Import the program's configuration settings.
    var config = new Config();
    var path = config.Get("path") ?? "./www";
    var indexDocument = config.Get("indexDocument") ?? "index.html";
    var errorDocument = config.Get("errorDocument") ?? "error.html";

    // Create an S3 bucket and configure it as a website.
    var bucket = new Aws.S3.Bucket("bucket", new()
    {
        Acl = "public-read",
        Website = new Aws.S3.Inputs.BucketWebsiteArgs
        {
            IndexDocument = indexDocument,
            ErrorDocument = errorDocument,
        },
        Tags = new InputMap<string> {
            
        }
    });

    // Export the URLs and hostnames of the bucket and distribution.
    return new Dictionary<string, object?>
    {
        
    };
});
