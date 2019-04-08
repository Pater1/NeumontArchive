#pragma once
#include<string>

class NamesDatabase{
	public:
		static const int fNameCount = 5493;
		//static const std::string firstNamesFile;

		static const int lNameCount = 887899;
		//static const std::string lastNamesFile;

		static std::string GetFirstNameAtIndex(int index); 
		static std::string GetLastNameAtIndex(int index);

		static std::string NamesDatabase::GetRandomFirstName();
		static std::string NamesDatabase::GetRandomLastName();

		static std::string AssembleRandomFullName();

	private:
		static std::string NamesDatabase::ReadLineFromCsv(std::string defaultRet, int index, std::string filePath);
		static std::ifstream& NamesDatabase::GotoLine(std::ifstream& file, int num);
};

