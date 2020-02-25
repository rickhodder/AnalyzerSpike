using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TestHelper;
using AnalyzerSpike;

namespace AnalyzerSpike.Test
{
    [TestClass]
    public class AnalyzerTest : DiagnosticVerifier
    {
        [TestMethod]
        public void ShouldNotRaiseWarning_OnlyOneMethod()
        {
            var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        public class TypeName : RickHodder.ApiEndpoint
        {   
            public void OneMethod()
            {
            }
        }
    }

    namespace RickHodder
    {
        public class ApiEndpoint
        {
        }
    }
";
            VerifyCSharpDiagnostic(test);

        }

        [TestMethod]
        public void ShouldNotRaiseWarning_ConstructorNotConsideredPublicMethod()
        {
            var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        public class TypeName : RickHodder.ApiEndpoint
        {   
            public TypeName()
            {
            }

            public void OneMethod()
            {
            }
        }
    }

    namespace RickHodder
    {
        public class ApiEndpoint
        {
        }
    }
";
            VerifyCSharpDiagnostic(test);

        }

        [TestMethod]
        public void ShouldRaiseWarning_MoreThanOnePublicMethod()
        {
            var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        public class TypeName : RickHodder.ApiEndpoint
        {   
            public void OneMethod()
            {
            }

            public void TwoMethod()
            {
            }

        }
    }

    namespace RickHodder
    {
        public class ApiEndpoint
        {
        }
    }
";

            var expected = new DiagnosticResult
            {
                Id = "AnalyzerSpike",
                Message = String.Format("Type '{0}' inherits from ApiEndpoint and has more than one public method", "TypeName"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                        new DiagnosticResultLocation("Test0.cs", 11, 22)
                    }
            };

            VerifyCSharpDiagnostic(test, expected);

        }


        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new AnalyzerSpikeAnalyzer();
        }

    }

    // No codefixprovider, so no test class needed
    /*
    [TestClass]
    public class UnitTest : CodeFixVerifier
    {

        //No diagnostics expected to show up
        [TestMethod]
        public void TestMethod1()
        {
            var test = @"";

            VerifyCSharpDiagnostic(test);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public void TestMethod2()
        {
            var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class TypeName
        {   
        }
    }";
            var expected = new DiagnosticResult
            {
                Id = "AnalyzerSpike",
                Message = String.Format("Type name '{0}' contains lowercase letters", "TypeName"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 11, 15)
                        }
            };

            VerifyCSharpDiagnostic(test, expected);

            var fixtest = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class TYPENAME
        {   
        }
    }";
            VerifyCSharpFix(test, fixtest);
        }

        //protected override CodeFixProvider GetCSharpCodeFixProvider()
        //{
        //    return new AnalyzerSpikeCodeFixProvider();
        //}

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new AnalyzerSpikeAnalyzer();
        }
    }
    */
}
