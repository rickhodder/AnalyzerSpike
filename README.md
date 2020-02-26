# AnalyzerSpike

Spike of creating a rosyln analyzer that generates warnings when a class inherits from RickHodder.ApiEndpoint and has more than one public method
NOTE: Constructors dont count as public methods

Before trying to use this analyzer, you must go into VS 2019 installer, make sure Visual Studio extension development is checked, and in the items on the right, .NET Compiler Platform is checked

Once that is done, you can open the solution and press F5 and it will open a second VS 2019, and if you create a console project, and include the samples below, you will see squiggles under the type name.

Check out the unit tests if you dont want to run the analyzer. 

# Code Examples

```C#
// This doesnt generate a warning because it onlyn has one public method
public class ExampleOne: RickHodder.ApiEndpoint
{
  public void MethodOne()
  {
  }
}

// This doesnt generate a warning because it onlyn has one public method, 
// and a constructor, which doesnt count 
public class ExampleTwo: RickHodder.ApiEndpoint
{
  public ExampleTwo()
  {
  }
  public void MethodOne()
  {
  }
}

// This generates a warning because it has more than one public method, 
public class ExampleThree: RickHodder.ApiEndpoint
{
  public void MethodOne()
  {
  }
  
  public void MethodTwo()
  {
  }
}

namespace RickHodder
{
  public class ApiEndpoint
  {
  }
}
