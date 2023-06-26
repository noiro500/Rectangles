namespace Rectangles.Infrastructure
{
    internal interface IRectangleFigure
    {
        Rectangle Rectangle { get; }
        Color RecColor { get; }
        int RemovalCycles { get; set; }
        Random random { get; }
        bool CheckIntersections(RectangleFigure rec);
    }
}
