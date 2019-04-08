#include "MaxFind.h"
#include <iostream>
#include <string>

int main() {
	std::cout << "max of 1 and 2 is " << FindMaximum(1, 2) << "\n\n";

	int i[4] = {1, 2, 3, 4};
	std::cout << "max of 1, 2, 3 and 4 is... (using array) " << FindMaximum(i, 4) << "\n\n";

	std::cout << "max of 1, 2, 3 and 4 is... (using varidaic) " << FindMaximum(1, 4, 3, 2) << "\n\n";

	std::cout << "max of 1.01, 2.24, 1.53 and 2.21 is... (using varidaic) " << FindMaximum(1.01, 2.24, 1.53, 2.21) << "\n\n";

	std::cout << "max of 'Hi', 'Hello', 'There' and 'World' is... (using varidaic) " << FindMaximum("Hi", "Hello", "There", "World") << "\n\n";
}