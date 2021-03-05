using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace API_Training3.App.Helper
{
    public static class GeneralHelper
    {
        public static T MergeData<T>(this object newData, T originData)
        {
            foreach (PropertyInfo propertyInfo in newData.GetType().GetProperties())
            {
                if (propertyInfo.GetValue(newData, null) != null && originData.GetType().GetProperties().Any(p => p.Name.Equals(propertyInfo.Name)))
                {
                    originData.GetType().GetProperty(propertyInfo.Name).SetValue(originData, propertyInfo.GetValue(newData, null));
                }
            }
            return originData;
        }
    }
}
