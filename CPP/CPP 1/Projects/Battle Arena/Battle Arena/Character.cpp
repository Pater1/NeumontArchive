#include<string>
#include<iostream>

#include"Character.h"
#include"RandUtils.h"
#include"Attack.h"

#include "DeleteProtectionWrapper.h"

void Character::ConvenienceBaseClone(Character * from, Character * to){
	to->characterName = from->characterName;
}

Character* Character::DeleteProtect(Character* from){
	return new DeleteProtectionWrapper(from);
}

Character::Character(int HP_max) : maxHP(HP_max), curHP(maxHP){
	money = new Currency(10, 0, 0, 0);
}
Character::~Character() {
	if (attacks.size() > 0) {
		for (int i = (int)attacks.size() - 1; i >= 0; i--) {
			delete attacks[i];
		}
	}
}

double Character::XPDrop(){
	return level * ((wins<1? 1: wins)/(double)(losses<1? 1: losses));
}

void Character::XPPickup(double xp){
	curXP += xp;
	if (curXP >= xpToNextLevel) {
		curXP -= xpToNextLevel;
		xpToNextLevel *= 2;
		level++;
		LevelUp(level);
	}
}

Currency & Character::GetMoney(){
	return *money;
}

int Character::GetEditWins(int delta){
	wins += delta;
	return wins;
}

int Character::GetEditLosses(int delta){
	losses += delta; 
	return losses;
}

void Character::Revive(std::vector<Character*>* , std::vector<Character*>* , std::ostream* ) { 
	curHP = maxHP; 
}

bool Character::ThrowAttack(int atkIndex, std::vector<Character*>* allys, std::vector<Character*>* targets, std::ostream* out){
	if (PeekDead()) {
		if (out != nullptr)
			*FullName(out) << " is dead, and will do nothing this turn.\n";
		return false;
	}

	if (atkIndex < 0) atkIndex = RandInt((int)attacks.size());
	return (attacks[atkIndex]->Throw(this, allys, targets, out));
}

int Character::AlterHP(double HPDelta, std::ostream* out) {
	if (hidden && HPDelta < 0) {
		if (out != NULL)
			*FullName(out) << " avoids damage by hiding.\n";
		return 0;
	} else {
		int delta = (int)(HPDelta * maxHP);

		if ((delta < 0 ? delta*-1 : delta) < 1) {
			delta = (delta < 0 ? 1 : -1);
		}

		if (out != NULL)
			*FullName(out) << (delta < 0 ? " loses " : " heals ") << (delta < 0 ? delta*-1 : delta) << "hp!\n";

		TurnDelta_HP += delta;

		return delta;
	}
}
int Character::AlterHP(int delta, std::ostream* out) {
	if (hidden && delta < 0) {
		if (out != NULL)
			*FullName(out) << " avoids damage by hiding.\n";
		return 0;
	}else{
		if (out != NULL)
			*FullName(out) << (delta < 0 ? " loses " : " heals ") << (delta < 0 ? delta*-1 : delta) << "hp!\n";

		TurnDelta_HP += delta;

		return TurnDelta_HP;
	}
}

std::ostream* Character::FullName(std::ostream* out){
	if (out == NULL || out == nullptr) return out;

	return &(*out << characterName << ", the " << className);
}

bool Character::CheckDead(std::vector<Character*>* , std::vector<Character*>* , std::ostream* out){
	curHP += TurnDelta_HP;
	TurnDelta_HP = 0;

	if (curHP > maxHP) curHP = maxHP;
	if (curHP < 0) curHP = 0;

	bool b = (curHP <= 0);

	if(out != NULL){
		if (b) {
			*out << characterName << " is Dead!\n";
		} else {
			*out << characterName << " has " << curHP << "hp!\n";
		}
	}

	return b;
}

bool Character::PeekDead() {
	return ((curHP + TurnDelta_HP) <= 0);
}
int Character::PeekHP() {
	return curHP;
}