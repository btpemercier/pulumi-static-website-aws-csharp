using System.Collections.Immutable;
using System.Threading.Tasks;
using Pulumi;
using Pulumi.Testing;

class Mocks : IMocks
    {
        public Task<(string? id, object state)> NewResourceAsync(MockResourceArgs args)
        {
            var outputs = ImmutableDictionary.CreateBuilder<string, object>();

            // Forward all input parameters as resource outputs, so that we could test them.
            outputs.AddRange(args.Inputs);

            // Default the resource ID to `{name}_id`.
            // We could also format it as `/subscription/abc/resourceGroups/xyz/...` if that was important for tests.
            args.Id ??= $"{args.Name}_id";
            return Task.FromResult<(string? id, object state)>((args.Id, (object)outputs));
        }

        public Task<object> CallAsync(MockCallArgs args)
        {
            var outputs = ImmutableDictionary.CreateBuilder<string, object>();

            return Task.FromResult((object)outputs);
        }
    }

/// <summary>
/// Helper methods to streamlines unit testing experience.
/// </summary>
public static class Testing
{
    /// <summary>
    /// Run the tests for a given stack type.
    /// </summary>
    public static Task<ImmutableArray<Resource>> RunAsync<T>() where T : Stack, new()
    {
        return Deployment.TestAsync<T>(new Mocks(), new TestOptions { IsPreview = false });
    }

    /// <summary>
    /// Extract the value from an output.
    /// </summary>
    public static Task<T> GetValueAsync<T>(this Output<T> output)
    {
        var tcs = new TaskCompletionSource<T>();
        output.Apply(v =>
        {
            tcs.SetResult(v);
            return v;
        });
        return tcs.Task;
    }
}