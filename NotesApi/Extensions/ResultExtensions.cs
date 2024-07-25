namespace NotesApi.Extensions;

public static class ResultExtensions
{
    public static Result<TNew> OnSuccess<T, TNew>(this Result<T> result, Func<T, Result<TNew>> func)
    {
        return result.IsSuccess ? func(result.Value) : Result<TNew>.Failure(result.Error);
    }

    public static Result<T> OnFailure<T>(this Result<T> result, Action<string> action)
    {
        if (!result.IsSuccess)
        {
            action(result.Error);
        }
        return result;
    }

    public static Result<T> OnBoth<T>(this Result<T> result, Action<Result<T>> action)
    {
        action(result);
        return result;
    }
}
