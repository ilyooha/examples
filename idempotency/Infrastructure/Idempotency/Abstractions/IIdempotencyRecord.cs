using System;

namespace Infrastructure.Idempotency.Abstractions
{
    /// <summary>
    /// Для того, чтобы выполнять операции идемпотентно (например, создание объектов).
    /// Запись хранит информацию о пользователе (<see cref="UserId"/>), используемом им значении Idempotency-Key
    /// (<see cref="IdempotencyKey"/>) в разрезе операции (<see cref="Scope"/>) и результате выполнения
    /// этой операции (<see cref="Result"/>).
    /// Таким образом, при повторных попытках выполнить одну и ту же операцию с одинаковым Idempotency-Key достаточно
    /// прочесть ранее полученный результат из такой записи, не выполняя операцию повторно.
    /// </summary>
    public interface IIdempotencyRecord
    {
        public string UserId { get; }
        public string Scope { get; }
        public string IdempotencyKey { get; }
        public DateTime TimestampUtc { get; }
        public string Result { get; }
    }
}