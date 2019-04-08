#ifndef DEBUGUTILITY_H_
#define DEBUGUTILITY_H_
/*
Pat Code
*/
#include <stdarg.h>
#include <stdio.h>
/*
End Pat Code
*/

// Robert Lamb
// 4/1/2017
// Enables easy and useful printing of variables for learning purposes

// Note: you can use p, p2, p3, and p4 to debug print 1-4 variables
// You must #include <iostream> to use these

namespace DebugUtility
    {
    // #defines are generally bad, but they can do good in a limited set of circumstances
#define p(x)        DebugPrint(__LINE__,#x,(x))
#define p2(x,y)     DebugPrint(__LINE__,#x,(x),#y,(y))
#define p3(x,y,z)   DebugPrint(__LINE__,#x,(x),#y,(y),#z,(z))
#define p4(w,x,y,z) DebugPrint(__LINE__,#w,(w),#x,(x),#y,(y),#z,(z))
	/*
	Pat Code
	*/
//#define pn(...) VariadicDebugPrint(__LINE__, #__VA_ARGS__, __VA_ARGS__)
	/*
	End Pat Code
	*/
    // add more as needed using the examples above

	/*
		Pat Code
	*/
	//template <typename T1, typename I, typename... Targs>
	//void VariadicDebugPrint(T1 lineNum, int count, Targs ...) {
	//	va_list ap1;

	//	va_start(ap1, count); //Requires the last fixed parameter (to get the address)
	//	for (int j = 0; j<count; j++)
	//		DebugPrint(lineNum, va_arg(ap1), va_arg(ap1)); //Requires the type to cast to. Increments ap to the next argument.
	//	va_end(ap);
	//}
	/*
		End Pat Code
	*/

    // zero args - should not happen
    inline void DebugPrint() { std::cout << "Macro Error - Expected arguments!\n"; }

    // one arg - this is the expected conclusion of printing
    template<typename T>
    void DebugPrint(T) { std::cout << "----------\n"; }

    // two args - should not happen
    template<typename T1, typename T2> // should not happen
    void DebugPrint(T1 arg1, T2 arg2) 
        { std::cerr << "Macro Error - Bad Number of Args!\n----------\n"; }

    // three or more args - this prints a debug line of data with the following format
    // lineNumber: variableName = variableValue
	template <typename T1, typename T2, typename T3, typename... Targs>
	void DebugPrint(T1 lineNum, T2 name, T3 value, Targs... args)
	{
		std::cout << lineNum << ": " << name << " = " << value << "\n";
		DebugPrint(lineNum, args...);
	}
};

using DebugUtility::DebugPrint;

#endif // ndef DEBUGUTILITY_H_
