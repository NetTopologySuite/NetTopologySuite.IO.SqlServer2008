using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NetTopologySuite.IO.SqlServer2008.Test
{
    class Issue174
    {
        [Test, Category("Issue174")]
        public void ensure_NetTopologySuite_IO_SqlServer2008_assembly_is_strongly_named()
        {
            AssertStronglyNamedAssembly(typeof(MsSql2008GeometryReader));
        }

        private void AssertStronglyNamedAssembly(Type typeFromAssemblyToCheck)
        {
            Assert.IsNotNull(typeFromAssemblyToCheck, "Cannot determine assembly from null");
            Assembly assembly = typeFromAssemblyToCheck.Assembly;
            StringAssert.DoesNotContain("PublicKeyToken=null", assembly.FullName, "Strongly named assembly should have a PublicKeyToken in fully qualified name");
        }
    }
}
