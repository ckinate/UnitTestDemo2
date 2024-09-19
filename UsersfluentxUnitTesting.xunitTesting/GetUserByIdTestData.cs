using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersfluentxUnitTesting.xunitTesting
{
    // Defining a class GetUserByIdTestData that implements IEnumerable<object[]>
    public class GetUserByIdTestData : IEnumerable<object[]>
    {
        // Implementing the GetEnumerator method to provide test data
        // This method returns an enumerator that iterates through a collection of object[] arrays.
        // It is required by the IEnumerable<object[]> interface.
        public IEnumerator<object[]> GetEnumerator()
        {
            // Using yield return to provide test cases
            // Each test case is represented as an object array with the expected parameters and result
            yield return new object[] { 1, true }; // Test case with userId 1 and expected result true
            yield return new object[] { 99, false }; // Test case with userId 99 and expected result false
        }
        // This is an explicit implementation of the non-generic IEnumerable.GetEnumerator method.
        // It simply calls the generic GetEnumerator method.
        // Explicit interface implementation is used here to provide an implementation for the non-generic IEnumerable interface,
        // which is also required.
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
