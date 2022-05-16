using System.Collections.Generic;

namespace Transport_Management_System
{
    abstract class Information
    {
        public abstract string Title { get; }
        public abstract List<string> ColumnHeaders { get; }
        public virtual List<string> IDColumnHeaders { get; }
        public abstract int ID { get; }
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

        // Statistics properties
        public virtual List<string> Specialities { get; }
        public virtual string TransportType { get; }
        public virtual string TransportCondition { get; }
    }
}
