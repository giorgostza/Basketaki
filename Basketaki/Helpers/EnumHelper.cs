using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Basketaki.Helpers
{
    public static class EnumHelper
    {
        public static string GetDisplayName(Enum enumValue)
        {

            var member = enumValue.GetType().GetMember(enumValue.ToString()).FirstOrDefault();

            var displayAttribute = member?.GetCustomAttribute<DisplayAttribute>();

            return displayAttribute?.Name ?? enumValue.ToString();
        }

        public static List<SelectListItem> ToSelectList<TEnum>(TEnum? selectedValue = null) where TEnum : struct, Enum
        {
            return Enum.GetValues<TEnum>().Select(value => new SelectListItem
            {
                Value = value.ToString(),
                Text = GetDisplayName(value),
                Selected = selectedValue.HasValue && value.Equals(selectedValue.Value)

            }).ToList();

        }
    }
}
