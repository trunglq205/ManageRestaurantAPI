namespace QLNhaHang.API.Attribute
{
    /// <summary>
    /// Không được phép trống
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class NotEmpty : System.Attribute
    {

    }

    /// <summary>
    /// Độc nhất
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class Unique : System.Attribute
    {

    }

    /// <summary>
    /// Tên thuộc tính
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyName : System.Attribute
    {
        public string Name = string.Empty;
        public PropertyName(string name)
        {
            Name = name;
        }
    }
}
