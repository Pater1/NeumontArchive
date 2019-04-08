#include "LoginUtils.h"
#include<iostream>
#include <string>
#include <conio.h>

#include "Cleric.h"
#include "Necromancer.h"
#include "Wizard.h"
#include "Barbarian.h"
#include "Rogue.h"

#include "PlayerWrapper.h"

std::string* classes = new std::string[5]{
	"Cleric",
	"Barbarian",
	"Wizard",
	"Rogue",
	"Necromancer"
};

int LoginUtils::Login() {
	std::string name = "Dargon", pass = "SkyRim123", testName = "", testPass = "";
	for (int i = 0; i < 3; i++) {
		std::cout << "Username: ";
		testName = "";
		std::cin >> testName;
		std::cout << "Password: ";

		testPass = "";
		char ch;
		ch = (char)_getch();
		while (ch != 13) {//character 13 is enter
		testPass.push_back(ch);
		std::cout << '*';
		ch = (char)_getch();
	}
	std::cout << "\n";

	if (testName == name && testPass == pass) {
		std::cout << "Welcome, " << testName << "!\n";
		return 0;
	} else if(i < 2) {
		std::cout << "Unknown user or incorrect password! Please try again...\n";
	}
}

	std::cout << "You have been disconnected, please try again later...\n\n";
	return -1;
}

Character* LoginUtils::BuildPlayer() {
	int selection = 0;

	do {
		std::cout << "Select Your class\n\n	0: Exit Lobby\n";
		for (int i = 0; i < 5; i++) {
			std::cout << "	" << (i + 1) << ": " << classes[i] << "\n";
		}
		std::cout << "\nMake your selection! ";
		std::cin >> selection;

		if (selection > 5) {
			std::cout << "\n\nThat is an invalid selection! Please try again...\n";
		}
	} while (selection > 5);

	selection --;

	Character* tmp = nullptr;
	std::string name;

	if (selection < 0) {
		std::cout << "\nExiting Lobby...";
		return nullptr;
	} else {
		std::cout << "\nWelcome, " << classes[selection] << "! Pray, tell us your name...  ";
		std::cin >> name;
	}
		
	if (selection == 0) {
		tmp = new Cleric(name);
	} else if (selection == 1) {
		tmp = new Barbarian(name);
	} else if (selection == 2) {
		tmp = new Wizard(name);
	} else if (selection == 3) {
		tmp = new Rogue(name);
	} else if (selection == 4) {
		tmp = new Necromancer(name);
	}

	return new PlayerWrapper(tmp);
}

Character* LoginUtils::LoginAndAssembleCharacter(std::ostream* cacheOut){
	int i = LoginUtils::Login();
	if (i != 0) return nullptr;

	Character* cha = LoginUtils::BuildPlayer();
	cha->cachedOut = cacheOut;

	return cha;
}
