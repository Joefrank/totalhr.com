using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace totalhr.Shared
{
    public class EnumExtensions
    {
        private static readonly ResourceManager DescriptionResources = new ResourceManager(typeof(totalhr.Resources.EnumDescription));
        private static readonly ResourceManager FurtherInfoResources = new ResourceManager(typeof(totalhr.Resources.FieldFurtherInfo));
        private static readonly ResourceManager FileTypeNameResources = new ResourceManager(typeof(totalhr.Resources.FileTypeNames));

        /// <summary>
        /// Adds extension method to enum Description
        /// </summary>
        /// <param name="enumerator"></param>
        /// <returns></returns>
        public static string Description(Enum enumerator)
        {
            var localizedDescription = DescriptionResources.GetString(enumerator.ToString());
            return localizedDescription ?? enumerator.ToString();
        }


        /// <summary>
        /// Returns further info for each enum if it exists
        /// </summary>
        /// <param name="enumerator"></param>
        /// <returns></returns>
        public static string FurtherInfo(Enum enumerator)
        {
            var localizedDescription = FurtherInfoResources.GetString(enumerator.ToString());
            return localizedDescription ?? enumerator.ToString();
        }

        public static string FileType(Enum enumerator)
        {
            var localizedDescription = FileTypeNameResources.GetString(enumerator.ToString());
            return localizedDescription ?? enumerator.ToString();
        }

        public static string FileType(string fileType)
        {
            if (string.IsNullOrEmpty(fileType)) return "";

            var localizedDescription = FileTypeNameResources.GetString(fileType);
            return localizedDescription ?? fileType;
        }
    }
}
