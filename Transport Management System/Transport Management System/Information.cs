using System.Collections.Generic;

namespace Transport_Management_System
{
    abstract class Information
    {
        public abstract string Title { get; }
        public abstract List<string> ColumnHeaders { get; }
        public abstract List<dynamic> GetRow();
        public virtual List<List<dynamic>> GetSpecific() 
        {
            return new List<List<dynamic>>();
        }
        public virtual List<dynamic> GetValues()
        {
            return new List<dynamic>();
        }
        public abstract void SetValues(dynamic[] values);
    }
}
