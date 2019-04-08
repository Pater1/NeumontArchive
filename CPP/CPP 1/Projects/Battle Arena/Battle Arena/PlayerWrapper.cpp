#include "PlayerWrapper.h"
#include<iostream>

PlayerWrapper::PlayerWrapper(Character* cha): Character(0){
	trueCharacter = cha;
}

PlayerWrapper::~PlayerWrapper(){
	delete trueCharacter;
}


double PlayerWrapper::XPDrop() {
	return trueCharacter->XPDrop();
}

void PlayerWrapper::XPPickup(double xp) {
	trueCharacter->XPPickup(xp);
	if (trueCharacter->level > level) LevelUp(trueCharacter->level);
}

bool PlayerWrapper::ThrowAttack(int atkIndex, std::vector<Character*>* allys, std::vector<Character*>* targets, std::ostream* out) {
	//prompt player for move?
	return trueCharacter->ThrowAttack(atkIndex, allys, targets, out);
}

int PlayerWrapper::AlterHP(double HPDelta, std::ostream* out) {
	return trueCharacter->AlterHP(HPDelta, out);
}
int PlayerWrapper::AlterHP(int delta, std::ostream* out) {
	return trueCharacter->AlterHP(delta, out);
}

std::ostream* PlayerWrapper::FullName(std::ostream* out) {
	return trueCharacter->FullName(out);
}

bool PlayerWrapper::CheckDead(std::vector<Character*>* allys, std::vector<Character*>* targets, std::ostream* out) {
	return trueCharacter->CheckDead(allys, targets, out);
}

bool PlayerWrapper::PeekDead() {
	return trueCharacter->PeekDead();
}
int PlayerWrapper::PeekHP() {
	return trueCharacter->PeekHP();
}

void PlayerWrapper::Revive(std::vector<Character*>* , std::vector<Character*>* , std::ostream* out){
	if(trueCharacter->curHP <= 0) trueCharacter->curHP = 5;

	if (out != NULL) {
		*trueCharacter->GetMoney().PrintMoney(&(*FullName(&std::cout) << ", you have " << trueCharacter->PeekHP() << "hp, and " )) << "\n Please select the healing item you'd like to purchase:";

		bool valid = true;
		do {
			valid = true;
			int selection;
			do {
				std::cout << "Select Your healing item\n\n"
					<< "	0: Nothing\n"
					<< "	1: Bandages (+5hp, 5 copper)\n"
					<< "	2: Poultices (+10hp, 5 silver)\n"
					<< "	3: Tinctures (+15hp, 1 gold)\n"
					<< "	4: Elixers (20hp, 5 gold)\n"
					<< "	5: Ambrosia (100hp, 1 mythril)\n"
					<< "\nMake your selection! ";
				std::cin >> selection;

				if (selection > 5) {
					std::cout << "\n\nThat is an invalid selection! Please try again...\n";
				}
			} while (selection > 5);

			try {
				switch (selection) {
				case 1:
					GetMoney().AlterByCoinage(-5, 0, 0, 0);
					std::cout << "Bandages purchased.\n";
					AlterHP(5, out);
					break;
				case 2:
					GetMoney().AlterByCoinage(0, -5, 0, 0);
					std::cout << "Poultices purchased.\n";
					AlterHP(10, out);
					break;
				case 3:
					GetMoney().AlterByCoinage(0, 0, -1, 0);
					std::cout << "Tinctures purchased.\n";
					AlterHP(15, out);
					break;
				case 4:
					GetMoney().AlterByCoinage(0, 0, -5, 0);
					std::cout << "Elixers purchased.\n";
					AlterHP(20, out);
					break;
				case 5:
					GetMoney().AlterByCoinage(0, 0, 0, -1);
					std::cout << "Ambrosia purchased.\n";
					AlterHP(100, out);
					break;
				}
			} catch (std::exception const & e) {
				std::cout << "You can't afford that!\n\nPlease try again...\n";
				valid = false;
			}
		} while (!valid);
	}
}

void PlayerWrapper::LevelUp(int lvl) {
	level = trueCharacter->level;

	//if (cachedOut != NULL) {
		std::cout << "------------------------------------------------Level Up!------------------------------------------------\n";
		*FullName(&std::cout) << ", is now level " << lvl << "!\n\n";
	//}

	system("pause");
}

Character* PlayerWrapper::clone() {
	return new PlayerWrapper(trueCharacter->clone());
}

Currency& PlayerWrapper::GetMoney() {
	return trueCharacter->GetMoney();
}

int PlayerWrapper::GetEditWins(int delta) {
	return trueCharacter->GetEditWins(delta);
}
int PlayerWrapper::GetEditLosses(int delta) {
	return trueCharacter->GetEditLosses(delta);
}