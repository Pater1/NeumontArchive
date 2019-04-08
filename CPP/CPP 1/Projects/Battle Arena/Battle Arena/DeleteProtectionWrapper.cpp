#include "DeleteProtectionWrapper.h"

#include<iostream>

DeleteProtectionWrapper::DeleteProtectionWrapper(Character* cha) : Character(0){
	trueCharacter = cha;
	characterName = trueCharacter->characterName + "'s clone";
	className = "Clone";
}


DeleteProtectionWrapper::~DeleteProtectionWrapper(){
}//purposfully don't delete 'trueCharacter' field.

double DeleteProtectionWrapper::XPDrop() {
	return trueCharacter->XPDrop();
}

void DeleteProtectionWrapper::XPPickup(double xp) {
	trueCharacter->XPPickup(xp);
}

Currency & DeleteProtectionWrapper::GetMoney(){
	return trueCharacter->GetMoney();
}

int DeleteProtectionWrapper::GetEditWins(int delta){
	return trueCharacter->GetEditWins(delta);
}

int DeleteProtectionWrapper::GetEditLosses(int delta){
	return trueCharacter->GetEditLosses(delta);
}

bool DeleteProtectionWrapper::ThrowAttack(int atkIndex, std::vector<Character*>* allys, std::vector<Character*>* targets, std::ostream* out) {
	//prompt player for move?
	return trueCharacter->ThrowAttack(atkIndex, allys, targets, out);
}

int DeleteProtectionWrapper::AlterHP(double HPDelta, std::ostream* out) {
	return trueCharacter->AlterHP(HPDelta, out);
}
int DeleteProtectionWrapper::AlterHP(int delta, std::ostream* out) {
	return trueCharacter->AlterHP(delta, out);
}

std::ostream* DeleteProtectionWrapper::FullName(std::ostream* out) {
	return trueCharacter->FullName(out);
}

bool DeleteProtectionWrapper::CheckDead(std::vector<Character*>* allys, std::vector<Character*>* targets, std::ostream* out) {
	return trueCharacter->CheckDead(allys, targets, out);
}

bool DeleteProtectionWrapper::PeekDead() {
	return trueCharacter->PeekDead();
}
int DeleteProtectionWrapper::PeekHP() {
	return trueCharacter->PeekHP();
}

void DeleteProtectionWrapper::Revive(std::vector<Character*>* allys, std::vector<Character*>* targets, std::ostream * out){
	trueCharacter->Revive(allys, targets, out);
}

void DeleteProtectionWrapper::LevelUp(int lvl) {
	trueCharacter->LevelUp(lvl);
}

Character* DeleteProtectionWrapper::clone() {
	return new DeleteProtectionWrapper(trueCharacter);
}