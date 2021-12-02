using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Lime.Messaging.Contents;

namespace Take.Api.Tudologia.Tests.EqualityComparers
{
    public class SelectOptionsEqualityComparer : IEqualityComparer<SelectOption>
    {
        public bool Equals([AllowNull] SelectOption x, [AllowNull] SelectOption y)
        {
            return x.Text.Equals(y.Text);
        }

        public int GetHashCode([DisallowNull] SelectOption obj)
        {
            return base.GetHashCode();
        }
    }
}
