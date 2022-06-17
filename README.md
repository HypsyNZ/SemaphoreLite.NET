```cs
using SemaphoreLite;
```
[![Wiki](https://user-images.githubusercontent.com/54571583/173321360-737e4e55-0e46-40aa-ac4e-0ac01875ce96.png)](https://github.com/HypsyNZ/SemaphoreLite.NET/wiki) [![Nuget](https://img.shields.io/nuget/v/SemaphoreLite.NET)](https://www.nuget.org/packages/SemaphoreLite.NET/)

![k](https://user-images.githubusercontent.com/54571583/174004545-eb25d721-760f-4cdf-9920-df44035737d0.png)

# [Usage Example](https://github.com/HypsyNZ/SemaphoreLite/blob/main/Example/Example/Program.cs)

Create a new `SemaphoreLight`

```cs
private static SemaphoreLight light = new SemaphoreLight();
```

Checking if the `SemaphoreLight` has been `Taken`
```cs
light.IsTakenAsync();
```

Pass `SomeAsyncTask` as a parameter to `IsTakenAsync()` and if the `SemaphoreLight` hasn't been taken then `SomeAsyncTask` will run
```cs
private static async Task SomeAsyncTask()
{
    // Inside the Critical Section
    await Task.Delay(100).ConfigureAwait(false);
}

var callerRequiredToReleaseSemaphore = await semaphoreLight.IsTakenAsync(SomeAsyncTask, false).ConfigureAwait(false);
if (callerRequiredToReleaseSemaphore)
{
    // You can also do work here before you release, This is also inside the Critical Section
    semaphoreLight.Release();
}
```

Release the `SemaphoreLight` when your `Task` returns
```cs
light.Release();
```

## Ordering/Notes

Order of Execution is not Guaranteed, First `Task` into the `Lock` wins.

`SemaphoreLite` does not Order the `Tasks` in any way, It doesn't even try. Please understand this if you try to use `Multiple Tasks` with a single `SemaphoreLite` and set artificial delays on your `Tasks`, This behavior is standard for `Semaphores` I just wanted to mention it somewhere.

In other words, It is possible for the same `Task` to run multiple times in a row, but only one `Task` will ever be running the `Critical Section`