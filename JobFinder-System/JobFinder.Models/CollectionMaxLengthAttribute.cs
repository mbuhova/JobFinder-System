using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobFinder.Models
{
    class CollectionMaxLengthAttribute : ValidationAttribute
    {
        private int count;

        public CollectionMaxLengthAttribute(int count)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("The count parameter should be a non-negative number.");
            }

            this.count = count;
        }

        public override bool IsValid(object value)
        {
            var collection = value as ICollection;
            if (collection != null)
            {
                return collection.Count <= this.count;
            }
            return false;
        }
    }
}
