#include "NamesDatabase.h"
#include"RandUtils.h"

#include <fstream>
#include<limits>

//const std::string firstNamesFile = "CSV_Database_of_First_Names.csv";
//const std::string lastNamesFile = "CSV_Database_of_Last_Names.csv";

std::string NamesDatabase::GetRandomFirstName() {
	return GetFirstNameAtIndex(RandInt(fNameCount));
}
std::string NamesDatabase::GetRandomLastName() {
	return GetLastNameAtIndex(RandInt(lNameCount));
}

std::string NamesDatabase::GetFirstNameAtIndex(int index) {
	return ReadLineFromCsv("John", index, "CSV_Database_of_First_Names.csv");
}
std::string NamesDatabase::GetLastNameAtIndex(int index) {
	return ReadLineFromCsv("Doe", index, "CSV_Database_of_Last_Names.csv");
}

std::string NamesDatabase::ReadLineFromCsv(std::string defaultRet, int index, std::string filePath) {
	std::ifstream myfile(filePath);
	if (myfile) {  // same as: if (myfile.good())
		std::string line = "";
		GotoLine(myfile, index);
		myfile >> line;
		myfile.close();
		return line;
	}
	return defaultRet;
}

std::string NamesDatabase::AssembleRandomFullName() {
	return (GetRandomFirstName() + " " + GetRandomLastName());
}

std::ifstream& NamesDatabase::GotoLine(std::ifstream& file, int num) {
	file.seekg(std::ios::beg);
	for (int i = 0; i < num - 1; ++i) {
		file.ignore(std::numeric_limits<std::streamsize>::max(), '\r');
	}
	return file;
}