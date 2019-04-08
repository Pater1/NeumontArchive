#include <string>
#include <iostream>
#include "DayTime.h"

std::string testStrings[] = {
	"",
	"hi",
	"23:50:25",
	"1:34:2 pm",
	"1:34:2 am",
	"11:34:2 zm"
};

int main() {
	for each(std::string str in testStrings) {
		DayTime tim(str);

		if (tim.IsValid()) {
			std::cout << "Y ";
		} else {
			std::cout << "N ";
		}
		std::cout << "[\"" << str << "\"] \n	";

		tim.PrintTimes(&std::cout);

		if (tim.IsValid()) {
			std::cout << ".\n	Adding 3601 seconds... ";
			tim.AddTime(0, 0, 3601);

			std::cout << "\n	Resulting time int 24hr format is: " << tim.Get24hrTime();
		}

		std::cout << "\n\n";
	}
	return 0;
}