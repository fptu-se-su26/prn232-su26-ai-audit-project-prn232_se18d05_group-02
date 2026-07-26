namespace WanderXServer.Services;
public interface ISmsSender { Task<string?> SendAsync(string phoneNumber,string message,CancellationToken cancellationToken=default); }
public sealed class UnconfiguredSmsSender : ISmsSender { public Task<string?> SendAsync(string phoneNumber,string message,CancellationToken cancellationToken=default)=>throw new InvalidOperationException("SMS_PROVIDER_ERROR"); }