namespace LiveArt.Design.PostProcessing.Domain
{
    internal class Location
    {
        public string name;
        public string svg;
        public string editableArea;//string in format "x1 y1 x2 y2"

        // TODO: remove this durty hack:used to pass h/w from png generation to pdf generation
        internal int? height;
        internal int? width;

    }
}
