using System.Collections.Generic;

namespace Transport_Management_System
{
    abstract class Information
    {
        public abstract string Title { get; }
        public abstract List<string> ColumnHeaders { get; }
        public abstract List<dynamic> GetRow();
    }
}
