namespace DerivedXmlSerializer.Graphs
{
    public interface IMarker<in T>
    {
        bool Mark(T v);
        bool IsMarked(T v);
    }
}