namespace Track.Order.Common.Errors;

internal class IturriErrorHttpStatusComparer : IComparer<IturriError>
{
    public int Compare(IturriError? x, IturriError? y)
        => (int)(x?.HttpStatus.CompareTo(y?.HttpStatus ?? 0));
}
