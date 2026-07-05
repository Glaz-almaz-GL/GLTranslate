using GLTranslate.Abstractions.Languages;
using GLTranslate.Abstractions.Services;

namespace GLTranslate.Abstractions.Translators
{
    /// <summary>
    /// Представляет интерфейс для переводчиков, которые могут выполнять перевод контента между различными языками.
    /// </summary>
    /// <typeparam name="T">Тип контента, который будет переведен (например, string, byte[], и т.д.).</typeparam>
    public interface ITranslator<T> : ITranslationService<T>
    {
        /// <summary>
        /// Название переводчика, используемое для идентификации и логирования.
        /// </summary>
        string Name { get; }
    }
}
