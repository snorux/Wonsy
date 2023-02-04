namespace Wonsy.Models
{
    public class MapModel : IEquatable<MapModel>
    {
        public string MapName { get; set; }

        public bool IsMoreThan150MB { get; set; }

        public long FileSize { get; set; }

        bool IEquatable<MapModel>.Equals(MapModel other)
        {
            if (other is null) 
                return false;

            return MapName == other.MapName;
        }

        public override bool Equals(object obj)
            => Equals(obj as MapModel);

        public override int GetHashCode()
            => MapName.GetHashCode();
    }
}
