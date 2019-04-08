#include <iostream>
#include <string>
#include <stdlib.h>
#include <ctype.h>
#include <chrono>
#include <conio.h>

std::string* classes = new std::string[5]{
	"Cleric",
	"Barbarian",
	"Wizard",
	"Rogue",
	"Necromancer"
};

std::string* modifiers = new std::string[5]{
	"Pious",
	"Strong",
	"Wise",
	"Wily",
	"Vile"
};

std::string** names = new std::string*[5]{
	new std::string[3]{
		"Holy Blacksmith, Leokul",
		"Lady Victaatis Carter, Cleric of Many Gods",
		"Cleric Gramorn Tarmikos the Undaunted"
	},
	new std::string[3]{
		"Lady Pruatis Chandler, Lightning-Bearer",
		"Unaerris Pegason the Valet",
		"Mad Barmaid, Wandaonna Dagarkin"
	},
	new std::string[3]{
		"Arkane Baker, Ravaatris",
		"Lady Yllaora Huntinghawk the Powerful",
		"Lord Oloneiros Cupshigh the Merciless"
	},
	new std::string[3]{
		"Pursediver, Pruatra Shipsail",
		"Yllaerris Trickfoot the Wheeler",
		"Nerisella Huntinghawk, Silent Tyrantfeller"
	},
	new std::string[3]{
		"Fallen Monk, Pettumal Duskwalker",
		"Corpsemaker Brekhar",
		"Amuxir Atheir, The Rotting"
	},
};

std::string* blurbs = new std::string[5]{
	"Pray your God watches over you!",
	"Let your rage overwhelm!",
	"May the mana flow strong today.",
	"Stick to the shadows...",
	"Don't be burried yourself."
};

int RandInt(int max, int min = 0) {
	int r = rand();
	r %= (max - min);
	r += min;
	return r;
}

float RandFloat(float max = 1, float min = 0) {
	int ra = rand();
	float nd = (float)ra / RAND_MAX;

	nd *= (max - min);
	nd += min;

	return nd;
}

int Login() {
	std::string name = "Dargon", pass = "SkyRim123", testName = "", testPass = "";
	for (int i = 0; i < 3; i++) {
		std::cout << "Username: ";
		testName = "";
		std::cin >> testName;
		std::cout << "Password: ";

		testPass = "";
		char ch;
		ch = _getch();
		while (ch != 13) {//character 13 is enter
			testPass.push_back(ch);
			std::cout << '*';
			ch = _getch();
		}
		std::cout << "\n";

		if (testName == name && testPass == pass) {
			std::cout << "Welcome, " << testName << "!\n";
			return 0;
		}
		else if(i < 2) {
			std::cout << "Unknown user or incorrect password! Please try again...\n";
		}
	}
	
	std::cout << "You have been disconnected, please try again later...\n\n";
	return -1;
}

int SelectClass() {
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

	if (selection < 0) {
		std::cout << "\nExiting Lobby...";
	}
	else {
		std::cout << "\nWelcome, " << classes[selection] << "! " + blurbs[selection] << "\n\n";
	}

	return selection;
}

std::string GetName(int classSelection) {
	std::cout << modifiers[classSelection] << " " << classes[classSelection] << ", pray, tell us your name!\n\n";
	std::string name = "";

	do {
		std::cout << "What is your name? ";
		std::cin >> name;
		std::cout << "\n";

		if (!isalpha(name[0])) {
			std::cout << "\nCome now " << 
				((RandFloat() <= 0.25f)? modifiers[classSelection] + " " : "")
				<< classes[classSelection] << "! That could not possibly be your name.\nPlease tell us true your name.\n\n";
		}
	} while (!isalpha(name[0]));

	std::cout << "Well met, " << name << " the " << classes[classSelection] << ".\n\n";
	return name;
}

int* SelectEnemy(int classSelection) {
	int enemySelection = 0;

	do {
		enemySelection = RandInt(5);
	} while (enemySelection == classSelection);

	int enemyName = RandInt(3);

	return new int[2] {
		enemySelection,
		enemyName
	};
}

void Cleanup() {
	delete[] classes;
	delete[] modifiers;
	delete[] blurbs;
	for (int i = 0; i < 5; i++) {
		delete[] names[i];
	}
	delete[] names;
}

int main() {
	srand((unsigned int)(std::chrono::system_clock::now().time_since_epoch().count()));

	int i = Login();
	if (i != 0) return i;

	int classSelection = SelectClass();
	if (classSelection < 0) return -2;

	std::string playerName = "";
	playerName = GetName(classSelection);

	int* enemyInfo = 
		SelectEnemy(classSelection);

	std::cout << playerName << ", you shall engage " << names[enemyInfo[0]][enemyInfo[1]] <<
		", " << ((RandFloat() <= 0.25f) ? "our most " + modifiers[enemyInfo[0]] : "the")
		<< " " << classes[enemyInfo[0]] << " in battle!\n\n"
		<< ((RandFloat() > 0.5f) ? blurbs[classSelection] + "\n\n" : "");

	delete[] enemyInfo;
	Cleanup();

	return 0;
}