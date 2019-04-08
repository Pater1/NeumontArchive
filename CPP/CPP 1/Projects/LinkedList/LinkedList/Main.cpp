#include"WordCounter.h"
#include<iostream>
#include"MemTester.h"
#include <fstream> 

int main() {
	//std::istream* istr = new std::cin();
	std::istream* istr = new std::fstream("test.txt");
	WordCounter wordCount(*istr);
	delete istr;

	std::cout << wordCount << "\n";
}