namespace GLTranslate.Abstractions.Translation;

/// <summary>
/// Represents the public entry point for performing a translation
/// operation.
/// </summary>
/// <remarks>
/// <para>
/// A client is consumer-facing: callers depend on this abstraction instead
/// of any specific provider. An implementation may delegate to a single
/// provider or coordinate several (for example to provide a fallback when
/// one provider fails), without changing this contract.
/// </para>
/// <para>
/// Each concrete operation (text, image, audio, ...) is exposed through its
/// own specialization of this interface, mirroring the corresponding
/// provider capability.
/// </para>
/// <para>
/// Implementations are expected to be immutable and thread-safe.
/// </para>
/// </remarks>
/// <typeparam name="TRequest">
/// The immutable request type describing the operation's parameters.
/// </typeparam>
/// <typeparam name="TResult">
/// The immutable result type describing the operation's outcome.
/// </typeparam>
public interface ITranslationClient<in TRequest, TResult>
    where TRequest : ProviderRequest
    where TResult : ProviderResult
{
    /// <summary>
    /// Performs the operation described by the specified request.
    /// </summary>
    /// <param name="request">
    /// The request describing the operation to perform.
    /// </param>
    /// <param name="cancellationToken">
    /// A token that can be used to cancel the operation.
    /// </param>
    /// <returns>
    /// A task that completes with the result of the operation.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="request"/> is <see langword="null"/>.
    /// </exception>
    Task<TResult> ExecuteAsync(TRequest request, CancellationToken cancellationToken = default);
}
