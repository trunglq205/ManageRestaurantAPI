using QLNhaHang.API.Attribute;
using QLNhaHang.API.Entities;
using QLNhaHang.API.Exceptions;
using System.Reflection;

namespace QLNhaHang.API.Utils
{
    public static class EntityUtils<Entity>
    {
        /// <summary>
        /// Lấy ra tên thuộc tính
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static string GetDisplayName(PropertyInfo property)
        {
            var nameDisplay = string.Empty;
            //Lấy ra tên của property:
            var propertyNames = property.GetCustomAttributes(typeof(PropertyName), true);
            if (propertyNames.Length > 0)
            {
                nameDisplay = (propertyNames[0] as PropertyName).Name;
            }
            return nameDisplay;
        }

        /// <summary>
        /// Validate dữ liệu
        /// </summary>
        /// <param name="entity"></param>
        /// <exception cref="QLNhaHangException"></exception>
        public static void ValidateData(Entity entity)
        {
            // Lấy ra tất cả các property
            var properties = typeof(Entity).GetProperties();

            //Lấy ra các property được đánh dấu không được phép để trống - có Attribute là NotEmpty:
            var propNotEmpties = typeof(Entity).GetProperties().Where(p => System.Attribute.IsDefined(p, typeof(NotEmpty)));
            foreach (var property in propNotEmpties)
            {
                var propValue = property.GetValue(entity);
                var propName = property.Name;
                //Lấy ra tên của thuộc tính
                var nameDisplay = GetDisplayName(property);
                //Kiểm tra xem thuộc tính có bị trống hay không?
                if (propValue == null || string.IsNullOrEmpty(propValue.ToString()))
                {
                    nameDisplay = nameDisplay == string.Empty ? propName : nameDisplay;
                    throw new QLNhaHangException(String.Format(Resource.QLNhaHangResource.Property_NotEmpty, nameDisplay));
                }
            }
        }

        /// <summary>
        /// Bắt lỗi
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="userMessage"></param>
        /// <returns></returns>
        public static object CatchValidateException(Exception ex, string userMessage, Entity? entity)
        {
            var response = new
            {
                devMsg = ex.Message,
                userMsg = userMessage,
                data = entity
            };
            return response;
        }
        public static object CatchValidateException(Exception ex, string userMessage)
        {
            var response = new
            {
                devMsg = ex.Message,
                userMsg = userMessage
            };
            return response;
        }
    }
}
