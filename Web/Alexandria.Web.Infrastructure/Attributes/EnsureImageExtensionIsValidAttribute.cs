namespace Alexandria.Web.Infrastructure.Attributes
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;

    using Alexandria.Common;
    using Microsoft.AspNetCore.Http;

    public class EnsureImageExtensionIsValidAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var image = value as IFormFile;

            if (image != null)
            {
                var extension = Path.GetExtension(image.FileName).TrimStart('.');
                var list = GlobalConstants.AllowableExtensions
                    .Split(",", StringSplitOptions.RemoveEmptyEntries)
                    .ToArray();

                if (!list.Contains(extension))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            return true;
        }
    }
}
